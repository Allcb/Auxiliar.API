using Auxiliar.Configurations;
using Auxiliar.Domain.Core.Settings;
using Auxiliar.Domain.Core.Types;
using Auxiliar.Infra.CrossCutting.ExceptionHandler.Providers;
using Auxiliar.Infra.CrossCutting.IoC;
using Auxiliar.Infra.CrossCutting.Swagger.Providers;
using ElmahCore.Mvc;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;

namespace Auxiliar.Services.API
{
    public class Startup
    {
        #region Constantes Publicas

        public const string ELMAH_BASE_PATH = "/log-errors";

        #endregion Constantes Publicas

        #region Propriedades Publicas

        private readonly IWebHostEnvironment Environment;
        public IConfiguration Configuration { get; }

        #endregion Propriedades Publicas

        #region Construtores Publicos

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        #endregion Construtores Publicos

        #region Metodos Publicos

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            //services.AddAutoMapperSetup();
            //services.AddCustomPolicyProviderConfiguration();
            //services.AddCustomAzureAdConfiguration();

            DefinirCultureInfo();

            services.AddHttpClient();

            services.AddControllers(options =>
            {
                options.UseCentralRoutePrefix(new RouteAttribute("api/v{version:apiVersion}"));
                options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            }).ConfigureApiBehaviorOptions(options =>
              {
                  options.SuppressModelStateInvalidFilter = true;
              })
              .AddNewtonsoftJson(options =>
              {
                  options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                  options.SerializerSettings.Formatting = Formatting.Indented;
                  options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                  options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                  options.UseCamelCasing(true);
              });

            services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters();

            int? _apiVersion = Configuration.GetSection(nameof(ApplicationSettings))?
                                            .Get<ApplicationSettings>()?.ApiVersion;

            services.AddApiVersioning(options =>
             {
                 options.ReportApiVersions = true;
                 options.AssumeDefaultVersionWhenUnspecified = true;
                 options.DefaultApiVersion = new ApiVersion(majorVersion: _apiVersion ?? 1,
                                                            minorVersion: 0);
                 options.UseApiBehavior = false;
                 options.ErrorResponses = new ApiVersionExceptionHandler();
             });

            services.AddSwaggerConfiguration();
            services.AddHttpContextAccessor();

            services.AddMediatR(typeof(Startup));
            services.AddElmah(options => options.Path = ELMAH_BASE_PATH);
            RegisterServices(services);
            RegisterSettings(services);
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            // Configure the HTTP request pipeline.
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseExceptionHandlerMiddleware();
            app.UseSwaggerConfiguration();

            app.UseCors(corsPolicyBuilder =>
            {
                corsPolicyBuilder.AllowAnyHeader();
                corsPolicyBuilder.AllowAnyMethod();
                corsPolicyBuilder.AllowAnyOrigin();
                corsPolicyBuilder.WithExposedHeaders(HeadersTypes.TOTAL_QUANTIDADE, HeadersTypes.WWW_AUTHENTICATE);
            });

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseWhen(context => context.Request.Path.StartsWithSegments(ELMAH_BASE_PATH, StringComparison.OrdinalIgnoreCase),
               appBuilder => appBuilder.Use(next =>
               {
                   return async context =>
                   {
                       context.Features.Get<IHttpBodyControlFeature>().AllowSynchronousIO = true;

                       await next(context);
                   };
               }));

            app.UseElmah();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #endregion Metodos Publicos

        #region Metodos Privados

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
            NativeInjectorBootStrapper.RegisterServices(services);
        }

        private void RegisterSettings(IServiceCollection services)
        {
            services.Configure<TokenConfigurationsSettings>(Configuration.GetSection(nameof(TokenConfigurationsSettings)));
        }

        private void DefinirCultureInfo()
        {
            CultureInfo _cultureInfo = new CultureInfo("pt-BR");

            CultureInfo.DefaultThreadCurrentCulture = _cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = _cultureInfo;
        }

        #endregion Metodos Privados
    }
}