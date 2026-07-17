using System;

namespace SGE.Aplicacion;

public interface IActualizacionEstadoExpedienteService
{
    void Actualizar(Guid expedienteId, Guid idUsuario);
}
