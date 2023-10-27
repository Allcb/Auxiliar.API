using Auxiliar.Domain.Core.Models;
using Auxiliar.Infra.Data.Extensions;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Diagnostics;

namespace Auxiliar.Infra.Data.Context
{
    public class AuxiliarContext : DbContext, IDataProtectionKeyContext
    {
        #region Campos Privados

        private ILoggerFactory _loggerFactory;

        #endregion Campos Privados

        #region Construtores Publicos

        public AuxiliarContext()
        {
        }

        public AuxiliarContext(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        #endregion Construtores Publicos

        #region Propriedades Publicas

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        #endregion Propriedades Publicas

        #region Metodos Protegidos

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuxiliarContext).Assembly);
            modelBuilder.ApplyGlobalStandards();
            modelBuilder.SeedData();

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

            if (Debugger.IsAttached)
                optionsBuilder.UseLoggerFactory(_loggerFactory);

            if (_environment.Equals("Test"))
                optionsBuilder.UseInMemoryDatabase(databaseName: "EventStore");
            else
                optionsBuilder.UseSqlServer(GetConnectionStringFromEnvironment())
                              .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            optionsBuilder.EnableSensitiveDataLogging();
        }

        #endregion Metodos Protegidos

        #region Metodos Publicos

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess: acceptAllChangesOnSuccess,
                                         cancellationToken: cancellationToken);
        }

        #endregion Metodos Publicos

        #region Metodos Privados

        private void OnBeforeSaving()
        {
            ChangeTracker.Entries()
                         .ToList()
                         .ForEach(entry =>
                         {
                             if (entry.Entity is Entity trackableEntity)
                             {
                                 if (entry.State == EntityState.Added)
                                 {
                                     trackableEntity.DataCriacao = DateTime.Now;
                                     trackableEntity.Excluido = false;
                                 }
                                 else if (entry.State == EntityState.Modified)
                                     trackableEntity.DataModificacao = DateTime.Now;
                             }
                         });
        }

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