namespace Auxiliar.Services.API
{
    public class Startup
    {
        #region Constantes Publicas

        public const string ELMAH_BASE_PATH = "/log-errors";

        #endregion Constantes Publicas

        #region Propriedades Publicas

        public IConfiguration Configuration { get; }

        #endregion Propriedades Publicas

        #region Construtores Publicos

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion Construtores Publicos

        #region Metodos Publicos

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }

        #endregion Metodos Publicos
    }
}