using MediatR;

namespace Auxiliar.Domain.Core.Notifications
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        #region Campos Privados

        private List<DomainNotification> _notifications;

        #endregion Campos Privados

        #region Construtores Publicos

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        #endregion Construtores Publicos

        #region Metodos Publicos

        public Task Handle(DomainNotification message, CancellationToken cancellationToken)
        {
            _notifications.Add(message);

            return Task.CompletedTask;
        }

        public virtual IEnumerable<DomainNotification> GetNotifications() => _notifications;

        public virtual bool HasNotifications() => GetNotifications().Any();

        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }

        #endregion Metodos Publicos
    }
}