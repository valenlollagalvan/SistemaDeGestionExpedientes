using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Usuarios;

public class ListarUsuariosUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ListarUsuariosUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public ListarUsuariosResponse Ejecutar(ListarUsuariosRequest request)
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

        var usuarios = _usuarioRepository
            .ObtenerTodos()
            .Select(usuario => new UsuarioDTO(
                usuario.Id,
                usuario.Nombre,
                usuario.CorreoElectronico,
                usuario.EsAdministrador,
                usuario.Permisos));

        return new ListarUsuariosResponse(usuarios);
    }
}
