using BIZ;
using COMMON;
using COMMON.Entidades;
using COMMON.Modelos;
using System.Collections.ObjectModel;
using ZXing.Net.Maui;

namespace PrestaITESHU.Pages;

public partial class PrestamosQRPage : ContentPage
{
    private PrestamosManager prestamosManager;
    public bool MostrarFormulario { get; set; }
    public string CodigoQREscaneado { get; set; }
    public int IdPrestamoEscaneado { get; set; }

    public PrestamosQRViewModel PrestamoSeleccionado { get; set; }

    public string Estado { get; set; }

    public string Observaciones { get; set; }

    public DateTime FechaDevolucion { get; set; } = DateTime.Now;

    public ObservableCollection<PrestamosQRViewModel> ListaPrestamos { get; set; } = new();

    public bool MostrarCamara { get; set; }
    private bool procesandoQR = false;
    public PrestamosQRPage()
    {
        InitializeComponent();

        BindingContext = this;

        prestamosManager = FabricManager.PrestamosManager;
    }

    private void EscanearQR_Clicked(object sender, EventArgs e)
    {
        MostrarCamara = true;
        cameraView.IsDetecting = true;
        OnPropertyChanged(nameof(MostrarCamara));
    }

    private async void CameraView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        // EVITAR MULTIPLES LECTURAS
        if (procesandoQR)
            return;

        procesandoQR = true;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var codigo =
                    e.Results.FirstOrDefault()?.Value;
                CodigoQREscaneado = codigo;

                if (string.IsNullOrEmpty(codigo))
                {
                    procesandoQR = false;
                    return;
                }

                // OCULTAR CAMARA
                MostrarCamara = false;
                cameraView.IsDetecting = false;
                OnPropertyChanged(nameof(MostrarCamara));

                // BUSCAR PRESTAMO
                var partes = codigo.Split('|');

                var idTexto =
                    partes[0].Replace("Prestamo:", "");

                int idPrestamo =
                    Convert.ToInt32(idTexto);
              
                IdPrestamoEscaneado = idPrestamo;

                var lista =
                    await prestamosManager
                    .BuscarPrestamoQR(
                        idPrestamo,
                        Params.IdLaboratorioConectado);
                if (lista == null)
                {
                    await DisplayAlert(
     "ERROR API",
     $"Error:\n{prestamosManager.Error}",
     "OK");

                    return;
                }
                ListaPrestamos.Clear();

                foreach (var item in lista)
                {
                    ListaPrestamos.Add(item);
                }

                OnPropertyChanged(
                    nameof(ListaPrestamos));
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
                procesandoQR = false;
            }
        });
    }

    private async void Regresar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuAdmin());
    }

    private async void GuardarCambios_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (PrestamoSeleccionado == null)
            {
                await DisplayAlert(
                    "Error",
                    "No hay préstamo seleccionado",
                    "OK");

                return;
            }

            Prestamos prestamo =
                await prestamosManager.ObtenerPorId(
                    PrestamoSeleccionado.IdPrestamo);

            if (prestamo == null)
            {
                await DisplayAlert(
                    "Error",
                    prestamosManager.Error,
                    "OK");

                return;
            }

            prestamo.Estado = Estado;

            prestamo.Observaciones = Observaciones;

            prestamo.FechaDevolucion =
    FechaDevolucion.Date + DateTime.Now.TimeOfDay;

            var resultado =
                await prestamosManager.Modificar(prestamo);

            if (resultado != null)
            {
                await DisplayAlert(
                    "Correcto",
                    "Préstamo actualizado",
                    "OK");

                MostrarFormulario = false;

                await RecargarPrestamos();

                OnPropertyChanged(nameof(MostrarFormulario));
            }
            else
            {
                await DisplayAlert(
                    "Error",
                    prestamosManager.Error,
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

    private async void Cancelar_Clicked(object sender, EventArgs e)
    {
        MostrarFormulario = false;
        OnPropertyChanged(nameof(MostrarFormulario));
    }

    private async void Editar_Clicked(object sender, EventArgs e)
    {
        Button btn = sender as Button;

        PrestamosQRViewModel prestamo =
            btn.BindingContext as PrestamosQRViewModel;

        PrestamoSeleccionado = prestamo;

        Estado = prestamo.Estado;

        Observaciones = prestamo.Observaciones;

        FechaDevolucion =
    prestamo.FechaDevolucion ?? DateTime.Now;

        MostrarFormulario = true;

        OnPropertyChanged(nameof(Estado));
        OnPropertyChanged(nameof(Observaciones));
        OnPropertyChanged(nameof(FechaDevolucion));
        OnPropertyChanged(nameof(MostrarFormulario));
    }
    private async Task RecargarPrestamos()
    {
        try
        {
            var lista = await prestamosManager.BuscarPrestamoQR(IdPrestamoEscaneado, Params.IdLaboratorioConectado);

            ListaPrestamos.Clear();

            foreach (var item in lista)
            {
                ListaPrestamos.Add(item);
            }

            OnPropertyChanged(nameof(ListaPrestamos));
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }
    private async void Eliminar_Clicked(object sender, EventArgs e)
    {
        try
        {
            Button btn = sender as Button;

            PrestamosQRViewModel prestamo =
                btn.BindingContext as PrestamosQRViewModel;

            bool confirmar =
                await DisplayAlert(
                    "Confirmar",
                    "¿Eliminar préstamo?",
                    "Sí",
                    "No");

            if (!confirmar)
                return;

            bool resultado =
                await prestamosManager
                .Eliminar(prestamo.IdPrestamo.ToString());

            if (resultado)
            {
                ListaPrestamos.Remove(prestamo);

                await DisplayAlert(
                    "Correcto",
                    "Préstamo eliminado",
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
        await RecargarPrestamos();
    }
}