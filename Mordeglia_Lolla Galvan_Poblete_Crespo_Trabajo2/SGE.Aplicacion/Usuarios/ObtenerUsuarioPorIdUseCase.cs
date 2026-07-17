using SGE.Aplicacion.Comun;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public class ObtenerUsuarioPorIdUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ObtenerUsuarioPorIdUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public ObtenerUsuarioPorIdResponse Ejecutar(ObtenerUsuarioPorIdRequest request)
    {
        Usuario? usuario = _usuarioRepository.ObtenerPorId(request.IdUsuario);
        if (usuario is null)
        {
            throw new EntidadNoEncontradaException("No se encontró el usuario con el ID proporcionado.");
        }

        return new ObtenerUsuarioPorIdResponse(new UsuarioDTO(
            usuario.Id,
            usuario.Nombre,
            usuario.CorreoElectronico,
            usuario.EsAdministrador,
            usuario.Permisos));
    }
}
