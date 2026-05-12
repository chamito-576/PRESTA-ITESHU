using BIZ;
using COMMON;
using COMMON.Entidades;
using COMMON.Modelos;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace PrestaITESHU.Pages;

public partial class LaboratoriosPage : ContentPage
{
    private UsuariosManager usuariosManager;
    public ObservableCollection<Usuarios> UsuariosLista { get; set; } = new();
    public Usuarios UsuarioSeleccionado { get; set; }
    public bool MostrarFormulario { get; set; } = false;
    private LaboratoriosManager laboratorioManager;
    public ObservableCollection<LaboratoriosViewModel> Laboratorio { get; set; } = new();
    private List<LaboratoriosViewModel> listaOriginal = new();
    public string Nombre { get; set; }
    public string Edificio { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; } = true;
    public string NombreAdministrador { get; set; }
    public string EstadoIcono => Activo ? "✔" : "✖";
    public Color EstadoColor => Activo ? Colors.Green : Colors.Red;
    public bool IsLoading { get; set; }
    private Laboratorios laboratorioSeleccionado;

    public LaboratoriosPage()
    {
        InitializeComponent();

        BindingContext = this;

        laboratorioManager = FabricManager.LaboratoriosManager;
        usuariosManager = FabricManager.UsuariosManager;
        CargarUsuarios();
        CargarLaboratorios();
    }

    private async void CargarLaboratorios()
    {
        try
        {
            IsLoading = true;
            OnPropertyChanged(nameof(IsLoading));

            var listaUsuarios = await usuariosManager.ObtenerTodos();

            var lista = await laboratorioManager.ObtenerTodos();

            Laboratorio.Clear();
            listaOriginal.Clear();

            if (lista != null)
            {
                foreach (var item in lista)
                {
                    var usuario = listaUsuarios?.FirstOrDefault(u => u.IdUsuario == item.IdUsuario);

                    var laboratorioVM = new LaboratoriosViewModel
                    {
                        IdLaboratorio = item.IdLaboratorio,
                        Nombre = item.Nombre,
                        Edificio = item.Edificio,
                        Descripcion = item.Descripcion,
                        Activo = item.Activo,
                        IdUsuario = item.IdUsuario,
                        NombreAdministrador = usuario?.Nombre ?? "Sin administrador"
                    };

                    Laboratorio.Add(laboratorioVM);
                    listaOriginal.Add(laboratorioVM);
                }
            }
            else
            {
                await DisplayAlert("Aviso", laboratorioManager.Error, "OK");
            }

            OnPropertyChanged(nameof(Laboratorio));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    private async void CargarUsuarios()
    {
        try
        {
            var listaUsuarios = await usuariosManager.ObtenerTodos();

            UsuariosLista.Clear();

            if (listaUsuarios != null)
            {
                foreach (var item in listaUsuarios)
                {
                    if (item.Rol == "Administrador")
                    {
                        UsuariosLista.Add(item);
                    }
                }
            }

            OnPropertyChanged(nameof(UsuariosLista));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void LimpiarFormulario()
    {
        Nombre = string.Empty;
        Edificio = string.Empty;
        Descripcion = string.Empty;
        Activo = true;

        laboratorioSeleccionado = null;

        OnPropertyChanged(nameof(Nombre));
        OnPropertyChanged(nameof(Edificio));
        OnPropertyChanged(nameof(Descripcion));
        OnPropertyChanged(nameof(Activo));
    }

    private void OnAgregarClicked(object sender, EventArgs e)
    {
        LimpiarFormulario();

        MostrarFormulario = true;

        OnPropertyChanged(nameof(MostrarFormulario));
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        try
        {
            IsLoading = true;

            OnPropertyChanged(nameof(IsLoading));

            Laboratorios laboratorio;

            // =========================
            // NUEVO
            // =========================
            if (laboratorioSeleccionado == null)
            {
                laboratorio = new Laboratorios
                {
                    Nombre = Nombre,
                    Edificio = Edificio,
                    Descripcion = Descripcion,
                    Activo = Activo,

                    IdUsuario = UsuarioSeleccionado?.IdUsuario,

                    FechaAlta = DateTime.Now,
                    UsuarioAlta = Params.UsuarioConectado,

                    FechaMod = null,
                    UsuarioMod = null
                };

                // VALIDAR FECHA
                if (laboratorio.FechaAlta == DateTime.MinValue)
                {
                    laboratorio.FechaAlta = DateTime.Now;
                }

                var resultado =
                    await laboratorioManager.Agregar(laboratorio);

                if (resultado != null)
                {
                    await DisplayAlert(
                        "Correcto",
                        "Laboratorio agregado correctamente",
                        "OK");
                }
                else
                {
                    await DisplayAlert(
                        "Error",
                        laboratorioManager.Error,
                        "OK");
                }
            }

            // =========================
            // MODIFICAR
            // =========================
            else
            {
                laboratorio = laboratorioSeleccionado;

                laboratorio.Nombre = Nombre;
                laboratorio.Edificio = Edificio;
                laboratorio.Descripcion = Descripcion;
                laboratorio.Activo = Activo;

                laboratorio.IdUsuario =
                    UsuarioSeleccionado?.IdUsuario;

                // CONSERVAR DATOS
                laboratorio.FechaAlta =
                    laboratorioSeleccionado.FechaAlta;

                laboratorio.UsuarioAlta =
                    laboratorioSeleccionado.UsuarioAlta;

                // SI VIENE VACÍO
                if (laboratorio.FechaAlta == DateTime.MinValue)
                {
                    laboratorio.FechaAlta = DateTime.Now;
                }

                if (string.IsNullOrEmpty(laboratorio.UsuarioAlta))
                {
                    laboratorio.UsuarioAlta =
                        Params.UsuarioConectado;
                }

                laboratorio.FechaMod = DateTime.Now;

                laboratorio.UsuarioMod =
                    Params.UsuarioConectado;

                var resultado =
                    await laboratorioManager.Modificar(laboratorio);

                if (resultado != null)
                {
                    await DisplayAlert(
                        "Correcto",
                        "Laboratorio actualizado correctamente",
                        "OK");
                }
                else
                {
                    await DisplayAlert(
                        "Error",
                        laboratorioManager.Error,
                        "OK");
                }
            }

            LimpiarFormulario();
            MostrarFormulario = false;

            OnPropertyChanged(nameof(MostrarFormulario));

            CargarLaboratorios();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
        finally
        {
            IsLoading = false;

            OnPropertyChanged(nameof(IsLoading));
        }
    }

    private void OnEditarClicked(object sender, EventArgs e)
    {
        MostrarFormulario = true;

        OnPropertyChanged(nameof(MostrarFormulario));
        Button btn = sender as Button;

        laboratorioSeleccionado =
            btn.BindingContext as Laboratorios;

        Nombre = laboratorioSeleccionado.Nombre;
        Edificio = laboratorioSeleccionado.Edificio;
        Descripcion = laboratorioSeleccionado.Descripcion;
        Activo = laboratorioSeleccionado.Activo;
        UsuarioSeleccionado = UsuariosLista.FirstOrDefault(x => x.IdUsuario == laboratorioSeleccionado.IdUsuario);

        OnPropertyChanged(nameof(Nombre));
        OnPropertyChanged(nameof(Edificio));
        OnPropertyChanged(nameof(Descripcion));
        OnPropertyChanged(nameof(Activo));
        OnPropertyChanged(nameof(UsuarioSeleccionado));
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        Button btn = sender as Button;

        Laboratorios laboratorio =
            btn.BindingContext as Laboratorios;

        bool respuesta = await DisplayAlert(
            "Confirmar",
            $"¿Eliminar {laboratorio.Nombre}?",
            "Sí",
            "No");

        if (respuesta)
        {
            var eliminado =
                await laboratorioManager.Eliminar(laboratorio.IdLaboratorio.ToString());

            if (eliminado)
            {
                await DisplayAlert(
                    "Correcto",
                    "Eliminado",
                    "OK");

                CargarLaboratorios();
            }
            else
            {
                await DisplayAlert(
                    "Error",
                    laboratorioManager.Error,
                    "OK");
            }
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuAdmin());
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string texto = e.NewTextValue.ToLower();

        var filtrados = listaOriginal
        .Where(x =>

            x.Nombre.ToLower().Contains(texto) ||

            x.Edificio.ToLower().Contains(texto) ||

            x.Descripcion.ToLower().Contains(texto) ||

            x.NombreAdministrador.ToLower().Contains(texto)

        )
        .ToList();

        Laboratorio.Clear();

        foreach (var item in filtrados)
        {
            Laboratorio.Add(item);
        }
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        LimpiarFormulario();

        MostrarFormulario = false;

        OnPropertyChanged(nameof(MostrarFormulario));
    }
}