using Auxiliar.Domain.Core.Events;

namespace Auxiliar.Domain.Core.Notifications
{
    public class DomainNotification : Event
    {
        #region Construtores Publicos

        public DomainNotification(string key,
                                  string value)
        {
            DomainNotificationId = Guid.NewGuid();
            Version = 1;
            Key = key;
            Value = value;
        }

        #endregion Construtores Publicos

        #region Propriedades Publicas

        public Guid DomainNotificationId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public int Version { get; private set; }

        #endregion Propriedades Publicas
    }
}