using SGE.Frontend.Models;

namespace SGE.Frontend.Services;

public class UsuarioService(ApiClient api)
{
    public async Task<IReadOnlyList<UsuarioDto>> ListarAsync()
    {
        var response = await api.GetAsync<ListarUsuariosResponse>("/api/usuarios/");
        return response?.Usuarios.ToList() ?? [];
    }

    public async Task<UsuarioDto?> ObtenerAsync(Guid id)
    {
        var response = await api.GetAsync<ObtenerUsuarioPorIdResponse>($"/api/usuarios/{id}");
        return response?.Usuario;
    }

    public Task ModificarMisDatosAsync(string nombre, string correoElectronico, string? nuevaContrasena)
    {
        return api.PutAsync("/api/usuarios/mis-datos", new ModificarMisDatosRequest(Guid.Empty, Guid.Empty, nombre, correoElectronico, string.IsNullOrWhiteSpace(nuevaContrasena) ? null : nuevaContrasena));
    }

    public Task ModificarPermisosAsync(Guid id, IEnumerable<int> permisosActuales, IEnumerable<int> permisosSeleccionados)
    {
        var actuales = permisosActuales.ToHashSet();
        var seleccionados = permisosSeleccionados.ToHashSet();
        var asignar = seleccionados.Except(actuales).ToArray();
        var remover = actuales.Except(seleccionados).ToArray();

        return api.PutAsync($"/api/usuarios/{id}/permisos", new ModificarPermisosUsuarioRequest(id, Guid.Empty, asignar, remover));
    }

    public Task EliminarAsync(Guid id)
    {
        return api.DeleteAsync($"/api/usuarios/{id}");
    }
}
