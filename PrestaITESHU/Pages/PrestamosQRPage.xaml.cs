using BIZ;
using ClosedXML.Excel;
using COMMON;
using COMMON.Entidades;
using COMMON.Modelos;
using System.Collections.ObjectModel;
using ZXing.Net.Maui;

namespace PrestaITESHU.Pages;

public partial class PrestamosQRPage : ContentPage
{
    private PrestamosManager prestamosManager;
    private InventarioManager inventarioManager;
    private SolicitudesManager solicitudesManager;

    private bool procesandoQR = false;

    public bool MostrarFormulario { get; set; }

    public bool MostrarCamara { get; set; }

    public string CodigoQREscaneado { get; set; }

    public PrestamosQRViewModel PrestamoSeleccionado { get; set; }

    public string Estado { get; set; }

    public string Observaciones { get; set; }

    public DateTime FechaDevolucion { get; set; } = DateTime.Now;

    public ObservableCollection<PrestamosQRViewModel> ListaPrestamos { get; set; } = new();

    public PrestamosQRPage()
    {
        InitializeComponent();

        BindingContext = this;

        prestamosManager = FabricManager.PrestamosManager;
        inventarioManager = FabricManager.InventarioManager;
        solicitudesManager = FabricManager.SolicitudesManager;
    }

    private void EscanearQR_Clicked(object sender, EventArgs e)
    {
        procesandoQR = false;

        MostrarCamara = true;
        OnPropertyChanged(nameof(MostrarCamara));

        MainThread.BeginInvokeOnMainThread(() =>
        {
            cameraView.IsDetecting = true;
        });
    }
    

    private async void CameraView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {

        if (procesandoQR)
            return;

        procesandoQR = true;

        try
        {
            var codigo = e?.Results?.FirstOrDefault()?.Value?.Trim();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                procesandoQR = false;
                return;
            }

            CodigoQREscaneado = codigo;

            System.Diagnostics.Debug.WriteLine($"QR: {codigo}");

            // 🔴 DETENER CÁMARA EN UI THREAD
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                cameraView.IsDetecting = false;
                MostrarCamara = false;
                OnPropertyChanged(nameof(MostrarCamara));
            });

            // 🔥 CONVERTIR QR A ID
            int idPrestamo = ParseQR(codigo);

            // 🔥 CONSULTA
            var lista = await prestamosManager.BuscarPrestamoQR(idPrestamo, Params.IdLaboratorioConectado);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                ListaPrestamos.Clear();

                if (lista != null && lista.Count > 0)
                {
                    foreach (var item in lista)
                        ListaPrestamos.Add(item);
                }

                OnPropertyChanged(nameof(ListaPrestamos));
            });

            if (lista == null || lista.Count == 0)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    DisplayAlert("Aviso", "No se encontraron préstamos", "OK");
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                DisplayAlert("Error", ex.Message, "OK");
            });
        }
        finally
        {
            procesandoQR = false;
        }
    }

    private async void GuardarCambios_Clicked(object sender, EventArgs e)
    {
        try
        {
            Prestamos prestamo =
                await prestamosManager.ObtenerPorId(
                    PrestamoSeleccionado.IdPrestamo);

            if (prestamo == null)
            {
                await DisplayAlert(
                    "Error",
                    "Préstamo no encontrado",
                    "OK");

                return;
            }

            string estadoAnterior = prestamo.Estado;

            prestamo.Estado = Estado;
            prestamo.Observaciones = Observaciones;
            prestamo.FechaDevolucion = FechaDevolucion;

            var resultado =
                await prestamosManager.Modificar(prestamo);

            if (resultado != null)
            {
                if (estadoAnterior != "Devuelto" &&
                    Estado == "Devuelto")
                {
                    var solicitud =
                        await solicitudesManager.ObtenerPorId(
                            prestamo.IdSolicitud);

                    if (solicitud != null)
                    {
                        var material =
                            await inventarioManager.ObtenerPorId(
                                solicitud.IdMaterial);

                        if (material != null)
                        {
                            material.Cantidad++;

                            await inventarioManager.Modificar(material);
                        }
                    }
                }

                await DisplayAlert(
                    "Correcto",
                    "Préstamo actualizado",
                    "OK");

                MostrarFormulario = false;

                OnPropertyChanged(nameof(MostrarFormulario));

                await RecargarPrestamos();
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

    private void Cancelar_Clicked(object sender, EventArgs e)
    {
        MostrarFormulario = false;

        OnPropertyChanged(nameof(MostrarFormulario));
    }

    private async void Editar_Clicked(object sender, EventArgs e)
    {
        ImageButton btn = sender as ImageButton;

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
            int idPrestamo =
                Convert.ToInt32(CodigoQREscaneado);

            var lista =
                await prestamosManager.BuscarPrestamoQR(
                    idPrestamo,
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
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }
    private int ParseQR(string codigo)
    {
        try
        {
            if (codigo.Contains("Prestamo:"))
            {
                var partes = codigo.Split('|');
                return Convert.ToInt32(partes[0].Replace("Prestamo:", "").Trim());
            }

            return Convert.ToInt32(codigo);
        }
        catch
        {
            return 0;
        }
    }

    private async void Eliminar_Clicked(object sender, EventArgs e)
    {
        try
        {
            ImageButton btn =
            sender as ImageButton;

            PrestamosQRViewModel prestamo =
                btn.BindingContext
                as PrestamosQRViewModel;

            bool confirmar =
                await DisplayAlert(
                    "Confirmar",
                    "¿Eliminar préstamo?",
                    "Sí",
                    "No");

            if (!confirmar)
                return;

            var prestamoCompleto =
                await prestamosManager.ObtenerPorId(
                    prestamo.IdPrestamo);

            var solicitud =
                await solicitudesManager.ObtenerPorId(
                    prestamoCompleto.IdSolicitud);

            if (solicitud != null)
            {
                var material =
                    await inventarioManager.ObtenerPorId(
                        solicitud.IdMaterial);

                if (material != null)
                {
                    material.Cantidad++;

                    await inventarioManager.Modificar(material);
                }
            }

            bool resultado =
                await prestamosManager.Eliminar(
                    prestamo.IdPrestamo.ToString());

            if (resultado)
            {
                ListaPrestamos.Remove(prestamo);

                await DisplayAlert(
                    "Correcto",
                    "Préstamo eliminado",
                    "OK");
            }

            await RecargarPrestamos();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
            "Error",
            ex.Message,
            "OK");
        }
    }

    private async void Regresar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuAdmin());
    }

    private async void Archivo_Clicked(object sender, EventArgs e)
    {
        try
        {
            ImageButton btn =sender as ImageButton;

            var prestamo =btn.BindingContext as PrestamosQRViewModel;

            if (prestamo == null)
            {
                await DisplayAlert("Error","Préstamo no encontrado","OK");
                return;
            }

            var reporte =await prestamosManager.ObtenerReportePrestamo(prestamo.IdPrestamo);

            if (reporte == null)
            {
                await DisplayAlert("Error","No se pudo generar el reporte","OK");
                return;
            }

            using Stream plantilla =await FileSystem.OpenAppPackageFileAsync("FormatoReporte.xlsx");

            string rutaArchivo =
                Path.Combine(FileSystem.CacheDirectory,$"Reporte_{prestamo.IdPrestamo}.xlsx");

            using FileStream fs =new FileStream(rutaArchivo,FileMode.Create,FileAccess.Write);
            await plantilla.CopyToAsync(fs);

            fs.Close();
            using var workbook =new XLWorkbook(rutaArchivo);

            var ws =workbook.Worksheet(1);

            // LLENAR CELDAS

            ws.Cell("AU2").Value =reporte.Laboratorio;

            ws.Cell("C7").Value =reporte.Carrera;

            ws.Cell("U7").Value =reporte.Laboratorio;

            if (reporte.FechaEntrega.HasValue)
            {
                ws.Cell("BE7").Value =
                    reporte.FechaEntrega.Value;

                ws.Cell("BE7")
                    .Style.DateFormat
                    .Format = "dd/MM/yyyy";
            }

            if (reporte.FechaDevolucion.HasValue)
            {
                ws.Cell("BE9").Value =
                    reporte.FechaDevolucion.Value;

                ws.Cell("BE9")
                    .Style.DateFormat
                    .Format = "dd/MM/yyyy";
            }

            ws.Cell("V12").Value =reporte.Laboratorio;

            ws.Cell("D15").Value =reporte.Material;

            ws.Cell("AW15").Value =reporte.IdMaterial;

            //ws.Cell("BF15").Value =reporte.Cantidad;
            ws.Cell("BF15").Value = 1;

            ws.Cell("M22").Value =reporte.Observaciones;

            ws.Cell("J23").Value =reporte.Carrera;

            ws.Cell("J24").Value =reporte.Laboratorio;

            ws.Cell("J25").Value =reporte.NombreUsuario;

            workbook.Save();

            await Launcher.OpenAsync(new OpenFileRequest
                {
                File =new ReadOnlyFile(rutaArchivo)
                });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error",ex.Message,"OK");
        }
    }
}