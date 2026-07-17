using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Enums;
using SGE.Dominio.ValueObjects;

namespace SGE.Aplicacion.Tramites;

public class ModificarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly IAutorizacionService _autorizacionService; 
    private readonly IActualizacionEstadoExpedienteService _actualizacionEstadoExpedienteService;

    public ModificarTramiteUseCase(
        ITramiteRepository tramiteRepository,
        IUnidadDeTrabajo unidadDeTrabajo,
        IAutorizacionService autorizacionService,
        IActualizacionEstadoExpedienteService actualizacionEstadoExpedienteService) {
        _tramiteRepository = tramiteRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
        _autorizacionService = autorizacionService;
        _actualizacionEstadoExpedienteService = actualizacionEstadoExpedienteService;
    }
    public ModificarTramiteResponse Ejecutar (ModificarTramiteRequest request) {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteModificacion)) {
            throw new AutorizacionException("El usuario no posee permiso para modificar el tramite.");
        }
        var tramite = _tramiteRepository.ObtenerTramiteId(request.Id);
        if (tramite is null) {
            throw new EntidadNoEncontradaException($"No se encontro el tramite con ID: {request.Id}");
        }
        
        if (request.Contenido != null) {
            tramite.ActualizarContenido(new ContenidoTramite(request.Contenido), request.IdUsuario);
        }
        if (request.Etiqueta != null) {
            tramite.CambiarEtiqueta(Enum.Parse<EtiquetaTramite>(request.Etiqueta, true), request.IdUsuario);
        }
        
        _actualizacionEstadoExpedienteService.Actualizar(tramite.ExpedienteId, request.IdUsuario);
        _unidadDeTrabajo.Guardar();
        return new ModificarTramiteResponse(tramite.Id);       
    }
}
