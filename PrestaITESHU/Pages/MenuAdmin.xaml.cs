using BIZ;
using COMMON;
using COMMON.Entidades;
using PrestaITESHU.Pages;
namespace PrestaITESHU.Pages;

public partial class MenuAdmin : ContentPage
{
    public string RolAdmin { get; set; }
    public string NombreAdmin { get; set; }
    public string FechaHoraActual { get; set; }
    private UsuariosManager usuariosManager;

    private Usuarios administradorActual;
    public string Nombrecom { get; set; }
    public bool MostrarFormulario { get; set; }

    public string Nombre { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public string Correo { get; set; }
    public string Contrasena { get; set; }
    public string Matricula { get; set; }
    public string Carrera { get; set; }
    public string Semestre { get; set; }
    public MenuAdmin()
	{
		InitializeComponent();


        BindingContext = this;
        usuariosManager = FabricManager.UsuariosManager;
        NombreAdmin = Params.UsuarioConectado;
        OnPropertyChanged(nameof(NombreAdmin));
        RolAdmin = Params.RolUsuarioConectado;
        OnPropertyChanged(nameof(RolAdmin));
        CargarAdministrador();

        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            FechaHoraActual = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            OnPropertyChanged(nameof(FechaHoraActual));

            return true;
        });
    }

    private async void CargarAdministrador()
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
            ApellidoPaterno =administradorActual.ApellidoPaterno;
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

    private async void cerrarsesionbutton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginAdmin());
    }

    private void ingresardatosbutton_Clicked(object sender, EventArgs e)
    {
        MostrarFormulario = true;
        OnPropertyChanged(nameof(MostrarFormulario));
    }

    private async void usuariosbutton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UsuariosPage());
    }

    private async void laboratoriosbutton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LaboratoriosPage());
    }

    private async void inventariobutton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventarioPage());
    }

    private async void solicitudesbutton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SolicitudesPage());
    }

    private void prestamosdevolucionesbutton_Clicked(object sender, EventArgs e)
    {

    }

    private void reportesbutton_Clicked(object sender, EventArgs e)
    {

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

            OnPropertyChanged(nameof(MostrarFormulario));
        }
        else
        {
            await DisplayAlert(
                "Error",
                usuariosManager.Error,
                "OK");
        }
        CargarAdministrador();
    }

    private void Cancelar_Clicked(object sender, EventArgs e)
    {
        MostrarFormulario = false;
        OnPropertyChanged(nameof(MostrarFormulario));
    }
}