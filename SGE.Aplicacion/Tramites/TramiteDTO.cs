namespace SGE.Aplicacion.Tramites;

public record TramiteDTO (
    Guid Id,
    Guid ExpedienteId,
    string? Contenido,
    string? Etiqueta,
    DateTime FechaCreacion,
    DateTime FechaUltimaModificacion,
    Guid UsuarioUltimoCambio
);
