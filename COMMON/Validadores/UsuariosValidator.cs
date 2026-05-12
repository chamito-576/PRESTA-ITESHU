using COMMON.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Validadores
{
    public class UsuariosValidator:CamposControlValidator<Usuarios>
    {
        public UsuariosValidator()
        {
            RuleFor(u => u.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(u => u.ApellidoPaterno).NotEmpty().MaximumLength(100);
            RuleFor(u => u.ApellidoMaterno).MaximumLength(100).When(u => !string.IsNullOrEmpty(u.ApellidoMaterno));
            RuleFor(u => u.Correo).NotEmpty().EmailAddress();
            RuleFor(u => u.Contrasena).NotEmpty().MinimumLength(6);
            RuleFor(u => u.Rol).NotEmpty().MaximumLength(100);
            RuleFor(u => u.Activo).NotNull();
            
        }
    }
}
