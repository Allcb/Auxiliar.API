namespace Auxiliar.Domain.Core.DTO
{
    public class ExceptionDetailsDTO
    {
        #region Propriedades Publicas

        public int HttpStatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<ExceptionMessageDTO> Errors { get; set; }

        #endregion Propriedades Publicas
    }
}