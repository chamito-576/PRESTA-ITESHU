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

    public ObservableCollection<SolicitudesViewModel>Solicitudes { get; set; } = new();

    public bool IsLoading { get; set; }

    public SolicitudesPage()
    {
        InitializeComponent();

        BindingContext = this;

        solicitudesManager = FabricManager.SolicitudesManager;
        prestamosManager = FabricManager.PrestamosManager;

        CargarSolicitudes();
    }

    private async void CargarSolicitudes()
    {
        try
        {
            var lista =
                await solicitudesManager
                .ObtenerSolicitudesAdmin(
                    Params.IdUsuarioConectado);

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
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    private async void OnAprobarClicked(object sender,EventArgs e)
    {
        try
        {
            Button btn = sender as Button;

            SolicitudesViewModel solicitud =
                btn.BindingContext as SolicitudesViewModel;

            bool resultado =
                await solicitudesManager
                .CambiarEstadoSolicitud(
                    solicitud.IdSolicitud,
                    "Aprobado");

            if (resultado)
            {
                Prestamos prestamo = new Prestamos
                {
                    IdSolicitud = solicitud.IdSolicitud,
                    FechaEntrega = DateTime.Now,
                    FechaDevolucion = null,
                    Estado ="Aprobado",
                    CodigoQR = $"Prestamo:{solicitud.IdSolicitud}" +
                    $"|Usuario:{solicitud.NombreUsuario}" +
                    $"|Fecha:{DateTime.Now}",
                    Observaciones = ""
                };
                await DisplayAlert("DEBUG",
                    $"Solicitud: {prestamo.IdSolicitud}\n" +
                    $"Estado: {prestamo.Estado}\n" +
                    $"QR: {prestamo.CodigoQR}",
                    "OK");
                var prestamoGuardado =
                    await prestamosManager
                    .Agregar(prestamo);

                if (prestamoGuardado != null)
                {
                    // OCULTAR
                    Solicitudes.Remove(solicitud);

                    await DisplayAlert(
                        "Correcto",
                        "Solicitud aprobada",
                        "OK");
                }
                else
                {
                    await DisplayAlert(
                        "Error",
                        prestamosManager.Error,
                        "OK");
                }
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

    private async void OnRechazarClicked(object sender,EventArgs e)
    {
        Button btn = sender as Button;

        SolicitudesViewModel solicitud =
            btn.BindingContext as SolicitudesViewModel;

        bool resultado =
            await solicitudesManager
            .CambiarEstadoSolicitud(
                solicitud.IdSolicitud,
                "Rechazado");

        if (resultado)
        {
            Solicitudes.Remove(solicitud);
            await DisplayAlert(
                "Correcto",
                "Solicitud rechazada",
                "OK");

            CargarSolicitudes();
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuAdmin());
    }
}