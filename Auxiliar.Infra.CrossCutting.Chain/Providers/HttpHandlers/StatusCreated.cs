using Auxiliar.Domain.Core.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auxiliar.Infra.CrossCutting.Chain.Providers.HttpHandlers
{
    public class StatusCreated : HttpResponseHandle
    {
        public override IActionResult Handle(object result, ApiErrorCodes apiErrorCode, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Created)
                return new CreatedResult(string.Empty, result);

            return ((HttpResponseHandle)Next).Handle(result, apiErrorCode, statusCode);
        }
    }
}