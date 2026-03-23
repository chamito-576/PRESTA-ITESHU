using COMMON.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class LaboratoriosManager:GenericManager<Laboratorios>
    {
        public LaboratoriosManager(AbstractValidator<Laboratorios> validador) : base(validador)
        {
        }
    }
}
