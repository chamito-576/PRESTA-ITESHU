using BIZ;
using COMMON;
using COMMON.Modelos;
using System.Collections.ObjectModel;

namespace MovilPrestaITESHU.Pages;

public partial class HistorialPrestamosPage : ContentPage
{
    private PrestamosManager prestamosManager;

    public ObservableCollection<HistorialPrestamoViewModel> Historial{ get; set; } = new();
    public HistorialPrestamosPage()
	{
		InitializeComponent();
        BindingContext = this;
        prestamosManager =FabricManager.PrestamosManager;
        CargarHistorial();
    }
    private async void CargarHistorial()
    {
        try
        {
            var lista =await prestamosManager.ObtenerHistorialUsuario(Params.IdUsuarioConectado);
            Historial.Clear();
            foreach (var item in lista)
            {
                Historial.Add(item);
            }
            OnPropertyChanged(nameof(Historial));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error",ex.Message,"OK");
        }
    }

    private async void regresar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuUsuario());
    }
}