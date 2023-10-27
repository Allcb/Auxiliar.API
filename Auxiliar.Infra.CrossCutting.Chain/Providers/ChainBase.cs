namespace Auxiliar.Infra.CrossCutting.Chain.Providers
{
    public abstract class ChainBase
    {
        #region Propriedades Publicas

        public ChainBase Next { get; set; }

        #endregion Propriedades Publicas
    }
}