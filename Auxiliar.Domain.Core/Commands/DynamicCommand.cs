using Auxiliar.Domain.Core.Events;
using FluentValidation.Results;

namespace Auxiliar.Domain.Core.Commands
{
    public abstract class DynamicCommand<T> : DynamicMessage<T>
    {
        #region Construtores Protegidos

        protected DynamicCommand()
        {
            Timestamp = DateTime.Now;
        }

        #endregion Construtores Protegidos

        #region Propriedades Publicas

        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        #endregion Propriedades Publicas

        #region Metodos Publicos

        public abstract bool EValido();

        public void AddValidationErrors(params ValidationFailure[] errors)
        {
            AddValidationErrors(errors.AsEnumerable());
        }

        public void AddValidationErrors(IEnumerable<ValidationFailure> errors)
        {
            if (ValidationResult.Errors is not List<ValidationFailure> _validationErros)
                return;

            _validationErros.AddRange(errors);

            ValidationResult = new ValidationResult(_validationErros);
        }

        #endregion Metodos Publicos
    }
}