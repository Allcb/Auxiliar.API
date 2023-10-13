using MediatR;

namespace Auxiliar.Domain.Core.Events
{
    public abstract class DynamicMessage<T> : IRequest<T>
    {
        #region Construtores Protegidos

        protected DynamicMessage()
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