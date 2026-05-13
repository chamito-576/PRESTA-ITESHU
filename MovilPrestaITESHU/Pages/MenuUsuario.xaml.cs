using BIZ;
using COMMON;
using COMMON.Entidades;

namespace MovilPrestaITESHU.Pages;

public partial class MenuUsuario : ContentPage
{
    public string RolAdmin { get; set; }
    public string NombreAdmin { get; set; }
    public bool MostrarFormulario { get; set; }
    private UsuariosManager usuariosManager;
    public string Nombrecom { get; set; }
    private Usuarios administradorActual;
    public string Nombre { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public string Correo { get; set; }
    public string Contrasena { get; set; }
    public string Matricula { get; set; }
    public string Carrera { get; set; }
    public string Semestre { get; set; }
    public MenuUsuario()
	{
		InitializeComponent();
        BindingContext = this;
        usuariosManager = FabricManager.UsuariosManager;
        NombreAdmin = Params.UsuarioConectado;
        OnPropertyChanged(nameof(NombreAdmin));
        RolAdmin = Params.RolUsuarioConectado;
        OnPropertyChanged(nameof(RolAdmin));
        CargarUsuarios();
    }
    private async void CargarUsuarios()
    {
        administradorActual =
            await usuariosManager.ObtenerPorId(
                Params.IdUsuarioConectado);

        Nombrecom =
                $"{administradorActual.Nombre} " +
                $"{administradorActual.ApellidoPaterno} " +
                $"{administradorActual.ApellidoMaterno}";

        OnPropertyChanged(nameof(Nombrecom));

        if (administradorActual != null)
        {
            Nombre = administradorActual.Nombre;
            ApellidoPaterno = administradorActual.ApellidoPaterno;
            ApellidoMaterno = administradorActual.ApellidoMaterno;
            Correo = administradorActual.Correo;
            Contrasena = administradorActual.Contrasena;
            Matricula = administradorActual.Matricula;
            Carrera = administradorActual.Carrera;
            Semestre = administradorActual.Semestre;

            OnPropertyChanged(nameof(Nombre));
            OnPropertyChanged(nameof(ApellidoPaterno));
            OnPropertyChanged(nameof(ApellidoMaterno));
            OnPropertyChanged(nameof(Correo));
            OnPropertyChanged(nameof(Contrasena));
            OnPropertyChanged(nameof(Matricula));
            OnPropertyChanged(nameof(Carrera));
            OnPropertyChanged(nameof(Semestre));
        }
    }

    private async void buscarmaterial_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BuscarMaterial());
    }

    private async void misprestamos_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PrestamosPage());
    }

    private async void cerrarsesionbutton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginUsuario());
    }

    private void ingresardatosbutton_Clicked(object sender, EventArgs e)
    {
        MostrarFormulario = true;
        OnPropertyChanged(nameof(MostrarFormulario));
    }

    private void Cancelar_Clicked(object sender, EventArgs e)
    {
        MostrarFormulario = false;
        OnPropertyChanged(nameof(MostrarFormulario));
    }

    private async void GuardarCambios_Clicked(object sender, EventArgs e)
    {
        administradorActual.Nombre = Nombre;
        administradorActual.ApellidoPaterno = ApellidoPaterno;
        administradorActual.ApellidoMaterno = ApellidoMaterno;
        administradorActual.Correo = Correo;
        administradorActual.Contrasena = Contrasena;
        administradorActual.Matricula = Matricula;
        administradorActual.Carrera = Carrera;
        administradorActual.Semestre = Semestre;

        var resultado =
            await usuariosManager.Modificar(administradorActual);

        if (resultado != null)
        {
            Params.UsuarioConectado = Nombre;

            NombreAdmin = Nombre;

            OnPropertyChanged(nameof(NombreAdmin));

            await DisplayAlert(
                "Correcto",
                "Información actualizada",
                "OK");

            MostrarFormulario = false;
            CargarUsuarios();

            OnPropertyChanged(nameof(MostrarFormulario));
        }
        else
        {
            await DisplayAlert(
                "Error",
                usuariosManager.Error,
                "OK");
        }
    }
}