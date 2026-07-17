using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Enums;
using SGE.Dominio.Comun;
using SGE.Dominio.ValueObjects;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class AltaExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly IAutorizacionService _autorizacionService;

    public AltaExpedienteUseCase(IExpedienteRepository expedienteRepository, IUnidadDeTrabajo unidadDeTrabajo, IAutorizacionService autorizacionService)
    {
        _expedienteRepository = expedienteRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
        _autorizacionService = autorizacionService;
    }

    public AltaExpedienteResponse Ejecutar(AltaExpedienteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteAlta))
        {
            throw new AutorizacionException("El usuario no tiene permiso para crear expedientes.");
        }
        var caratula = new Caratula(request.Caratula);
        var nuevoExpediente = new Expediente(caratula, request.IdUsuario);
        _expedienteRepository.Agregar(nuevoExpediente);
        _unidadDeTrabajo.Guardar();
        return new AltaExpedienteResponse(nuevoExpediente.Id);
    }
}
