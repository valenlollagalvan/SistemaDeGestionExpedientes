using System.Security.Cryptography;
using SGE.Aplicacion.Usuarios;

namespace SGE.Infraestructura.HashContrasenas;

public class HashContrasenaService : IHashContrasenaService
{
    private const string Prefijo = "PBKDF2-SHA256";
    private const int Iteraciones = 120_000;
    private const int TamanoSalt = 16;
    private const int TamanoHash = 32;

    public string Hashear(string contrasenaEnTextoPlano)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contrasenaEnTextoPlano);
        var salt = RandomNumberGenerator.GetBytes(TamanoSalt);
        var hash = Rfc2898DeriveBytes.Pbkdf2(contrasenaEnTextoPlano, salt, Iteraciones, HashAlgorithmName.SHA256, TamanoHash);
        return $"{Prefijo}${Iteraciones}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public bool Verificar(string contrasenaEnTextoPlano, string hashAlmacenado)
    {
        if (string.IsNullOrWhiteSpace(contrasenaEnTextoPlano) || string.IsNullOrWhiteSpace(hashAlmacenado)) return false;

        if (hashAlmacenado.StartsWith($"{Prefijo}$", StringComparison.Ordinal))
        {
            var partes = hashAlmacenado.Split('$');
            if (partes.Length != 4 || !int.TryParse(partes[1], out var iteraciones)) return false;
            try
            {
                var salt = Convert.FromBase64String(partes[2]);
                var esperado = Convert.FromBase64String(partes[3]);
                var actual = Rfc2898DeriveBytes.Pbkdf2(contrasenaEnTextoPlano, salt, iteraciones, HashAlgorithmName.SHA256, esperado.Length);
                return CryptographicOperations.FixedTimeEquals(actual, esperado);
            }
            catch (FormatException) { return false; }
        }

        // Compatibilidad temporal con hashes SHA-256 creados por versiones anteriores.
        var legacy = Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(contrasenaEnTextoPlano))).ToLowerInvariant();
        return CryptographicOperations.FixedTimeEquals(System.Text.Encoding.UTF8.GetBytes(legacy), System.Text.Encoding.UTF8.GetBytes(hashAlmacenado));
    }

    public bool RequiereActualizacion(string hashAlmacenado) => !hashAlmacenado.StartsWith($"{Prefijo}$", StringComparison.Ordinal);
}
