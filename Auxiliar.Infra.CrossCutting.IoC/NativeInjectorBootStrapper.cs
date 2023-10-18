using Auxiliar.Application.Interfaces;
using Auxiliar.Application.Services;
using Auxiliar.Domain.Core.Bus;
using Auxiliar.Domain.Core.Events;
using Auxiliar.Domain.Core.Notifications;
using Auxiliar.Infra.CrossCutting.Bus;
using Auxiliar.Infra.Data.EventSourcing;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Auxiliar.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        #region Metodos Publicos

        public static void RegisterServices(IServiceCollection services)
        {
            #region Singleton

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #endregion Singleton

            #region Scoped

            #region Domain - Bus (Mediator)

            services.AddScoped<IMediatorHandler, InMemoryBus>();

            #endregion Domain - Bus (Mediator)

            #region Domain - Notifications

            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            #endregion Domain - Notifications

            #region Infra - Data EventSourcing

            services.AddScoped<IEventStore, SqlEventStore>();
            // services.AddScoped<IEventStoreRepository, EventStoreSQLRepository>();

            #endregion Infra - Data EventSourcing

            #endregion Scoped
        }

        #endregion Metodos Publicos
    }
}