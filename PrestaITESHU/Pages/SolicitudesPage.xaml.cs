using BIZ;
using COMMON;
using COMMON.Entidades;
using COMMON.Modelos;
using System.Collections.ObjectModel;

namespace PrestaITESHU.Pages;

public partial class SolicitudesPage : ContentPage
{
    private SolicitudesManager solicitudesManager;
    private PrestamosManager prestamosManager;
    private InventarioManager inventarioManager;

    public ObservableCollection<SolicitudesViewModel> Solicitudes { get; set; } = new();

    public bool IsLoading { get; set; }

    public SolicitudesPage()
    {
        InitializeComponent();

        BindingContext = this;

        solicitudesManager = FabricManager.SolicitudesManager;
        prestamosManager = FabricManager.PrestamosManager;
        inventarioManager = FabricManager.InventarioManager;

        CargarSolicitudes();
    }

    private async void CargarSolicitudes()
    {
        try
        {
            var lista =await solicitudesManager.ObtenerSolicitudesAdmin(Params.IdUsuarioConectado);

            Solicitudes.Clear();

            foreach (var item in lista)
            {
                if (item.Estado == "Pendiente")
                {
                    Solicitudes.Add(item);
                }
            }

            OnPropertyChanged(nameof(Solicitudes));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error",ex.Message,"OK");
        }
    }

    private async void OnAprobarClicked(object sender,EventArgs e)
    {
        try
        {
            Button btn = sender as Button;
            SolicitudesViewModel solicitud =btn.BindingContext as SolicitudesViewModel;

            // BUSCAR MATERIAL
            var listaMateriales =await inventarioManager.ObtenerTodos();
            
            var material =
                listaMateriales.FirstOrDefault(x =>
                    x.IdMaterial == solicitud.IdMaterial);

            if (material == null)
            {
                await DisplayAlert(
                    "Error",
                    $"Material no encontrado. IdMaterial: {solicitud.IdMaterial}",
                    "OK");

                return;
            }

            // VALIDAR EXISTENCIAS
            if (material.Cantidad <= 0)
            {
                await DisplayAlert("Aviso","No hay existencias disponibles","OK");
                return;
            }

            // CAMBIAR ESTADO SOLICITUD
            bool resultado =await solicitudesManager.CambiarEstadoSolicitud(solicitud.IdSolicitud,"Aprobado");

            if (resultado)
            {
                // DISMINUIR INVENTARIO
                material.Cantidad--;
                var inventarioActualizado =await inventarioManager.Modificar(material);

                if (inventarioActualizado == null)
                {
                    await DisplayAlert("Error",inventarioManager.Error,"OK");
                    return;
                }

                // CREAR PRESTAMO
                Prestamos prestamo = new Prestamos
                {
                    IdSolicitud = solicitud.IdSolicitud,
                    FechaEntrega = DateTime.Now,
                    FechaDevolucion = null,
                    Estado = "Aprobado",
                    CodigoQR = "Prueba",
                    Observaciones = ""
                };

                // GUARDAR PRESTAMO
                var prestamoGuardado = await prestamosManager.Agregar(prestamo);

                if (prestamoGuardado != null)
                {
                    // YA EXISTE EL ID
                    prestamoGuardado.CodigoQR =
                        prestamoGuardado.IdPrestamo.ToString();

                    // ACTUALIZAR
                    await prestamosManager.Modificar(prestamoGuardado);

                    Solicitudes.Remove(solicitud);

                    await DisplayAlert("Correcto","Solicitud aprobada","OK");
                }
                else
                {
                    await DisplayAlert("Error",prestamosManager.Error,"OK");
                }
            }
            else
            {
                await DisplayAlert("Error",solicitudesManager.Error,"OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error",ex.Message,"OK");
        }
    }

    private async void OnRechazarClicked(object sender,EventArgs e)
    {
        Button btn = sender as Button;

        SolicitudesViewModel solicitud =btn.BindingContext as SolicitudesViewModel;

        bool resultado =await solicitudesManager.CambiarEstadoSolicitud(solicitud.IdSolicitud,"Rechazado");

        if (resultado)
        {
            Solicitudes.Remove(solicitud);
            await DisplayAlert("Correcto","Solicitud rechazada","OK");
            CargarSolicitudes();
        }
    }

    private async void Button_Clicked(object sender,EventArgs e)
    {
        await Navigation.PushAsync(new MenuAdmin());
    }
}