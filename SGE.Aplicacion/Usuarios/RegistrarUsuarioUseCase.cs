using SGE.Aplicacion.Comun;
using SGE.Dominio.Comun;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public class RegistrarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHashContrasenaService _hashContrasenaService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public RegistrarUsuarioUseCase(
        IUsuarioRepository usuarioRepository,
        IHashContrasenaService hashContrasenaService,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepository = usuarioRepository;
        _hashContrasenaService = hashContrasenaService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public RegistrarUsuarioResponse Ejecutar(RegistrarUsuarioRequest request)
    {
        var usuarioExistente = _usuarioRepository.ObtenerPorCorreoElectronico(request.CorreoElectronico);
        if (usuarioExistente != null)
        {
            throw new DominioException("Ya existe un usuario registrado con ese correo electrónico.");
        }

        var contrasenaHash = _hashContrasenaService.Hashear(request.Contrasena);

        var usuario = new Usuario(
            request.Nombre,
            request.CorreoElectronico,
            contrasenaHash,
            false);

        _usuarioRepository.Agregar(usuario);
        _unidadDeTrabajo.Guardar();

        return new RegistrarUsuarioResponse(usuario.Id);
    }
}
