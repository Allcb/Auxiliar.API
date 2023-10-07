using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Auxiliar.Infra.CrossCutting.Swagger.Providers
{
    public static class SwaggerConfiguration
    {
        #region Metodos Publicos

        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(name: "v1",
                                   info: new OpenApiInfo
                                   {
                                       Title = "Auxiliar API",
                                       Version = "v1",
                                       Description = "API REST desenvolvida com ASP .NET 7.0 para a aplicação <b>Auxiliar</b>.",
                                       Contact = new OpenApiContact
                                       {
                                           Name = "Allan Carvalho",
                                           Email = "contato@teste.com.br",
                                           Url = new Uri("http://www.teste.com.br")
                                       }
                                   });

                options.AddSecurityDefinition(name: "Bearer",
                                              securityScheme: new OpenApiSecurityScheme
                                              {
                                                  In = ParameterLocation.Header,
                                                  Type = SecuritySchemeType.Http,
                                                  Scheme = "bearer",
                                                  BearerFormat = "JWT",
                                                  Name = "Authorization",
                                                  Description = "Utilização: Escreva '{seuToken}'"
                                              });

                options.AddSecurityDefinition(name: "ApiKey",
                                              securityScheme: new OpenApiSecurityScheme
                                              {
                                                  In = ParameterLocation.Query,
                                                  Type = SecuritySchemeType.ApiKey,
                                                  Name = "api-key",
                                                  Description = "Utilização: Cole sua '{apiKey}'"
                                              });

                options.IncludeXmlComments(Path.Combine(path1: "wwwroot",
                                                        path2: "api-docs.xml"));

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "ApiKey",
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
            });
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            return app.UseSwagger()
                      .UseSwaggerUI(options =>
                      {
                          options.RoutePrefix = "api/v1/documentation";

                          options.SwaggerEndpoint(url: "../../../swagger/v1/swagger.json",
                                                  name: "Documentação API v1");

                          options.DocumentTitle = "Auxiliar API - Swagger UI";

                          options.DisplayRequestDuration();

                          options.DocExpansion(DocExpansion.None);
                      });
        }

        #endregion Metodos Publicos
    }
}