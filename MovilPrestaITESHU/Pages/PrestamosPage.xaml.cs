using BIZ;
using COMMON;
using COMMON.Entidades;
using COMMON.Modelos;
using System.Collections.ObjectModel;

namespace MovilPrestaITESHU.Pages;

public partial class PrestamosPage : ContentPage
{
    private PrestamosManager prestamosManager;
    private SolicitudesManager solicitudesManager;

    private InventarioManager inventarioManager;
    public ObservableCollection<PrestamosViewModel> Prestamos { get; set; } = new();

    public PrestamosPage()
    {
        InitializeComponent();

        BindingContext = this;
        prestamosManager = FabricManager.PrestamosManager;
        solicitudesManager = FabricManager.SolicitudesManager;
        inventarioManager = FabricManager.InventarioManager;

        CargarPrestamos();
    }

    private async void CargarPrestamos()
    {
        try
        {
            var lista =
                await prestamosManager
                .ObtenerPrestamosUsuario(
                    Params.IdUsuarioConectado);

            var solicitudes =await solicitudesManager.ObtenerTodos();

            var inventario =await inventarioManager.ObtenerTodos();

            Prestamos.Clear();

            foreach (var item in lista)
            {
                var solicitud =
                    solicitudes.FirstOrDefault(s =>
                        s.IdSolicitud == item.IdSolicitud);

                string nombreMaterial = "";

                if (solicitud != null)
                {
                    var material =
                        inventario.FirstOrDefault(i =>
                            i.IdMaterial == solicitud.IdMaterial);

                    nombreMaterial =
                        material?.Nombre ?? "";
                }

                Prestamos.Add(new PrestamosViewModel
                {
                    IdPrestamo = item.IdPrestamo,
                    IdSolicitud = item.IdSolicitud,
                    FechaEntrega = item.FechaEntrega,
                    FechaDevolucion = item.FechaDevolucion,
                    Estado = item.Estado,
                    CodigoQR = item.CodigoQR,
                    Observaciones = item.Observaciones,
                    NombreMaterial = nombreMaterial
                });
            }

            OnPropertyChanged(nameof(Prestamos));
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    private async void regresarbutton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuUsuario());
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Grid grid = sender as Grid;

        PrestamosViewModel prestamo = grid.BindingContext as PrestamosViewModel;

        await Navigation.PushAsync(
            new DetalleQRPage(prestamo));
    }
}