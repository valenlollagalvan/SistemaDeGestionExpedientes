using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SGE.Dominio.Usuarios;
using SGE.Aplicacion.Usuarios;

namespace SGE.WebAPI.Servicios;

public class ServicioDeToken(IConfiguration config) : ITokenService
{
    public string GenerarToken(Usuario usuario)
    {
        var claims = new[] {
            new Claim("ID", usuario.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(config.GetValue("Jwt:ExpirationMinutes", 60)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
