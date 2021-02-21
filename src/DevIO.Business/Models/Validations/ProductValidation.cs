using FluentValidation;

namespace DevIO.Business.Models.Validations
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(product => product.Name)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(2, 200)
                    .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(product => product.Description)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(2, 1000)
                    .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(product => product.Value)
                .GreaterThan(0)
                    .WithMessage("O campo {PropertyName} precisa ser maior que {ComparisonValue}.");
        }
    }
}
