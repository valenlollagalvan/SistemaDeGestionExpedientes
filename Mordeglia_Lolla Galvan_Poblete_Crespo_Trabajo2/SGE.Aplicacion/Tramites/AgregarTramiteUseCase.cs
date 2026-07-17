using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Tramites;
using SGE.Dominio.Enums;
using SGE.Dominio.ValueObjects;

namespace SGE.Aplicacion.Tramites;

public class AgregarTramiteUseCase 
{
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly IAutorizacionService _autorizacionService;
    private readonly IActualizacionEstadoExpedienteService _actualizacionEstadoExpedienteService;

    public AgregarTramiteUseCase(
        ITramiteRepository tramiteRepository,
        IUnidadDeTrabajo unidadDeTrabajo,
        IActualizacionEstadoExpedienteService actualizacionEstadoExpedienteService,
        IAutorizacionService autorizacionService)
    {
        _tramiteRepository = tramiteRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
        _actualizacionEstadoExpedienteService = actualizacionEstadoExpedienteService;
        _autorizacionService = autorizacionService;
    }

    public AgregarTramiteResponse Ejecutar (AgregarTramiteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteAlta))
        {
            throw new AutorizacionException("El usuario no posee permiso para dar de alta un tramite.");
        }
        
        Tramite tramite = new Tramite(
            request.ExpedienteId,
            Enum.Parse<EtiquetaTramite>(request.etiqueta ?? "", true),
            new ContenidoTramite(request.contenido ?? ""),
            request.IdUsuario
        );
        
        _tramiteRepository.AgregarTramite(tramite);
        _actualizacionEstadoExpedienteService.Actualizar(tramite.ExpedienteId, request.IdUsuario);
        _unidadDeTrabajo.Guardar();

        return new AgregarTramiteResponse(tramite.Id);
    }
}
