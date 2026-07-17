using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Enums;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public class EliminarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly IAutorizacionService _autorizacionService;
    private readonly IActualizacionEstadoExpedienteService _actualizacionEstadoExpedienteService;

    public EliminarTramiteUseCase(
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

    public EliminarTramiteResponse Ejecutar(EliminarTramiteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteBaja)) {
            throw new AutorizacionException("El usuario no posee permiso para eliminar el tramite.");
        }
        var tramite =  _tramiteRepository.ObtenerTramiteId(request.Id);
        if (tramite is null) {
            throw new EntidadNoEncontradaException($"No se encontro el tramite con id {request.Id}");
        }
        Guid expedienteId = tramite.ExpedienteId;
        _tramiteRepository.EliminarTramite(request.Id);
        _actualizacionEstadoExpedienteService.Actualizar(expedienteId, request.IdUsuario);
        _unidadDeTrabajo.Guardar();
        return new EliminarTramiteResponse(tramite.Id);
    }
}
