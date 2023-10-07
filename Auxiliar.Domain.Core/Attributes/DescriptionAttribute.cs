namespace Auxiliar.Domain.Core.Attributes
{
    public sealed class DescriptionAttribute : Attribute
    {
        #region Construtores Publicos

        public DescriptionAttribute(string description,
                                    string title = null)
        {
            Description = description;
            Title = title;
        }

        #endregion Construtores Publicos

        #region Propriedades Publicas

        public string Title { get; set; }
        public string Description { get; set; }

        #endregion Propriedades Publicas
    }
}