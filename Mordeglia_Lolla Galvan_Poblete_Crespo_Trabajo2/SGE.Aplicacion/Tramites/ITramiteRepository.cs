using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Tramites;
public interface ITramiteRepository
{
    void AgregarTramite (Tramite tramite);

    Tramite? ObtenerTramiteId (Guid Id);
        
    void EliminarTramite(Guid Id);
    
    IEnumerable<Tramite> ObtenerTramitesPorExpediente(Guid ExpedienteId);

    void EliminarTramitePorExpediente(Guid ExpedienteId);

    IEnumerable<Tramite> ObtenerTodos();
}
