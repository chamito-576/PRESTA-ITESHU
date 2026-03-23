using COMMON.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class ReportesManager:GenericManager<Reportes>
    {
        public ReportesManager(AbstractValidator<Reportes> validador) : base(validador)
        {
        }
    }
}
