namespace SGE.Aplicacion.Usuarios;

public record class ModificarMisDatosRequest(
    Guid IdUsuarioToken,
    Guid IdUsuarioObjetivo,
    string Nombre,
    string CorreoElectronico,
    string? NuevaContrasena);
