namespace SGE.Aplicacion.Tramites;

public record ModificarTramiteRequest(
    Guid Id,
    string? Contenido,
    string? Etiqueta,
    Guid IdUsuario
);