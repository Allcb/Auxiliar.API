namespace Auxiliar.Domain.Core.Types
{
    public class SenhaTypes
    {
        #region Constantes Publicas

        public const string CARACTERES_MINUSCULOS = "abcdefghijklmnopqrstuvwxyz";
        public const string CARACTERES_MAIUSCULOS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string NUMEROS = "0123456789";
        public const string CARACTERES_ESPECIAIS = "!@#$%^&*()_+-=[]{}|;:,.<>?";
        public const string TODOS_CARACTERES = CARACTERES_MINUSCULOS + CARACTERES_MAIUSCULOS + NUMEROS + CARACTERES_ESPECIAIS;

        #endregion Constantes Publicas
    }
}