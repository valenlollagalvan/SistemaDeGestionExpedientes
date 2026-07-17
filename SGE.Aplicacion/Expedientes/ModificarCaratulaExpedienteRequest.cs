namespace SGE.Aplicacion.Expedientes;

public record class ModificarCaratulaExpedienteRequest(Guid IdExpediente, Guid IdUsuario, string NuevaCaratula);
