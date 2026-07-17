using SGE.Frontend.Models;

namespace SGE.Frontend.Services;

public class ExpedienteService(ApiClient api)
{
    public async Task<IReadOnlyList<ExpedienteDto>> ListarAsync()
    {
        var response = await api.GetAsync<ObtenerTodosExpedienteResponse>("/api/expedientes/");
        return response?.Expedientes.ToList() ?? [];
    }

    public async Task<ExpedienteDto?> ObtenerAsync(Guid id)
    {
        var response = await api.GetAsync<ObtenerExpedientePorIdResponse>($"/api/expedientes/{id}");
        return response is null
            ? null
            : new ExpedienteDto(response.Id, response.Caratula, response.FechaCreacion, response.FechaUltimaModificacion, response.UsuarioUltimoCambio, response.Estado);
    }

    public async Task<Guid> CrearAsync(string caratula)
    {
        var response = await api.PostAsync<AltaExpedienteResponse>("/api/expedientes/", new AltaExpedienteRequest(caratula, Guid.Empty));
        return response?.IdExpediente ?? Guid.Empty;
    }

    public Task CambiarCaratulaAsync(Guid id, string caratula)
    {
        return api.PutAsync($"/api/expedientes/{id}/caratula", new ModificarCaratulaExpedienteRequest(id, Guid.Empty, caratula));
    }

    public Task CambiarEstadoAsync(Guid id, string estado)
    {
        return api.PutAsync($"/api/expedientes/{id}/estado", new ModificarEstadoExpedienteRequest(id, Guid.Empty, estado));
    }

    public Task EliminarAsync(Guid id)
    {
        return api.DeleteAsync($"/api/expedientes/{id}");
    }
}
