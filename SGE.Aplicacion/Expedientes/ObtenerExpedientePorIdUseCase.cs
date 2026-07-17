using System;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Comun;
namespace SGE.Aplicacion.Expedientes;

public class ObtenerExpedientePorIdUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly ITramiteRepository _tramiteRepository;

    public ObtenerExpedientePorIdUseCase(IExpedienteRepository expedienteRepository, ITramiteRepository tramiteRepository)
    {
        _expedienteRepository = expedienteRepository;
        _tramiteRepository = tramiteRepository;
    }
    public ObtenerExpedientePorIdResponse Ejecutar(Guid idExpediente)
    {
        var expediente = _expedienteRepository.ObtenerExpedientePorId(idExpediente);
        if (expediente == null)
        {
            throw new EntidadNoEncontradaException("No se encontró el expediente con el ID proporcionado.");
        }
        var tramites = _tramiteRepository.ObtenerTramitesPorExpediente(idExpediente);
        var tramitesDTO = tramites.Select(t => new TramiteDTO(
            t.Id,
            t.ExpedienteId,
            t.Contenido.ToString(),
            t.Etiqueta.ToString(),
            t.FechaCreacion,
            t.FechaUltimaModificacion,
            t.UsuarioUltimoCambio
        )).ToList();

        return new ObtenerExpedientePorIdResponse(
            expediente.Id,
            expediente.Caratula.Texto,
            expediente.FechaCreacion,
            expediente.FechaUltimaModificacion,
            expediente.UsuarioUltimoCambio,
            expediente.EstadoExpediente.ToString(),
            tramitesDTO
        );
    }
}
