using Auxiliar.Infra.CrossCutting.ExceptionHandler.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Net;

namespace Auxiliar.Infra.CrossCutting.ExceptionHandler.Providers
{
    public class ApiVersionExceptionHandler : DefaultErrorResponseProvider
    {
        #region Constantes Privadas

        private const string UnsupportedApiVersionError = "UnsupportedApiVersion";

        #endregion Constantes Privadas

        #region Metodos Publicos

        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            switch (context.ErrorCode)
            {
                case UnsupportedApiVersionError:
                    throw new ApiException(httpStatusCode: HttpStatusCode.BadRequest,
                                           messages: "Versão da Api não suportada");

                default:
                    throw new ApiException(httpStatusCode: HttpStatusCode.BadRequest,
                                           messages: "Erro no versionamento da Api");
            }
        }

        #endregion Metodos Publicos
    }
}