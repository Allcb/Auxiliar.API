using Auxiliar.Domain.Core.Events;
using MediatR;

namespace Auxiliar.Domain.Core.Bus
{
    public interface IMediatorHandler
    {
        #region Metodos Publicos

        Task SendCommand<T>(T command);

        Task<TReturn> SendDynamicCommand<TCommand, TReturn>(TCommand command) where TCommand : IRequest<TReturn>;

        Task RaiseEvent<T>(T @event, Guid? usuarioId = null) where T : Event;

        #endregion Metodos Publicos
    }
}