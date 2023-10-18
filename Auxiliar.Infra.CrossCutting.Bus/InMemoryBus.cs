using Auxiliar.Domain.Core.Bus;
using Auxiliar.Domain.Core.Events;
using Auxiliar.Domain.Core.Notifications;
using MediatR;

namespace Auxiliar.Infra.CrossCutting.Bus
{
    public sealed class InMemoryBus : IMediatorHandler
    {
        #region Campos Privados

        private readonly IEventStore _eventStore;
        private readonly IMediator _mediator;

        #endregion Campos Privados

        #region Construtores Publicos

        public InMemoryBus(IEventStore eventStore,
                           IMediator mediator)
        {
            _eventStore = eventStore;
            _mediator = mediator;
        }

        #endregion Construtores Publicos

        #region Metodos Publicos

        public Task SendCommand<T>(T command)
        {
            return _mediator.Send(command);
        }

        public Task<TReturn> SendDynamicCommand<TCommand, TReturn>(TCommand command) where TCommand : IRequest<TReturn>
        {
            return _mediator.Send(command);
        }

        public Task RaiseEvent<T>(T @event, Guid? usuarioId = null) where T : Event
        {
            if (!@event.MessageType.Equals(nameof(DomainNotification)))
                _eventStore?.Save(@event, usuarioId);

            return _mediator.Publish(@event);
        }

        #endregion Metodos Publicos
    }
}