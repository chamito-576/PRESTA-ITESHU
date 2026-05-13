using COMMON.Entidades;

namespace MovilPrestaITESHU.Pages;

public partial class DetalleQRPage : ContentPage
{
    public string CodigoQR { get; set; }

    public DetalleQRPage(Prestamos prestamo)
    {
        InitializeComponent();

        CodigoQR = prestamo.CodigoQR;

        BindingContext = this;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PrestamosPage());
    }
}