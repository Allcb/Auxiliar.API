using Auxiliar.Domain.Core.Bus;
using Auxiliar.Domain.Core.Notifications;
using Auxiliar.Infra.CrossCutting.ExceptionHandler.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Auxiliar.Services.Api.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        #region Campos Privados

        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;

        #endregion Campos Privados

        #region Construtores Protegidos

        protected ApiController(IMediatorHandler mediator,
                                INotificationHandler<DomainNotification> notifications)
        {
            _mediator = mediator;
            _notifications = (DomainNotificationHandler)notifications;
        }

        #endregion Construtores Protegidos

        #region Propriedades Protegidas

        protected IEnumerable<DomainNotification> Notifications => _notifications.GetNotifications();

        #endregion Propriedades Protegidas

        #region Metodos Protegidos

        protected new IActionResult Response(object result = null)
        {
            if (IsValidOperation())
                return Ok(result);

            throw new ApiException(httpStatusCode: HttpStatusCode.BadRequest,
                                   messages: Notifications.Select(domainNotification => domainNotification.Value)
                                                          .ToArray());
        }

        protected void NotifyModelStateErrors()
        {
            IEnumerable<ModelError> _errors = ModelState.Values.SelectMany(modelStateEntry => modelStateEntry.Errors);

            _errors.ToList()
                   .ForEach(error =>
                   {
                       string _message = error.Exception == null ? error.ErrorMessage : error.Exception.Message;

                       NotifyError(code: string.Empty,
                                   message: _message);
                   });
        }

        protected void NotifyError(string code, string message)
        {
            _mediator.RaiseEvent(new DomainNotification(key: code,
                                                        value: message));
        }

        protected bool IsValidOperation() => !_notifications.HasNotifications();

        #endregion Metodos Protegidos
    }
}