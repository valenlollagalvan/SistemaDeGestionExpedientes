namespace SGE.Aplicacion.Tramites;

public record AgregarTramiteRequest(
    Guid ExpedienteId,
    string? contenido,
    string? etiqueta,
    Guid IdUsuario
);
