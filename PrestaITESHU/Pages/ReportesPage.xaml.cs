using ClosedXML.Excel;
using Syncfusion.XlsIO;
using Syncfusion.Pdf;
using System.Collections.ObjectModel;
using COMMON.Modelos;
using BIZ;
using COMMON;
using NPOI.SS.UserModel;

namespace PrestaITESHU.Pages;

public partial class ReportesPage : ContentPage
{
    private UsuariosManager usuariosManager;

    private LaboratoriosManager laboratoriosManager;

    private InventarioManager inventarioManager;

    private SolicitudesManager solicitudesManager;
    private PrestamosManager prestamosManager;
    public ObservableCollection<ReporteExcelViewModel>Reportes{ get; set; } = new();

    public ReportesPage()
    {
        InitializeComponent();
        BindingContext = this;
        usuariosManager =
            FabricManager.UsuariosManager;

        laboratoriosManager =
            FabricManager.LaboratoriosManager;

        inventarioManager =
            FabricManager.InventarioManager;

        solicitudesManager =
            FabricManager.SolicitudesManager;

        prestamosManager = 
            FabricManager.PrestamosManager;
    }

    private async void GenerarReportes_Clicked(
    object sender,
    EventArgs e)
    {
        try
        {
            Reportes.Clear();

            var administrador =
                await usuariosManager.ObtenerPorId(
                    Params.IdUsuarioConectado);

            int idLaboratorio =
                Params.IdLaboratorioConectado;

            var laboratorio =
                await laboratoriosManager.ObtenerPorId(
                    idLaboratorio);

            var solicitudes =
                await solicitudesManager.ObtenerTodos();

            var usuarios =
                await usuariosManager.ObtenerTodos();

            var prestamos =
                await prestamosManager.ObtenerTodos();

            var inventario =
                await inventarioManager.ObtenerTodos();

            var solicitudesLab =
                solicitudes.Where(s =>
                    usuarios.Any(u =>
                        u.IdUsuario == s.IdUsuario &&
                        u.IdLaboratorio ==
                        idLaboratorio))
                .ToList();

            int contador = 1;

            foreach (var solicitud in solicitudesLab)
            {
                var material =
                    inventario.FirstOrDefault(i =>
                        i.IdMaterial ==
                        solicitud.IdMaterial);

                if (material == null)
                    continue;

                var prestamo =
                    prestamos.FirstOrDefault(p =>
                        p.IdSolicitud ==
                        solicitud.IdSolicitud);

                using var stream =
                    await FileSystem
                    .OpenAppPackageFileAsync(
                        "PlantillaReporte.xlsx");

                string ruta =
                    Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.Desktop),
                        $"Reporte_{contador}.xlsx");

                using (var fileStream =
                    File.Create(ruta))
                {
                    await stream.CopyToAsync(
                        fileStream);
                }

                using (var workbook =
                    new ClosedXML.Excel.XLWorkbook(
                        ruta))
                {
                    var hoja =
                        workbook.Worksheet(1);

                    hoja.Cell("AU2").Value =
                        laboratorio.Nombre;

                    hoja.Cell("BE7").Value =
                        prestamo?.FechaEntrega;

                    hoja.Cell("BE9").Value =
                        prestamo?.FechaDevolucion;

                    hoja.Cell("V12").Value =
                        laboratorio.Nombre;

                    hoja.Cell("D15").Value =
                        material.Descripcion;

                    hoja.Cell("AW15").Value =
                        material.IdMaterial;

                    hoja.Cell("BF15").Value =
                        material.Cantidad;

                    hoja.Cell("M22").Value =
                        prestamo?.Observaciones;

                    hoja.Cell("J25").Value =
                        administrador.Nombre + " " +
                        administrador.ApellidoPaterno;

                    workbook.Save();
                }

                // AGREGAR A LISTA
                Reportes.Add(
                    new ReporteExcelViewModel
                    {
                        NombreArchivo =
                            $"Reporte_{contador}.xlsx",

                        RutaArchivo = ruta
                    });

                contador++;
            }

            await DisplayAlert(
                "Correcto",
                "Reportes generados",
                "OK");
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

    private async void GenerarPDF_Clicked(
    object sender,
    EventArgs e)
    {
        try
        {
            // ADMINISTRADOR
            var administrador =
                await usuariosManager.ObtenerPorId(
                    Params.IdUsuarioConectado);

            int idLaboratorio =
                Params.IdLaboratorioConectado;

            // LABORATORIO
            var laboratorio =
                await laboratoriosManager.ObtenerPorId(
                    idLaboratorio);

            // SOLICITUDES
            var solicitudes =
                await solicitudesManager.ObtenerTodos();

            // USUARIOS
            var usuarios =
                await usuariosManager.ObtenerTodos();

            // PRÉSTAMOS
            var prestamos =
                await prestamosManager.ObtenerTodos();

            // INVENTARIO
            var inventario =
                await inventarioManager.ObtenerTodos();

            // FILTRAR SOLICITUDES
            var solicitudesLab =
                solicitudes.Where(s =>
                    usuarios.Any(u =>
                        u.IdUsuario == s.IdUsuario &&
                        u.IdLaboratorio ==
                        idLaboratorio))
                .ToList();

            int contador = 1;

            foreach (var solicitud in solicitudesLab)
            {
                var material =
                    inventario.FirstOrDefault(i =>
                        i.IdMaterial ==
                        solicitud.IdMaterial);

                if (material == null)
                    continue;

                var prestamo =
                    prestamos.FirstOrDefault(p =>
                        p.IdSolicitud ==
                        solicitud.IdSolicitud);

                // COPIAR PLANTILLA
                using var stream =
                    await FileSystem.OpenAppPackageFileAsync(
                        "PlantillaReporte.xlsx");

                string rutaExcel =
                    Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.Desktop),
                        $"Reporte_{contador}.xlsx");

                using (var fileStream =
                    File.Create(rutaExcel))
                {
                    await stream.CopyToAsync(fileStream);
                }

                // LLENAR EXCEL
                using (var workbook =
                    new XLWorkbook(rutaExcel))
                {
                    var hoja =
                        workbook.Worksheet(1);

                    hoja.Cell("AU2").Value =
                        laboratorio.Nombre;

                    hoja.Cell("BE7").Value =
                        prestamo?.FechaEntrega;

                    hoja.Cell("BE9").Value =
                        prestamo?.FechaDevolucion;

                    hoja.Cell("V12").Value =
                        laboratorio.Nombre;

                    hoja.Cell("D15").Value =
                        material.Descripcion;

                    hoja.Cell("AW15").Value =
                        material.IdMaterial;

                    hoja.Cell("BF15").Value =
                        material.Cantidad;

                    hoja.Cell("M22").Value =
                        prestamo?.Observaciones;

                    hoja.Cell("J25").Value =
                        administrador.Nombre + " " +
                        administrador.ApellidoPaterno;

                    workbook.Save();
                }

                // CONVERTIR A PDF
                string rutaPDF =
                    Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.Desktop),
                        $"Reporte_{contador}.pdf");

                Microsoft.Office.Interop.Excel.Application app =
                    new Microsoft.Office.Interop.Excel.Application();

                Microsoft.Office.Interop.Excel.Workbook wb =
                    app.Workbooks.Open(rutaExcel);

                wb.ExportAsFixedFormat(
                    Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF,
                    rutaPDF);

                wb.Close(false);

                app.Quit();

                contador++;
            }

            await DisplayAlert(
                "Correcto",
                "PDFs generados en el escritorio",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    private async void AbrirReporte_Clicked(object sender, EventArgs e)
    {
        try
        {
            Button boton =
                sender as Button;

            ReporteExcelViewModel reporte =
                boton.BindingContext
                as ReporteExcelViewModel;

            await Launcher.OpenAsync(
                new OpenFileRequest
                {
                    File =
                        new ReadOnlyFile(
                            reporte.RutaArchivo)
                });
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }
}