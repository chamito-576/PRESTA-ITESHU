using COMMON.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Validadores
{
    public class ReportesValidator : CamposControlValidator<Reportes>
    {
        public ReportesValidator()
        {
            RuleFor(r => r.IdPrestamo).NotEmpty();
            RuleFor(r => r.FechaGeneracion).LessThanOrEqualTo(DateTime.Now);
            RuleFor(r => r.RutaPDF).NotEmpty().MaximumLength(200);
        }
    }
}
