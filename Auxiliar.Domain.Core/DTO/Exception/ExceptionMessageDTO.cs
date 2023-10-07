namespace Auxiliar.Domain.Core.DTO
{
    public class ExceptionMessageDTO
    {
        #region Propriedades Publicas

        public string Code { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string Field { get; set; }

        #endregion Propriedades Publicas
    }
}