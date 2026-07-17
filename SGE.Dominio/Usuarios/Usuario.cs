using SGE.Dominio.Enums;
using SGE.Dominio.Comun;

namespace SGE.Dominio.Usuarios;

public class Usuario
{
    private readonly HashSet<Permiso> _permisos = [];

    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = null!;
    public string CorreoElectronico { get; private set; } = null!;
    public string ContrasenaHash { get; private set; } = null!;
    public bool EsAdministrador { get; private set; }
    public IReadOnlyCollection<Permiso> Permisos => _permisos.ToArray();

    public Usuario(string nombre, string correoElectronico, string contrasenaHash, bool esAdministrador = false)
    {
        ValidarNombre(nombre);
        ValidarCorreoElectronico(correoElectronico);
        ValidarContrasenaHash(contrasenaHash);

        Id = Guid.NewGuid();
        Nombre = nombre.Trim();
        CorreoElectronico = correoElectronico.Trim().ToLowerInvariant();
        ContrasenaHash = contrasenaHash.Trim();
        EsAdministrador = esAdministrador;
    }

    private Usuario()
    {
    }

    public void ModificarDatosPersonales(string nombre, string correoElectronico)
    {
        ValidarNombre(nombre);
        ValidarCorreoElectronico(correoElectronico);

        Nombre = nombre.Trim();
        CorreoElectronico = correoElectronico.Trim().ToLowerInvariant();
    }

    public void CambiarContrasenaHash(string nuevaContrasenaHash)
    {
        ValidarContrasenaHash(nuevaContrasenaHash);
        ContrasenaHash = nuevaContrasenaHash.Trim();
    }

    public void AsignarPermiso(Permiso permiso)
    {
        ValidarPermiso(permiso);
        _permisos.Add(permiso);
    }

    public void RevocarPermiso(Permiso permiso)
    {
        ValidarPermiso(permiso);
        _permisos.Remove(permiso);
    }

    public bool PoseePermiso(Permiso permiso)
    {
        ValidarPermiso(permiso);
        return _permisos.Contains(permiso);
    }

    private static void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new DominioException("El nombre del usuario es obligatorio.");
        }
    }

    private static void ValidarCorreoElectronico(string correoElectronico)
    {
        if (string.IsNullOrWhiteSpace(correoElectronico))
        {
            throw new DominioException("El correo electrónico es obligatorio.");
        }

        var correoNormalizado = correoElectronico.Trim();
        if (!correoNormalizado.Contains('@') || correoNormalizado.StartsWith('@') || correoNormalizado.EndsWith('@'))
        {
            throw new DominioException("El correo electrónico no tiene un formato válido.");
        }
    }

    private static void ValidarContrasenaHash(string contrasenaHash)
    {
        if (string.IsNullOrWhiteSpace(contrasenaHash))
        {
            throw new DominioException("La contraseña cifrada es obligatoria.");
        }
    }

    private static void ValidarPermiso(Permiso permiso)
    {
        if (!Enum.IsDefined(typeof(Permiso), permiso))
        {
            throw new DominioException("Permiso inválido.");
        }
    }
}
