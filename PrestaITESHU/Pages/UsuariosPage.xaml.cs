using BIZ;
using COMMON;
using COMMON.Entidades;
using COMMON.Modelos;
using System.Collections.ObjectModel;

namespace PrestaITESHU.Pages;

public partial class UsuariosPage : ContentPage
{
    private UsuariosManager usuariosManager;
    private LaboratoriosManager laboratoriosManager;

    public ObservableCollection<UsuarioViewModel> Usuarios { get; set; } = new();
    private List<UsuarioViewModel> listaOriginal = new();

    public ObservableCollection<Laboratorios> LaboratoriosLista { get; set; } = new();
    public Laboratorios LaboratorioSeleccionado { get; set; }

    public ObservableCollection<string> RolesLista { get; set; } = new()
    {
        "Alumno", "Docente", "Administrador"
    };
    public string RolSeleccionado { get; set; }

    public bool MostrarFormulario { get; set; } = false;
    public bool IsLoading { get; set; }

    public string Nombre { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public string Correo { get; set; }
    public string Contrasena { get; set; }
    public string Matricula { get; set; }
    public string Carrera { get; set; }
    public string Semestre { get; set; }
    public bool Activo { get; set; } = true;

    private Usuarios usuarioSeleccionado;

    public UsuariosPage()
    {
        InitializeComponent();
        BindingContext = this;
        usuariosManager = FabricManager.UsuariosManager;
        laboratoriosManager = FabricManager.LaboratoriosManager;
        CargarLaboratorios();
        CargarUsuarios();
    }

    private async void CargarLaboratorios()
    {
        try
        {
            var lista = await laboratoriosManager.ObtenerTodos();
            LaboratoriosLista.Clear();
            if (lista != null)
                foreach (var item in lista)
                    LaboratoriosLista.Add(item);

            OnPropertyChanged(nameof(LaboratoriosLista));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void CargarUsuarios()
    {
        try
        {
            IsLoading = true;
            OnPropertyChanged(nameof(IsLoading));

            var listaLabs = await laboratoriosManager.ObtenerTodos();
            var lista = await usuariosManager.ObtenerTodos();

            Usuarios.Clear();
            listaOriginal.Clear();

            if (lista != null)
            {
                foreach (var item in lista)
                {
                    var lab = listaLabs?.FirstOrDefault(l => l.IdLaboratorio == item.IdLaboratorio);

                    var vm = new UsuarioViewModel
                    {
                        IdUsuario = item.IdUsuario,
                        Nombre = item.Nombre,
                        ApellidoPaterno = item.ApellidoPaterno,
                        ApellidoMaterno = item.ApellidoMaterno,
                        Correo = item.Correo,
                        Contrasena = item.Contrasena,
                        Matricula = item.Matricula,
                        Carrera = item.Carrera,
                        Semestre = item.Semestre,
                        Rol = item.Rol,
                        Activo = item.Activo,
                        IdLaboratorio = item.IdLaboratorio,
                        NombreLaboratorio = lab?.Nombre ?? "Sin laboratorio"
                    };

                    Usuarios.Add(vm);
                    listaOriginal.Add(vm);
                }
            }
            else
            {
                await DisplayAlert("Aviso", usuariosManager.Error, "OK");
            }

            OnPropertyChanged(nameof(Usuarios));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    private void LimpiarFormulario()
    {
        Nombre = string.Empty;
        ApellidoPaterno = string.Empty;
        ApellidoMaterno = string.Empty;
        Correo = string.Empty;
        Contrasena = string.Empty;
        Matricula = string.Empty;
        Carrera = string.Empty;
        Semestre = string.Empty;
        RolSeleccionado = null;
        LaboratorioSeleccionado = null;
        Activo = true;
        usuarioSeleccionado = null;

        OnPropertyChanged(nameof(Nombre));
        OnPropertyChanged(nameof(ApellidoPaterno));
        OnPropertyChanged(nameof(ApellidoMaterno));
        OnPropertyChanged(nameof(Correo));
        OnPropertyChanged(nameof(Contrasena));
        OnPropertyChanged(nameof(Matricula));
        OnPropertyChanged(nameof(Carrera));
        OnPropertyChanged(nameof(Semestre));
        OnPropertyChanged(nameof(RolSeleccionado));
        OnPropertyChanged(nameof(LaboratorioSeleccionado));
        OnPropertyChanged(nameof(Activo));
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
            IsLoading = true;
            OnPropertyChanged(nameof(IsLoading));

            Usuarios usuario;

            if (usuarioSeleccionado == null)
            {
                usuario = new Usuarios
                {
                    Nombre = Nombre,
                    ApellidoPaterno = ApellidoPaterno,
                    ApellidoMaterno = ApellidoMaterno,
                    Correo = Correo,
                    Contrasena = Contrasena,
                    Matricula = Matricula,
                    Carrera = Carrera,
                    Semestre = Semestre,
                    Rol = RolSeleccionado,
                    IdLaboratorio = LaboratorioSeleccionado.IdLaboratorio,
                    Activo = Activo,
                    FechaAlta = DateTime.Now,
                    UsuarioAlta = Params.UsuarioConectado,
                    FechaMod = null,
                    UsuarioMod = null
                };

                var resultado = await usuariosManager.Agregar(usuario);

                if (resultado != null)
                    await DisplayAlert("Correcto", "Usuario agregado correctamente", "OK");
                else
                    await DisplayAlert("Error", usuariosManager.Error, "OK");
            }
            else
            {
                usuario = usuarioSeleccionado;
                usuario.Nombre = Nombre;
                usuario.ApellidoPaterno = ApellidoPaterno;
                usuario.ApellidoMaterno = ApellidoMaterno;
                usuario.Correo = Correo;
                usuario.Contrasena = Contrasena;
                usuario.Matricula = Matricula;
                usuario.Carrera = Carrera;
                usuario.Semestre = Semestre;
                usuario.Rol = RolSeleccionado;
                usuario.IdLaboratorio = LaboratorioSeleccionado.IdLaboratorio;
                usuario.Activo = Activo;
                usuario.FechaMod = DateTime.Now;
                usuario.UsuarioMod = Params.UsuarioConectado;

                if (usuario.FechaAlta == DateTime.MinValue)
                    usuario.FechaAlta = DateTime.Now;
                if (string.IsNullOrEmpty(usuario.UsuarioAlta))
                    usuario.UsuarioAlta = Params.UsuarioConectado;

                var resultado = await usuariosManager.Modificar(usuario);

                if (resultado != null)
                    await DisplayAlert("Correcto", "Usuario actualizado correctamente", "OK");
                else
                    await DisplayAlert("Error", usuariosManager.Error, "OK");
            }

            LimpiarFormulario();
            MostrarFormulario = false;
            OnPropertyChanged(nameof(MostrarFormulario));
            CargarUsuarios();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    private void OnEditarClicked(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        var vm = btn.BindingContext as UsuarioViewModel;

        usuarioSeleccionado = new Usuarios
        {
            IdUsuario = vm.IdUsuario,
            Nombre = vm.Nombre,
            ApellidoPaterno = vm.ApellidoPaterno,
            ApellidoMaterno = vm.ApellidoMaterno,
            Correo = vm.Correo,
            Contrasena = vm.Contrasena,
            Matricula = vm.Matricula,
            Carrera = vm.Carrera,
            Semestre = vm.Semestre,
            Rol = vm.Rol,
            Activo = vm.Activo,
            IdLaboratorio = vm.IdLaboratorio
        };

        Nombre = vm.Nombre;
        ApellidoPaterno = vm.ApellidoPaterno;
        ApellidoMaterno = vm.ApellidoMaterno;
        Correo = vm.Correo;
        Contrasena = usuarioSeleccionado.Contrasena;
        Matricula = vm.Matricula;
        Carrera = vm.Carrera;
        Semestre = vm.Semestre;
        RolSeleccionado = vm.Rol;
        LaboratorioSeleccionado = LaboratoriosLista.FirstOrDefault(l => l.IdLaboratorio == vm.IdLaboratorio);
        Activo = vm.Activo;

        OnPropertyChanged(nameof(Nombre));
        OnPropertyChanged(nameof(ApellidoPaterno));
        OnPropertyChanged(nameof(ApellidoMaterno));
        OnPropertyChanged(nameof(Correo));
        OnPropertyChanged(nameof(Contrasena));
        OnPropertyChanged(nameof(Matricula));
        OnPropertyChanged(nameof(Carrera));
        OnPropertyChanged(nameof(Semestre));
        OnPropertyChanged(nameof(RolSeleccionado));
        OnPropertyChanged(nameof(LaboratorioSeleccionado));
        OnPropertyChanged(nameof(Activo));

        MostrarFormulario = true;
        OnPropertyChanged(nameof(MostrarFormulario));
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        var vm = btn.BindingContext as UsuarioViewModel;

        bool respuesta = await DisplayAlert("Confirmar", $"¿Eliminar a {vm.Nombre}?", "Sí", "No");

        if (respuesta)
        {
            var eliminado = await usuariosManager.Eliminar(vm.IdUsuario.ToString());

            if (eliminado)
            {
                await DisplayAlert("Correcto", "Eliminado", "OK");
                CargarUsuarios();
            }
            else
            {
                await DisplayAlert("Error", usuariosManager.Error, "OK");
            }
        }
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        LimpiarFormulario();
        MostrarFormulario = false;
        OnPropertyChanged(nameof(MostrarFormulario));
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuAdmin());
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string texto = e.NewTextValue.ToLower();

        var filtrados = listaOriginal
        .Where(x =>

            x.Nombre?.ToLower().Contains(texto) == true ||

            x.ApellidoPaterno?.ToLower().Contains(texto) == true ||

            x.ApellidoMaterno?.ToLower().Contains(texto) == true ||

            x.Carrera?.ToLower().Contains(texto) == true ||

            x.Semestre?.ToLower().Contains(texto) == true ||

            x.Matricula?.ToLower().Contains(texto) == true ||

            x.Rol?.ToLower().Contains(texto) == true ||

            x.NombreLaboratorio?.ToLower().Contains(texto) == true

        ).ToList();

        Usuarios.Clear();

        foreach (var item in filtrados)
        {
            Usuarios.Add(item);
        }
    }
}