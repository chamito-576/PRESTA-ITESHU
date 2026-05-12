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
}