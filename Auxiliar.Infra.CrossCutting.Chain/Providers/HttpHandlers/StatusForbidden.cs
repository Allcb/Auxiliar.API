﻿using Auxiliar.Domain.Core.Enum;
using Auxiliar.Infra.CrossCutting.ExceptionHandler.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auxiliar.Infra.CrossCutting.Chain.Providers.HttpHandlers
{
    public class StatusForbidden : HttpResponseHandle
    {
        public override IActionResult Handle(object result, ApiErrorCodes apiErrorCode, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Forbidden)
                throw new ApiException(requestData: result, apiErrorCode: apiErrorCode, httpStatusCode: statusCode);

            return ((HttpResponseHandle)Next).Handle(result, apiErrorCode, statusCode);
        }
    }
}