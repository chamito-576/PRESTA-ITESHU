using BIZ;
using COMMON;
using COMMON.Entidades;
using COMMON.Modelos;
using System.Collections.ObjectModel;

namespace MovilPrestaITESHU.Pages;

public partial class BuscarMaterial : ContentPage
{
    // MANAGERS
    private LaboratoriosManager laboratorioManager;
    private InventarioManager inventarioManager;
    private SolicitudesManager solicitudesManager;

    // LISTAS
    public ObservableCollection<Laboratorios> Laboratorios { get; set; } = new();

    public ObservableCollection<InventarioViewModel> Materiales { get; set; } = new();

    // LABORATORIO SELECCIONADO
    public Laboratorios LaboratorioSeleccionado { get; set; }

    // BUSCADOR
    public string TextoBusqueda { get; set; }

    // LOADING
    public bool IsLoading { get; set; }

    public BuscarMaterial()
    {
        InitializeComponent();

        BindingContext = this;

        laboratorioManager = FabricManager.LaboratoriosManager;
        inventarioManager = FabricManager.InventarioManager;
        solicitudesManager = FabricManager.SolicitudesManager;

        CargarLaboratorios();
    }
    private async void CargarLaboratorios()
    {
        try
        {
            IsLoading = true;

            OnPropertyChanged(nameof(IsLoading));

            var lista = await laboratorioManager.ObtenerTodos();

            Laboratorios.Clear();

            foreach (var item in lista)
            {
                Laboratorios.Add(item);
            }

            OnPropertyChanged(nameof(Laboratorios));
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

    private async void Picker_SelectedIndexChanged(object sender,EventArgs e)
    {
        if (LaboratorioSeleccionado == null)
            return;

        try
        {
            IsLoading = true;

            OnPropertyChanged(nameof(IsLoading));

            var lista =
                await inventarioManager.ObtenerTodos();

            Materiales.Clear();

            foreach (var item in lista.Where(x =>
                         x.IdLaboratorio ==
                         LaboratorioSeleccionado.IdLaboratorio))
            {
                Materiales.Add(new InventarioViewModel
                {
                    IdMaterial = item.IdMaterial,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    Cantidad = item.Cantidad,
                    IdLaboratorio = item.IdLaboratorio,
                    Activo = item.Activo
                });
            }

            OnPropertyChanged(nameof(Materiales));
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

    private async void OnSolicitarClicked(object sender,EventArgs e)
    {
        try
        {
            var seleccionados =
                Materiales.Where(x => x.Seleccionado).ToList();

            if (seleccionados.Count == 0)
            {
                await DisplayAlert(
                    "Aviso",
                    "Selecciona al menos un material",
                    "OK");

                return;
            }

            foreach (var item in seleccionados)
            {
                Solicitudes solicitud =
                    new Solicitudes
                    {
                        IdUsuario = Params.IdUsuarioConectado,

                        IdMaterial = item.IdMaterial,

                        FechaSolicitud = (DateTime.Now).Date,

                        Estado = "Pendiente"
                    };
                await solicitudesManager.Agregar(solicitud);

                var resultado =
                    await solicitudesManager.Agregar(solicitud);

                if (resultado == null)
                {
                    await DisplayAlert(
                        "Error SQL",
                        solicitudesManager.Error,
                        "OK");

                    return;
                }
            }

            await DisplayAlert(
                "Correcto",
                "Solicitud enviada",
                "OK");

            // LIMPIAR CHECKS
            foreach (var item in Materiales)
            {
                item.Seleccionado = false;
            }

            OnPropertyChanged(nameof(Materiales));
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    private async void regresar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuUsuario());
    }
}