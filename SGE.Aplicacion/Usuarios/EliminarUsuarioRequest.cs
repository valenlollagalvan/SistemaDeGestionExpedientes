namespace SGE.Aplicacion.Usuarios;

public record class EliminarUsuarioRequest(Guid IdUsuarioAEliminar, Guid IdUsuarioEjecutor);
