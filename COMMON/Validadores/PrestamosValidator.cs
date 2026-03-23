using COMMON.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Validadores
{
    public class PrestamosValidator : CamposControlValidator<Prestamos>
    {
        public PrestamosValidator()
        {
            RuleFor(p => p.IdSolicitud).NotEmpty();
            RuleFor(p => p.Estado).NotEmpty().Must(e => e == "Aprobado" || e == "Entregado" || e == "Devuelto");
            RuleFor(p => p.CodigoQR).NotEmpty();
            RuleFor(p => p.Observaciones).NotEmpty().MaximumLength(500);
            RuleFor(p => p.FechaDevolucion).GreaterThanOrEqualTo(p => p.FechaEntrega.Value).When(p => p.FechaEntrega.HasValue && p.FechaDevolucion.HasValue);
        }
    }
}
