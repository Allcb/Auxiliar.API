namespace Auxiliar.Domain.Core.Models
{
    public abstract class Entity
    {
        #region Construtores Protegidos

        protected Entity()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.Now;
            Excluido = false;
        }

        #endregion Construtores Protegidos

        #region Propriedades Publicas

        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public bool Excluido { get; set; }

        #endregion Propriedades Publicas
    }
}