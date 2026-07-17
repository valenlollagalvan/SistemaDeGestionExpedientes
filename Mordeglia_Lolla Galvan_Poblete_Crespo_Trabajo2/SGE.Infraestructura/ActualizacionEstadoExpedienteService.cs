using SGE.Aplicacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;

namespace SGE.Infraestructura;

public class ActualizacionEstadoExpedienteService(
    IExpedienteRepository expedienteRepository,
    ITramiteRepository tramiteRepository) : IActualizacionEstadoExpedienteService
{
    public void Actualizar(Guid expedienteId, Guid idUsuario)
    {
        var expediente = expedienteRepository.ObtenerExpedientePorId(expedienteId);
        if (expediente is null)
        {
            throw new EntidadNoEncontradaException("Expediente no encontrado.");
        }

        var ultimoTramite = tramiteRepository
            .ObtenerTramitesPorExpediente(expedienteId)
            .OrderByDescending(tramite => tramite.FechaUltimaModificacion)
            .ThenByDescending(tramite => tramite.FechaCreacion)
            .FirstOrDefault();

        expediente.ActualizarEstadoPorTramite(ultimoTramite?.Etiqueta, idUsuario);
    }
}
