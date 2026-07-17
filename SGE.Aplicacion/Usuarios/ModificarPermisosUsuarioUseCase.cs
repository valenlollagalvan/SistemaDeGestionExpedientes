using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;

namespace SGE.Aplicacion.Usuarios;

public class ModificarPermisosUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarPermisosUsuarioUseCase(IUsuarioRepository usuarioRepository, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepository = usuarioRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public ModificarPermisosUsuarioResponse Ejecutar(ModificarPermisosUsuarioRequest request)
    {
        var usuarioEjecutor = _usuarioRepository.ObtenerPorId(request.IdUsuarioEjecutor);
        if (usuarioEjecutor is null)
        {
            throw new EntidadNoEncontradaException("Usuario ejecutor no encontrado.");
        }

        if (!usuarioEjecutor.EsAdministrador)
        {
            throw new AutorizacionException("El usuario no tiene permisos de administrador.");
        }

        var usuario = _usuarioRepository.ObtenerPorId(request.IdUsuarioObjetivo);
        if (usuario is null)
        {
            throw new EntidadNoEncontradaException("Usuario no encontrado.");
        }

        if (usuario.Id == usuarioEjecutor.Id && request.PermisosARemover.Contains(Dominio.Enums.Permiso.ExpedienteBaja))
        {
            throw new AutorizacionException("No puede revocar permisos críticos de su propia cuenta.");
        }

        foreach (var permiso in request.PermisosAAsignar)
        {
            usuario.AsignarPermiso(permiso);
        }

        foreach (var permiso in request.PermisosARemover)
        {
            usuario.RevocarPermiso(permiso);
        }

        _unidadDeTrabajo.Guardar();

        return new ModificarPermisosUsuarioResponse(usuario.Id);
    }
}
