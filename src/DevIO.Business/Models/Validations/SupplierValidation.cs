using FluentValidation;

namespace DevIO.Business.Models.Validations
{
    public class SupplierValidation : AbstractValidator<Supplier>
    {
        public SupplierValidation()
        {
            RuleFor(supplier => supplier.Name)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(2, 100)
                    .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            When(supplier => supplier.SupplierType == SupplierType.PhysicalPerson, () =>
                {
                    RuleFor(sup => sup.IdentificationNumber.Length)
                        .Equal(14)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

                    //RuleFor(sup => sup.IdentificationNumber.Length)
                    //    .Equal(CpfValidate.CpfSize)
                    //.WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

                    //RuleFor(sup => CpfValidation.Validate(sup.IdentificationNumber))
                    //    .Equal(true)
                    //.WithMessage("O documento fornecido é inválido.");
                }
            );

            When(supplier => supplier.SupplierType == SupplierType.LegalPerson, () =>
                {
                    RuleFor(sup => sup.IdentificationNumber.Length)
                        .Equal(11)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

                    //RuleFor(sup => sup.IdentificationNumber.Length)
                    //    .Equal(CnpjValidate.CnpjSize)
                    //.WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

                    //RuleFor(sup => CnpjValidation.Validate(sup.IdentificationNumber))
                    //    .Equal(true)
                    //.WithMessage("O documento fornecido é inválido.");
                }
            );
        }
    }
}
