using BIZ;
using COMMON;

namespace PrestaITESHU.Pages;

public partial class LoginAdmin : ContentPage
{
    private UsuariosManager usuariosManager;
    public bool OcultarContrasena { get; set; } = true;

    public string Correo { get; set; }

    public string Contrasena { get; set; }
    public LoginAdmin()
	{
		InitializeComponent();
        BindingContext = this;

        usuariosManager = FabricManager.UsuariosManager;
    }

    private void cerrarsesionbutton_Clicked(object sender, EventArgs e)
    {
        Microsoft.Maui.Controls.Application.Current?.CloseWindow(this.Window);
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            var usuario = await usuariosManager.LoginAdmin(Correo,Contrasena);

            if (usuario != null)
            {
                // GUARDAR USUARIO ACTUAL
                Params.UsuarioConectado =
                    usuario.Nombre;
                Params.IdUsuarioConectado = usuario.IdUsuario;
                Params.RolUsuarioConectado = usuario.Rol;
                Params.IdLaboratorioConectado = usuario.IdLaboratorio;

                await DisplayAlert(
                    "Correcto",
                    $"Bienvenido {usuario.Nombre}",
                    "OK");

                // ABRIR MENU ADMIN
                Application.Current.MainPage =
                    new NavigationPage(
                        new MenuAdmin());
            }


            else
            {

                await DisplayAlert(
                    "Error",
                    "Correo o contraseña incorrectos",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    private void VerContrasena_Clicked(object sender, EventArgs e)
    {
        OcultarContrasena = !OcultarContrasena;

        OnPropertyChanged(nameof(OcultarContrasena));
    }
}