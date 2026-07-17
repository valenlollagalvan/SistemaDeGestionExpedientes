namespace SGE.Aplicacion.Usuarios;

public interface IHashContrasenaService
{
    string Hashear(string contrasenaEnTextoPlano);
    bool Verificar(string contrasenaEnTextoPlano, string hashAlmacenado);
    bool RequiereActualizacion(string hashAlmacenado);
}
