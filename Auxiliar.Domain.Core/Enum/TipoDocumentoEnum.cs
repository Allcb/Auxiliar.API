using Auxiliar.Domain.Core.Attributes;

namespace Auxiliar.Domain.Core.Enum
{
    public enum TipoDocumentoEnum
    {
        [Description("Pessoa física Cpf")]
        CPF = 0,

        [Description("Pessoa jurídica Cnpj")]
        CNPJ = 1,
    }
}