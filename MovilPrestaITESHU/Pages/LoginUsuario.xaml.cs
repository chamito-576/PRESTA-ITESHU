using BIZ;
using COMMON;

namespace MovilPrestaITESHU.Pages;

public partial class LoginUsuario : ContentPage
{
    private UsuariosManager usuariosManager;
    public bool OcultarContrasena { get; set; } = true;

    public string Correo { get; set; }

    public string Contrasena { get; set; }
    public LoginUsuario()
	{
		InitializeComponent();
        BindingContext = this;

        usuariosManager = FabricManager.UsuariosManager;
    }

    private void VerContrasena_Clicked(object sender, EventArgs e)
    {
        OcultarContrasena = !OcultarContrasena;

        OnPropertyChanged(nameof(OcultarContrasena));
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            var usuario =
                await usuariosManager.LoginUsuario(
                    Correo,
                    Contrasena);

            if (usuario != null)
            {
                Params.UsuarioConectado =
                    usuario.Nombre;
                Params.IdUsuarioConectado = usuario.IdUsuario;
                Params.RolUsuarioConectado = usuario.Rol;

                await DisplayAlert(
                    "Correcto",
                    $"Bienvenido {usuario.Nombre}",
                    "OK");

                // ABRIR MENU
                Application.Current.MainPage =
                    new NavigationPage(
                        new MenuUsuario());
            }
            else
            {
                await DisplayAlert(
                    "Error",
                    "Acceso denegado",
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

    private void cerrarsesionbutton_Clicked(object sender, EventArgs e)
    {
        Microsoft.Maui.Controls.Application.Current?.CloseWindow(this.Window);

    }

    private async void crearcuentaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(
        new RegistroUsuario());
    }
}