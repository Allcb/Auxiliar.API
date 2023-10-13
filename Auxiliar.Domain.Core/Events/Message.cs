using MediatR;

namespace Auxiliar.Domain.Core.Events
{
    public abstract class Message : IRequest<bool>
    {
        #region Construtores Protegidos

        protected Message()
        {
            MessageType = GetType().Name;
        }

        #endregion Construtores Protegidos

        #region Propriedades Publicas

        public string MessageType { get; protected set; }
        public Guid AggregateId { get; protected set; }

        #endregion Propriedades Publicas
    }
}