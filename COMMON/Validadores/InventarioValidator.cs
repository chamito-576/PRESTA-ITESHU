using COMMON.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Validadores
{
    public class InventarioValidator : CamposControlValidator<Inventario>
    {
        public InventarioValidator()
        {
            RuleFor(i => i.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(i => i.Descripcion).NotEmpty().MaximumLength(100);
            RuleFor(i => i.Cantidad).GreaterThanOrEqualTo(0);
            RuleFor(i => i.IdLaboratorio).NotEmpty();
        }
    }
}
