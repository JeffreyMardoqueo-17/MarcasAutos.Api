using FluentValidation;
using MarcasAutos.Api.Models.Requests;

namespace MarcasAutos.Api.Validators;

public class CreateMarcaAutoRequestValidator : AbstractValidator<CreateMarcaAutoRequest>
{
    public CreateMarcaAutoRequestValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la marca es obligatorio.")
            .MinimumLength(2).WithMessage("El nombre de la marca debe tener al menos 2 caracteres.")
            .MaximumLength(100).WithMessage("El nombre de la marca no puede exceder 100 caracteres.");

        RuleFor(x => x.PaisOrigen)
            .MaximumLength(100).WithMessage("El pais de origen no puede exceder 100 caracteres.")
            .Must(pais => pais == null || pais.Trim().Length >= 2)
            .WithMessage("El pais de origen debe tener al menos 2 caracteres validos cuando se envia.");
    }
}
