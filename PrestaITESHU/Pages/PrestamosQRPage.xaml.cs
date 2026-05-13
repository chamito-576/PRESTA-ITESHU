using BIZ;
using COMMON;
using COMMON.Modelos;
using System.Collections.ObjectModel;
using ZXing.Net.Maui;

namespace PrestaITESHU.Pages;

public partial class PrestamosQRPage : ContentPage
{
    private PrestamosManager prestamosManager;

    public ObservableCollection<PrestamosQRViewModel> ListaPrestamos{ get; set; } = new();

    public bool MostrarCamara { get; set; }
    public PrestamosQRPage()
	{
		InitializeComponent();
        BindingContext = this;

        prestamosManager = FabricManager.PrestamosManager;
    }
    private void EscanearQR_Clicked(
        object sender,
        EventArgs e)
    {
        MostrarCamara = true;

        OnPropertyChanged(nameof(MostrarCamara));
    }

    private async void CameraView_BarcodesDetected(
        object sender,
        BarcodeDetectionEventArgs e)
    {
        var resultado =
            e.Results.FirstOrDefault();

        if (resultado == null)
            return;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            MostrarCamara = false;

            OnPropertyChanged(nameof(MostrarCamara));

            string codigoQR = resultado.Value;

            await DisplayAlert(
                "QR Detectado",
                codigoQR,
                "OK");

            var lista =
                await prestamosManager.BuscarPorQR(
                    codigoQR,
                    Params.IdLaboratorioConectado);

            ListaPrestamos.Clear();

            if (lista != null)
            {
                foreach (var item in lista)
                {
                    ListaPrestamos.Add(item);
                }
            }

            OnPropertyChanged(nameof(ListaPrestamos));
        });
    }

    private async void Regresar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuAdmin());
    }
}