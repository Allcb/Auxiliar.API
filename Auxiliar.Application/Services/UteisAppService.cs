using Auxiliar.Application.Interfaces;
using Auxiliar.Domain.Core.Types;
using System.Security.Cryptography;
using System.Text;

namespace Auxiliar.Application.Services
{
    public class UteisAppService : IUteisAppService
    {
        #region Metodos Publicos

        public string GerarCpf()
        {
            string _novoCpf = new Random().Next(100000000, 999999999).ToString();

            return _novoCpf += GerarDigitos(novoDocumento: _novoCpf,
                                            fatorMultiplicacao: new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 });
        }

        public bool ValidarCpf(string cpf)
        {
            cpf = cpf.Trim()
                     .Replace(".", string.Empty)
                     .Replace("-", string.Empty);

            if (cpf.Length != 11 || cpf.Equals("12345678909"))
                return false;

            return ValidarDocumento(documento: cpf,
                                    fatorMultiplicacao: new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 });
        }

        public bool ValidarCnpj(string cnpj)
        {
            cnpj = cnpj.Trim()
                       .Replace(".", string.Empty)
                       .Replace("/", string.Empty)
                       .Replace("-", string.Empty);

            if (cnpj.Length != 14)
                return false;

            int[] _fatorMultiplicacao = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            return ValidarDocumento(cnpj, _fatorMultiplicacao);
        }

        public string GerarSenhaForte(int quantidadeCaracteres = 12)
        {
            StringBuilder _senha = new StringBuilder(quantidadeCaracteres);

            using (RandomNumberGenerator valorAleatorio = RandomNumberGenerator.Create())
            {
                byte[] _arrayByte = new byte[4];

                for (int indice = 0; indice < quantidadeCaracteres; indice++)
                {
                    valorAleatorio.GetBytes(_arrayByte);

                    _senha.Append(SenhaTypes.TODOS_CARACTERES[Convert.ToInt32(BitConverter.ToUInt32(_arrayByte, 0) % SenhaTypes.TODOS_CARACTERES.Length)]);
                }
            }

            return _senha.ToString();
        }

        #endregion Metodos Publicos

        #region Metodos Privados

        private string GerarDigitos(string novoDocumento, int[] fatorMultiplicacao)
        {
            int[] _soma = new int[2] { 0, 0 },
                  _resultado = new int[2] { 0, 0 },
                  _digitos = new int[novoDocumento.Length + 1];

            string _digitoVerificador = string.Empty;

            try
            {
                for (int _digito = 0; _digito < novoDocumento.Length; _digito++)
                {
                    _digitos[_digito] = int.Parse(novoDocumento.Substring(_digito, 1));

                    if (_digito <= novoDocumento.Length - 1)
                        _soma[0] += _digitos[_digito] * fatorMultiplicacao[_digito + 1];

                    if (_digito <= novoDocumento.Length)
                        _soma[1] += _digitos[_digito] * fatorMultiplicacao[_digito];
                }

                for (int _digito = 0; _digito < 2; _digito++)
                {
                    _resultado[_digito] = (_soma[_digito] % 11);

                    if ((_resultado[_digito] == 0) || (_resultado[_digito] == 1))
                        _digitoVerificador += "0";
                    else
                        _digitoVerificador += (11 - _resultado[_digito]).ToString();

                    if (_digito == 0)
                    {
                        _digitos[novoDocumento.Length] = int.Parse(_digitoVerificador);
                        _soma[1] += _digitos[novoDocumento.Length] * fatorMultiplicacao[novoDocumento.Length];
                    }
                }

                return _digitoVerificador;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static bool ValidarDocumento(string documento, int[] fatorMultiplicacao)
        {
            int[] _soma = new int[2] { 0, 0 },
                  _resultado = new int[2] { 0, 0 },
                  _digitos = new int[documento.Length];

            bool _digitoIgual = true;

            string _digitoVerificador = string.Empty;

            try
            {
                for (int _digito = 0; _digito < documento.Length; _digito++)
                {
                    if (documento[_digito] != documento[0])
                        _digitoIgual = false;

                    _digitos[_digito] = int.Parse(documento.Substring(_digito, 1));

                    if (_digito <= documento.Length - 3)
                        _soma[0] += _digitos[_digito] * fatorMultiplicacao[_digito + 1];

                    if (_digito <= documento.Length - 2)
                        _soma[1] += _digitos[_digito] * fatorMultiplicacao[_digito];
                }

                if (_digitoIgual)
                    return false;

                for (int _digito = 0; _digito < 2; _digito++)
                {
                    _resultado[_digito] = (_soma[_digito] % 11);

                    if ((_resultado[_digito] == 0) || (_resultado[_digito] == 1))
                        _digitoVerificador += (0).ToString();
                    else
                        _digitoVerificador += (11 - _resultado[_digito]).ToString();
                }

                return documento.EndsWith(_digitoVerificador);
            }
            catch
            {
                return false;
            }
        }

        #endregion Metodos Privados
    }
}