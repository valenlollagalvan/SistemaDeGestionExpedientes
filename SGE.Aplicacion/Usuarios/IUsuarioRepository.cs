using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public interface IUsuarioRepository
{
    void Agregar(Usuario usuario);
    Usuario? ObtenerPorId(Guid id);
    Usuario? ObtenerPorCorreoElectronico(string correoElectronico);
    IEnumerable<Usuario> ObtenerTodos();
    void Eliminar(Guid id);
}
