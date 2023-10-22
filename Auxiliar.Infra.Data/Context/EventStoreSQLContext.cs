using Auxiliar.Domain.Core.Events;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections;

namespace Auxiliar.Infra.Data.Context
{
    public class EventStoreSQLContext : DbContext
    {
        #region Propriedades Publicas

        public DbSet<StoredEvent> StoredEvent { get; set; }

        #endregion Propriedades Publicas

        #region Metodos Protegidos

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                          .AddJsonFile(path: "appsettings.json",
                                                                                       optional: true)
                                                                          .AddJsonFile(path: $"appsettings.{_environment}.json",
                                                                                       optional: true,
                                                                                       reloadOnChange: true)
                                                                          .AddEnvironmentVariables()
                                                                          .Build();

            if (_environment.Equals("Test"))
                optionsBuilder.UseInMemoryDatabase(databaseName: "EventStore");
            else
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString(nameof(EventStoreSQLContext)));
        }

        #endregion Metodos Protegidos

        #region Metodos Privados

        private string GetConnectionStringFromEnvironment()
        {
            IDictionary _envVars = Environment.GetEnvironmentVariables();

            string _dataSource = _envVars["DB_DATA_SOURCE"].ToString();
            string _dataBase = _envVars["DB_CATALOG"].ToString();
            string _user = _envVars["DB_DATABASE_USER"].ToString();
            string _password = _envVars["DB_DATABASE_USER_PASSWORD"].ToString();

            SqlConnectionStringBuilder _connectionStringBuilder = new SqlConnectionStringBuilder();

            _connectionStringBuilder.DataSource = _dataSource;
            _connectionStringBuilder.InitialCatalog = _dataBase;
            _connectionStringBuilder.IntegratedSecurity = true;
            _connectionStringBuilder.PersistSecurityInfo = false;
            _connectionStringBuilder.UserID = _user;
            _connectionStringBuilder.Password = _password;
            _connectionStringBuilder.MultipleActiveResultSets = false;
            _connectionStringBuilder.Encrypt = false;
            _connectionStringBuilder.TrustServerCertificate = false;
            _connectionStringBuilder.Add(keyword: "Trusted_Connection", value: false);
            _connectionStringBuilder.Pooling = true;
            _connectionStringBuilder.MaxPoolSize = 5000;

            return _connectionStringBuilder.ConnectionString;
        }

        #endregion Metodos Privados
    }
}