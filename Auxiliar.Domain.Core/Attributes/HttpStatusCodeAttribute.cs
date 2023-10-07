using System.Net;

namespace Auxiliar.Domain.Core.Attributes
{
    public sealed class HttpStatusCodeAttribute : Attribute
    {
        #region Construtores Publicos

        public HttpStatusCodeAttribute(int statusCode)
        {
            HttpStatusCode = (HttpStatusCode)statusCode;
        }

        #endregion Construtores Publicos

        #region Propriedades Publicas

        public HttpStatusCode HttpStatusCode { get; }

        #endregion Propriedades Publicas
    }
}