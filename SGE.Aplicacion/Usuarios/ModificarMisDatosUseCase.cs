using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Comun;

namespace SGE.Aplicacion.Usuarios;

public class ModificarMisDatosUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHashContrasenaService _hashContrasenaService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarMisDatosUseCase(
        IUsuarioRepository usuarioRepository,
        IHashContrasenaService hashContrasenaService,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepository = usuarioRepository;
        _hashContrasenaService = hashContrasenaService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public ModificarMisDatosResponse Ejecutar(ModificarMisDatosRequest request)
    {
        if (request.IdUsuarioToken != request.IdUsuarioObjetivo)
        {
            throw new AutorizacionException("El usuario autenticado no puede modificar los datos de otro usuario.");
        }

        var usuario = _usuarioRepository.ObtenerPorId(request.IdUsuarioObjetivo);
        if (usuario is null)
        {
            throw new EntidadNoEncontradaException("Usuario no encontrado.");
        }

        var usuarioConMismoCorreo = _usuarioRepository.ObtenerPorCorreoElectronico(request.CorreoElectronico);
        if (usuarioConMismoCorreo != null && usuarioConMismoCorreo.Id != usuario.Id)
        {
            throw new DominioException("Ya existe un usuario registrado con ese correo electrónico.");
        }

        usuario.ModificarDatosPersonales(request.Nombre, request.CorreoElectronico);

        if (!string.IsNullOrWhiteSpace(request.NuevaContrasena))
        {
            var nuevaContrasenaHash = _hashContrasenaService.Hashear(request.NuevaContrasena);
            usuario.CambiarContrasenaHash(nuevaContrasenaHash);
        }

        _unidadDeTrabajo.Guardar();

        return new ModificarMisDatosResponse(usuario.Id);
    }
}
