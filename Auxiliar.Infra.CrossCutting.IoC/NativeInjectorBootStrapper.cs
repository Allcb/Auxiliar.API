using Auxiliar.Application.Interfaces;
using Auxiliar.Application.Services;
using Auxiliar.Domain.Core.Bus;
using Auxiliar.Domain.Core.Events;
using Auxiliar.Domain.Core.Notifications;
using Auxiliar.Domain.Interfaces.UoW;
using Auxiliar.Infra.CrossCutting.Bus;
using Auxiliar.Infra.CrossCutting.Chain.Extensions;
using Auxiliar.Infra.CrossCutting.Chain.Providers.HttpHandlers;
using Auxiliar.Infra.Data.Context;
using Auxiliar.Infra.Data.EventSourcing;
using Auxiliar.Infra.Data.UoW;
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
            #region Contexts

            services.AddDbContext<AuxiliarContext>();
            services.AddDbContext<EventStoreSQLContext>();

            #endregion Contexts

            #region Chain

            services.ConfigureChain<HttpResponseHandle>(new StatusOk())
                    .Next(new StatusAccepted())
                    .Next(new StatusCreated())
                    .Next(new StatusNoContent())
                    .Next(new StatusNotFound())
                    .Next(new StatusForbidden())
                    .Next(new StatusBadRequest())
                    .Next(new StatusUnauthorized())
                    .Next(new StatusInternalServerError())
                    .Next(new StatusConflict())
                    .Next(new DefaultStatus());

            #endregion Chain

            #region Singleton

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #endregion Singleton

            #region Scoped

            #region AppServices

            services.AddScoped<IUteisAppService, UteisAppService>();

            #endregion AppServices

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

            #region Infra - Data uow

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion Infra - Data uow

            #endregion Scoped
        }

        #endregion Metodos Publicos
    }
}