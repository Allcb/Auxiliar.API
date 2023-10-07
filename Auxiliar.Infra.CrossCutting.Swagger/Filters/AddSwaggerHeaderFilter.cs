using Auxiliar.Domain.Core.Types;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Auxiliar.Infra.CrossCutting.Swagger.Filters
{
    public class AddSwaggerHeaderFilter : IOperationFilter
    {
        #region Constantes Publicas

        public const string VALOR_HEADER_SWAGGER = "true";

        #endregion Constantes Publicas

        #region Metodos Publicos

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = HeadersTypes.SWAGGER,
                In = ParameterLocation.Header,
                Description = "Header personalizado para requisições feitas via Swagger.",
                Required = false,
                Schema = new OpenApiSchema { Type = "boolean" },
                Example = new OpenApiString(VALOR_HEADER_SWAGGER)
            });
        }

        #endregion Metodos Publicos
    }
}