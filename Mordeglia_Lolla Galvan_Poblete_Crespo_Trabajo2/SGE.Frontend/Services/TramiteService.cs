using SGE.Frontend.Models;

namespace SGE.Frontend.Services;

public class TramiteService(ApiClient api)
{
    public async Task<IReadOnlyList<TramiteDto>> ListarAsync(Guid expedienteId)
    {
        var response = await api.GetAsync<ListarTramitesResponse>($"/api/tramites/{expedienteId}");
        return response?.Tramites.ToList() ?? [];
    }

    public async Task<Guid> CrearAsync(Guid expedienteId, string contenido, string etiqueta)
    {
        var response = await api.PostAsync<AgregarTramiteResponse>("/api/tramites/", new AgregarTramiteRequest(expedienteId, contenido, etiqueta, Guid.Empty));
        return response?.Id ?? Guid.Empty;
    }

    public Task ModificarAsync(Guid id, string contenido, string etiqueta)
    {
        return api.PutAsync($"/api/tramites/{id}", new ModificarTramiteRequest(id, contenido, etiqueta, Guid.Empty));
    }

    public Task EliminarAsync(Guid id)
    {
        return api.DeleteAsync($"/api/tramites/{id}");
    }
}
