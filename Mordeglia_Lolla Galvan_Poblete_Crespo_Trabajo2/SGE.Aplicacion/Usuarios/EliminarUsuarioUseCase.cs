using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;

namespace SGE.Aplicacion.Usuarios;

public class EliminarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public EliminarUsuarioUseCase(IUsuarioRepository usuarioRepository, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepository = usuarioRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public EliminarUsuarioResponse Ejecutar(EliminarUsuarioRequest request)
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

        var usuario = _usuarioRepository.ObtenerPorId(request.IdUsuarioAEliminar);
        if (usuario is null)
        {
            throw new EntidadNoEncontradaException("Usuario no encontrado.");
        }

        _usuarioRepository.Eliminar(request.IdUsuarioAEliminar);
        _unidadDeTrabajo.Guardar();

        return new EliminarUsuarioResponse(request.IdUsuarioAEliminar);
    }
}
