using SGE.Aplicacion.Usuarios; // Asegurate que apunte a tu IUsuarioRepository
using SGE.Dominio.Usuarios;
using SGE.Infraestructura.Datos;

namespace SGE.Infraestructura.Expedientes.Usuarios;

public sealed class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(SgeContext context) : base(context)
    {
    }

    public Usuario? ObtenerPorCorreoElectronico(string correoElectronico)
    {
        var correoNormalizado = correoElectronico.Trim().ToLowerInvariant();
        return _dbSet.SingleOrDefault(usuario => usuario.CorreoElectronico == correoNormalizado);
    }
}
