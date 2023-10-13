using MediatR;

namespace Auxiliar.Domain.Core.Events
{
    public abstract class Event : Message, INotification
    {
        #region Construtores Protegidos

        protected Event()
        {
            CreatedDate = DateTime.Now;
        }

        #endregion Construtores Protegidos

        #region Propriedades Publicas

        public DateTime CreatedDate { get; private set; }

        #endregion Propriedades Publicas
    }
}