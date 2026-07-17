using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public class ListarTramitesUseCase 
{
    private readonly ITramiteRepository _tramiteRepository;

    public ListarTramitesUseCase (ITramiteRepository tramiteRepository)
    {
        _tramiteRepository = tramiteRepository;
    }

    public ListarTramitesResponse Ejecutar (ListarTramiteRequest request)
    {
        IEnumerable<Tramite> tramites = _tramiteRepository.ObtenerTramitesPorExpediente(request.ExpedienteId);

        List<TramiteDTO> tramiteDTO = new();
        
        foreach (Tramite tramite in tramites)
        {
            TramiteDTO dto = new(
                tramite.Id,
                tramite.ExpedienteId,
                tramite.Contenido.Texto,
                tramite.Etiqueta.ToString(),
                tramite.FechaCreacion,
                tramite.FechaUltimaModificacion,
                tramite.UsuarioUltimoCambio
            );
            
            tramiteDTO.Add(dto);
        }
        
        return new ListarTramitesResponse (tramiteDTO);
    }
}
