using BIZ;
using COMMON;
using COMMON.Entidades;
using System.Collections.ObjectModel;

namespace MovilPrestaITESHU.Pages;

public partial class PrestamosPage : ContentPage
{
    private PrestamosManager prestamosManager;

    public ObservableCollection<Prestamos>Prestamos{ get; set; } = new();

    public PrestamosPage()
    {
        InitializeComponent();

        BindingContext = this;

        prestamosManager = FabricManager.PrestamosManager;

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

            Prestamos.Clear();

            foreach (var item in lista)
            {
                Prestamos.Add(item);
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

        Prestamos prestamo =
            grid.BindingContext as Prestamos;

        await Navigation.PushAsync(
            new DetalleQRPage(prestamo));
    }
}