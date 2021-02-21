using FluentValidation;

namespace DevIO.Business.Models.Validations
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
            RuleFor(address => address.StreetAddress)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(2, 200)
                    .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(address => address.Neighborhood)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(2, 100)
                    .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(address => address.ZipCode)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(8)
                    .WithMessage("O campo {PropertyName} precisa ter {MaxLength} caracteres.");

            RuleFor(address => address.City)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(2, 100)
                    .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(address => address.State)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(2, 50)
                    .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(address => address.Number)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(1, 50)
                    .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");
        }
    }
}
