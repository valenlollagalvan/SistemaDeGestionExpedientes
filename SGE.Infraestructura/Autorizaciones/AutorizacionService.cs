using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Enums;

namespace SGE.Infraestructura.Autorizaciones;

public class AutorizacionService(IUsuarioRepository usuarioRepository) : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        var usuario = usuarioRepository.ObtenerPorId(idUsuario);
        if (usuario is null)
        {
            return false;
        }

        if (usuario.EsAdministrador)
        {
            return true;
        }

        if (permiso == Permiso.TramiteBaja && usuario.PoseePermiso(Permiso.ExpedienteBaja))
        {
            return true;
        }
        return usuario.PoseePermiso(permiso);
    }
}
