using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio;
using SGE.Dominio.Enums;
using SGE.Dominio.Comun;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class ActualizarEstadoExpedienteUseCase
{
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly IExpedienteRepository _repository;
    private readonly IAutorizacionService _autorizacionService;
    
    public ActualizarEstadoExpedienteUseCase(IExpedienteRepository repository, IAutorizacionService autorizacionService, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _unidadDeTrabajo = unidadDeTrabajo;
        _repository = repository;
        _autorizacionService = autorizacionService;
    }
    
    public void Ejecutar(ModificarEstadoExpedienteRequest request)
    {
        if(!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permiso para modificar el estado del expediente.");
        }
        
        var expediente = _repository.ObtenerExpedientePorId(request.IdExpediente);
        if (expediente == null)
        {
            throw new EntidadNoEncontradaException("Expediente no encontrado.");
        }
        
        if (!Enum.TryParse<Estado>(request.EstadoNuevo, true, out var nuevoEstadoParseado))
        {
            throw new DominioException("El estado ingresado no es válido para el negocio.");
        }
        
        expediente.CambiarEstadoManual(nuevoEstadoParseado, request.IdUsuario);
        _unidadDeTrabajo.Guardar();
    }
}