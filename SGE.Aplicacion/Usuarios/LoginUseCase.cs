using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;

namespace SGE.Aplicacion.Usuarios;

public class LoginUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHashContrasenaService _hashContrasenaService;
    private readonly ITokenService _tokenService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public LoginUseCase(
        IUsuarioRepository usuarioRepository,
        IHashContrasenaService hashContrasenaService,
        ITokenService tokenService,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepository = usuarioRepository;
        _hashContrasenaService = hashContrasenaService;
        _tokenService = tokenService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public LoginResponse Ejecutar(LoginRequest request)
    {
        var usuario = _usuarioRepository.ObtenerPorCorreoElectronico(request.CorreoElectronico);
        if (usuario is null)
        {
            throw new AutorizacionException("Credenciales inválidas.");
        }

        if (!_hashContrasenaService.Verificar(request.Contrasena, usuario.ContrasenaHash))
        {
            throw new AutorizacionException("Credenciales inválidas.");
        }

        if (_hashContrasenaService.RequiereActualizacion(usuario.ContrasenaHash))
        {
            usuario.CambiarContrasenaHash(_hashContrasenaService.Hashear(request.Contrasena));
            _unidadDeTrabajo.Guardar();
        }

        var token = _tokenService.GenerarToken(usuario);
        return new LoginResponse(token);
    }
}
