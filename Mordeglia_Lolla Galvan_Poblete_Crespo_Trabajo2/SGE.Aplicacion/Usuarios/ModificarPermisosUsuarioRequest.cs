using SGE.Dominio.Enums;

namespace SGE.Aplicacion.Usuarios;

public record class ModificarPermisosUsuarioRequest(
    Guid IdUsuarioObjetivo,
    Guid IdUsuarioEjecutor,
    IEnumerable<Permiso> PermisosAAsignar,
    IEnumerable<Permiso> PermisosARemover);
