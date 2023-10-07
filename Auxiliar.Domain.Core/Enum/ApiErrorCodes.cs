using Auxiliar.Domain.Core.Attributes;
using Microsoft.AspNetCore.Http;

namespace Auxiliar.Domain.Core.Enum
{
    public enum ApiErrorCodes
    {
        #region 500 Status (Internal Server Error)

        /// <summary>
        /// Erro inesperado.
        /// </summary>
        [HttpStatusCode(StatusCodes.Status500InternalServerError)]
        [Description("Erro inesperado.", "Contate o administrador do sistema.")]
        UNEXPC,

        #endregion 500 Status (Internal Server Error)
    }
}