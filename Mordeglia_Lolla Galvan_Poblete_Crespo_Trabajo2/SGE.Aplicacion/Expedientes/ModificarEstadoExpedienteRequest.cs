
namespace SGE.Aplicacion.Expedientes;

public record class ModificarEstadoExpedienteRequest(Guid IdExpediente, Guid IdUsuario, string? EstadoNuevo);
