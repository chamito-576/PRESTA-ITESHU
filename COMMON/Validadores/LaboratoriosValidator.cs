using COMMON.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Validadores
{
    public class LaboratoriosValidator : CamposControlValidator<Laboratorios>
    {
        public LaboratoriosValidator()
        {
            RuleFor(l => l.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(l => l.Edificio).NotEmpty().MaximumLength(100);
            RuleFor(l => l.Descripcion).MaximumLength(200).When(l => !string.IsNullOrEmpty(l.Descripcion));
            RuleFor(l => l.IdUsuario).NotEmpty();
        }
    }
}
