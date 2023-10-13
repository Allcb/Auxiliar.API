namespace Auxiliar.Domain.Interfaces.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        #region Metodos Publicos

        bool Commit();

        string GetCommitExceptionMessage();

        #endregion Metodos Publicos
    }
}