using Auxiliar.Domain.Core.Bus;
using Auxiliar.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auxiliar.Services.Api.Controllers
{
    [Route("[controller]")]
    public class AutenticacaoController : ApiController
    {
        #region Construtores Publicos

        public AutenticacaoController(INotificationHandler<DomainNotification> notifications,
                                      IMediatorHandler mediator)
            : base(mediator, notifications)
        {
        }

        #endregion Construtores Publicos
    }
}