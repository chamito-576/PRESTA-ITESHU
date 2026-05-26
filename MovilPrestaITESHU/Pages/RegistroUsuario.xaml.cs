using BIZ;
using COMMON.Entidades;
using COMMON.Modelos;
using System.Collections.ObjectModel;

namespace MovilPrestaITESHU.Pages;

public partial class RegistroUsuario : ContentPage
{
    private UsuariosManager usuariosManager;

    public string Nombre { get; set; }

    public string ApellidoPaterno { get; set; }

    public string ApellidoMaterno { get; set; }

    public string Correo { get; set; }

    public string Contrasena { get; set; }

    public string Matricula { get; set; }

    public string Carrera { get; set; }

    public string Semestre { get; set; }

    public ObservableCollection<string> RolesLista { get; set; }
        = new()
        {
            "Alumno",
            "Docente"
        };
    private LaboratoriosManager laboratoriosManager;

    public ObservableCollection<Laboratorios> LaboratoriosLista { get; set; } = new();

    public Laboratorios LaboratorioSeleccionado { get; set; }

    public string RolSeleccionado { get; set; }

    public RegistroUsuario()
    {
        InitializeComponent();

        BindingContext = this;

        usuariosManager = FabricManager.UsuariosManager;
        laboratoriosManager = FabricManager.LaboratoriosManager;

        CargarLaboratorios();
    }

    private async void Registrarse_Clicked(object sender, EventArgs e)
    {
        try
        {
            Usuarios usuario = new Usuarios
            {
                Nombre = Nombre,
                ApellidoPaterno = ApellidoPaterno,
                ApellidoMaterno = ApellidoMaterno,
                Correo = Correo,
                Contrasena = Contrasena,
                Matricula = Matricula,
                Carrera = Carrera,
                Semestre = Semestre,

                Rol = RolSeleccionado,

                Activo = true,

                FechaAlta = DateTime.Now,

                UsuarioAlta = Nombre
            };

            var resultado =
                await usuariosManager.Agregar(usuario);

            if (resultado != null)
            {
                await DisplayAlert(
                    "Correcto",
                    "Cuenta creada correctamente",
                    "OK");

                CargarLaboratorios();
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert(
                    "Error",
                    usuariosManager.Error,
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


    private async void CargarLaboratorios()
    {
        try
        {
            var lista =
            await laboratoriosManager.ObtenerTodos();

            LaboratoriosLista.Clear();

            if (lista != null)
            {
                foreach (var item in lista)
                {
                    LaboratoriosLista.Add(item);
                }
            }

            OnPropertyChanged(nameof(LaboratoriosLista));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
    private async void cancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(
        new LoginUsuario());

    }
}