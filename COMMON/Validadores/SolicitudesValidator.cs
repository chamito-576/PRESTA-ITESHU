using COMMON.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Validadores
{
    public class SolicitudesValidator : CamposControlValidator<Solicitudes>
    {
        public SolicitudesValidator()
        {
            RuleFor(s => s.IdUsuario).NotEmpty();
            RuleFor(s => s.IdMaterial).NotEmpty();
            RuleFor(s => s.Estado).NotEmpty().Must(e => e == "Pendiente" || e == "Aprobado" || e == "Rechazado");
            RuleFor(x => x.FechaSolicitud).NotEmpty();
        }
    }
}
