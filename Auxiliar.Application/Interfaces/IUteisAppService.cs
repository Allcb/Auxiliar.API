namespace Auxiliar.Application.Interfaces
{
    public interface IUteisAppService
    {
        #region Metodos Publicos

        string GerarCpf();

        bool ValidarCpf(string cpf);

        bool ValidarCnpj(string cnpj);

        string GerarSenhaForte(int quantidadeCaracteres);

        #endregion Metodos Publicos
    }
}