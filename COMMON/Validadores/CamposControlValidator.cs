using COMMON.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Validadores
{
    public abstract class CamposControlValidator<T> : AbstractValidator<T> where T : CamposControl
    {
        public CamposControlValidator() 
        {
            RuleFor(c => c.FechaAlta).NotEmpty();
            RuleFor(c => c.UsuarioAlta).MaximumLength(100);
            RuleFor(c => c.UsuarioMod).MaximumLength(100).When(c => !string.IsNullOrEmpty(c.UsuarioMod));
        }
    }
}
