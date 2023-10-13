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

        /// <summary>
        /// Erro ao executar operação no banco de dados.
        /// </summary>
        [HttpStatusCode(StatusCodes.Status500InternalServerError)]
        [Description("Erro ao executar operação no banco de dados.")]
        ERROPBD,

        #endregion 500 Status (Internal Server Error)
    }
}