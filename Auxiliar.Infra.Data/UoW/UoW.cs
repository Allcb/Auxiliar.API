using Auxiliar.Domain.Interfaces.UoW;
using Auxiliar.Infra.Data.Context;

namespace Auxiliar.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Campos Privados

        private readonly AuxiliarContext _auxiliarContext;
        private Exception _exception;

        #endregion Campos Privados

        #region Construtores Publicos

        public UnitOfWork(AuxiliarContext auxiliarContext)
        {
            _auxiliarContext = auxiliarContext;
        }

        #endregion Construtores Publicos

        #region Metodos Publicos

        public bool Commit()
        {
            try
            {
                int _commited = _auxiliarContext.SaveChanges();

                return _commited > 0;
            }
            catch (Exception ex)
            {
                SetCommitException(ex.InnerException ?? ex);

                return false;
            }
        }

        public string GetCommitExceptionMessage()
        {
            return _exception?.Message;
        }

        public void Dispose()
        {
            _auxiliarContext.Dispose();
        }

        #endregion Metodos Publicos

        #region Metodos Privados

        private void SetCommitException(Exception exception)
        {
            _exception = exception;
        }

        #endregion Metodos Privados
    }
}