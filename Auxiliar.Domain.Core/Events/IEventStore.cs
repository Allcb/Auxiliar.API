namespace Auxiliar.Domain.Core.Events
{
    public interface IEventStore
    {
        #region Metodos Publicos

        void Save<T>(T @event, Guid? usuarioId = null) where T : Event;

        #endregion Metodos Publicos
    }
}