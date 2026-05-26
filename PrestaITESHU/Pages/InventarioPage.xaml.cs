using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using BIZ;
using COMMON;
using COMMON.Entidades;
using COMMON.Modelos;
using System.Collections.ObjectModel;

namespace PrestaITESHU.Pages;

public partial class InventarioPage : ContentPage
{
    private InventarioManager inventarioManager;
    private LaboratoriosManager laboratoriosManager;

    public ObservableCollection<InventarioViewModel> Inventarios { get; set; } = new();

    private List<InventarioViewModel> listaOriginal = new();

    public ObservableCollection<Laboratorios> LaboratoriosLista { get; set; } = new();

    public Laboratorios LaboratorioSeleccionado { get; set; }

    public bool MostrarFormulario { get; set; }

    public string Nombre { get; set; }

    public string Descripcion { get; set; }

    public int Cantidad { get; set; }

    public bool Activo { get; set; } = true;

    private Inventario inventarioSeleccionado;

    public InventarioPage()
    {
        InitializeComponent();

        BindingContext = this;

        inventarioManager = FabricManager.InventarioManager;
        laboratoriosManager = FabricManager.LaboratoriosManager;

        CargarLaboratorios();
        CargarInventario();
    }

    private async void CargarLaboratorios()
    {
        var lista = await laboratoriosManager.ObtenerTodos();

        LaboratoriosLista.Clear();

        foreach (var item in lista)
        {
            LaboratoriosLista.Add(item);
        }

        OnPropertyChanged(nameof(LaboratoriosLista));
    }

    private async void CargarInventario()
    {
        var listaInventario = await inventarioManager.ObtenerTodos();

        var listaLaboratorios = await laboratoriosManager.ObtenerTodos();

        Inventarios.Clear();
        listaOriginal.Clear();

        foreach (var item in listaInventario)
        {
            var laboratorio = listaLaboratorios
                .FirstOrDefault(x => x.IdLaboratorio == item.IdLaboratorio);

            var vm = new InventarioViewModel
            {
                IdMaterial = item.IdMaterial,
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                Cantidad = item.Cantidad,
                Activo = item.Activo,
                IdLaboratorio = item.IdLaboratorio,
                NombreLaboratorio = laboratorio?.Nombre
            };

            Inventarios.Add(vm);
            listaOriginal.Add(vm);
        }

        OnPropertyChanged(nameof(Inventarios));
    }

    private void LimpiarFormulario()
    {
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Cantidad = 0;
        Activo = true;
        LaboratorioSeleccionado = null;

        inventarioSeleccionado = null;

        OnPropertyChanged(nameof(Nombre));
        OnPropertyChanged(nameof(Descripcion));
        OnPropertyChanged(nameof(Cantidad));
        OnPropertyChanged(nameof(Activo));
        OnPropertyChanged(nameof(LaboratorioSeleccionado));
    }

    private void OnAgregarClicked(object sender, EventArgs e)
    {
        LimpiarFormulario();

        MostrarFormulario = true;

        OnPropertyChanged(nameof(MostrarFormulario));
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        try
        {
            Inventario inventario;

            // NUEVO
            if (inventarioSeleccionado == null)
            {
                inventario = new Inventario
                {
                    Nombre = Nombre,
                    Descripcion = Descripcion,
                    Cantidad = Cantidad,
                    IdLaboratorio = LaboratorioSeleccionado.IdLaboratorio,
                    Activo = Activo,

                    FechaAlta = DateTime.Now,
                    UsuarioAlta = Params.UsuarioConectado
                };

                var resultado = await inventarioManager.Agregar(inventario);

                if (resultado != null)
                {
                    await DisplayAlert(
                        "Correcto",
                        "Inventario agregado correctamente",
                        "OK");
                }
                else
                {
                    await DisplayAlert(
                        "Error",
                        inventarioManager.Error,
                        "OK");
                }
            }

            // EDITAR
            else
            {
                inventario = inventarioSeleccionado;

                inventario.Nombre = Nombre;
                inventario.Descripcion = Descripcion;
                inventario.Cantidad = Cantidad;
                inventario.IdLaboratorio = LaboratorioSeleccionado.IdLaboratorio;
                inventario.Activo = Activo;

                // CONSERVAR DATOS
                inventario.FechaAlta =
                    inventarioSeleccionado.FechaAlta;

                inventario.UsuarioAlta =
                    inventarioSeleccionado.UsuarioAlta;

                // SI VIENE VACÍO
                if (inventario.FechaAlta == DateTime.MinValue)
                {
                    inventario.FechaAlta = DateTime.Now;
                }

                if (string.IsNullOrEmpty(inventario.UsuarioAlta))
                {
                    inventario.UsuarioAlta =
                        Params.UsuarioConectado;
                }

                inventario.FechaMod = DateTime.Now;

                inventario.UsuarioMod =
                    Params.UsuarioConectado;

                var resultado =
                    await inventarioManager.Modificar(inventario);

                if (resultado != null)
                {
                    await DisplayAlert(
                        "Correcto",
                        "Inventario actualizado correctamente",
                        "OK");
                }
                else
                {
                    await DisplayAlert(
                        "Error",
                        inventarioManager.Error,
                        "OK");
                }
            }

            LimpiarFormulario();

            MostrarFormulario = false;

            OnPropertyChanged(nameof(MostrarFormulario));

            CargarInventario();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    private void OnEditarClicked(object sender, EventArgs e)
    {
        MostrarFormulario = true;

        OnPropertyChanged(nameof(MostrarFormulario));

        Button btn = sender as Button;

        var vm = btn.BindingContext as InventarioViewModel;

        inventarioSeleccionado = new Inventario
        {
            IdMaterial = vm.IdMaterial,
            Nombre = vm.Nombre,
            Descripcion = vm.Descripcion,
            Cantidad = vm.Cantidad,
            Activo = vm.Activo,
            IdLaboratorio = vm.IdLaboratorio
        };

        Nombre = inventarioSeleccionado.Nombre;
        Descripcion = inventarioSeleccionado.Descripcion;
        Cantidad = inventarioSeleccionado.Cantidad;
        Activo = inventarioSeleccionado.Activo;

        LaboratorioSeleccionado =
            LaboratoriosLista.FirstOrDefault(x =>
                x.IdLaboratorio == inventarioSeleccionado.IdLaboratorio);

        OnPropertyChanged(nameof(Nombre));
        OnPropertyChanged(nameof(Descripcion));
        OnPropertyChanged(nameof(Cantidad));
        OnPropertyChanged(nameof(Activo));
        OnPropertyChanged(nameof(LaboratorioSeleccionado));
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        Button btn = sender as Button;

        var vm = btn.BindingContext as InventarioViewModel;

        bool respuesta = await DisplayAlert(
            "Confirmar",
            $"¿Eliminar {vm.Nombre}?",
            "Sí",
            "No");

        if (respuesta)
        {
            await inventarioManager.Eliminar(vm.IdMaterial.ToString());

            CargarInventario();
        }
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        LimpiarFormulario();

        MostrarFormulario = false;

        OnPropertyChanged(nameof(MostrarFormulario));
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string texto = e.NewTextValue.ToLower();

        var filtrados = listaOriginal
        .Where(x =>

            x.Nombre?.ToLower().Contains(texto) == true ||

            x.Descripcion?.ToLower().Contains(texto) == true ||

            x.NombreLaboratorio?.ToLower().Contains(texto) == true ||

            x.Cantidad.ToString().Contains(texto)

        ).ToList();

        Inventarios.Clear();

        foreach (var item in filtrados)
        {
            Inventarios.Add(item);
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuAdmin());
    }

    private async void Expediente_Clicked(object sender, EventArgs e)
    {
        try
        {
            var expediente =await inventarioManager.ObtenerExpedienteInventario(Params.IdLaboratorioConectado);

            if (expediente == null)
            {
                await DisplayAlert("Error","No se pudo obtener el expediente","OK");
                return;
            }

            string rutaPdf =Path.Combine(FileSystem.CacheDirectory,$"Expediente_{DateTime.Now:yyyyMMddHHmmss}.pdf");

            QuestPDF.Settings.License =LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("EXPEDIENTE DE INVENTARIO Y PRÉSTAMOS").Bold().FontSize(22).AlignCenter();
                    page.Content().Column(column =>
                    {
                        // DATOS GENERALES

                        column.Item().Text($"Laboratorio: {expediente.Laboratorio}");
                        column.Item().Text($"Fecha generación: {expediente.FechaGeneracion:dd/MM/yyyy}");
                        column.Item().Text($"Total movimientos: {expediente.TotalMovimientos}");
                        column.Item().PaddingVertical(10);

                        // RESUMEN GENERAL

                        column.Item().Text("RESUMEN GENERAL").Bold().FontSize(18);
                        column.Item().Text($"Total inventario: {expediente.TotalInventario}");
                        column.Item().Text($"Disponible para préstamo: {expediente.Disponibles}");
                        column.Item().Text($"Actualmente prestado: {expediente.Prestados}");
                        column.Item().Text($"Material devuelto: {expediente.Devueltos}");
                        column.Item().Text($"Material retrasado: {expediente.Retrasados}");
                        column.Item().PaddingVertical(15);

                        // TABLA BITÁCORA

                        column.Item().Text("BITÁCORA DE MOVIMIENTOS").Bold().FontSize(18).AlignCenter();
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1);
                            });

                            // HEADER
                            table.Header(header =>
                            {
                                header.Cell().Text("Material").Bold();
                                header.Cell().Text("Descripción").Bold();
                                header.Cell().Text("Usuario").Bold();
                                header.Cell().Text("Fecha entrega").Bold();
                                header.Cell().Text("Fecha devolución").Bold();
                                header.Cell().Text("Estado").Bold();
                            });

                            foreach (var item in expediente.Movimientos)
                            {
                                table.Cell().Text(item.Material);
                                table.Cell().Text(item.Descripcion);
                                table.Cell().Text(item.Usuario);
                                table.Cell().Text(item.FechaEntrega?.ToString("dd/MM/yyyy")?? "");
                                table.Cell().Text(item.FechaDevolucion?.ToString("dd/MM/yyyy")?? "");
                                table.Cell().Text(item.Estado);
                            }
                        });
                    });
                    page.Footer().AlignCenter().Text(text =>
                        {
                            text.Span("Generado automáticamente - ");
                            text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        });
                });
            })
            .GeneratePdf(rutaPdf);

            await Launcher.OpenAsync(
                new OpenFileRequest
                {
                    File =new ReadOnlyFile(rutaPdf)
                });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error",ex.Message,"OK");
        }
    }
}