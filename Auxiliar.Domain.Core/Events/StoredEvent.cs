namespace Auxiliar.Domain.Core.Events
{
    public class StoredEvent : Event
    {
        #region Construtores Protegidos

        protected StoredEvent()
        {
        }

        #endregion Construtores Protegidos

        #region Construtores Publicos

        public StoredEvent(Message theEvent, string data, Guid usuarioId)
        {
            Id = Guid.NewGuid();
            AggregateId = theEvent.AggregateId;
            MessageType = theEvent.MessageType;
            Data = data;
            UsuarioId = usuarioId;
        }

        #endregion Construtores Publicos

        #region Propriedades Publicas

        public Guid Id { get; private set; }
        public string Data { get; private set; }
        public Guid UsuarioId { get; private set; }

        #endregion Propriedades Publicas
    }
}