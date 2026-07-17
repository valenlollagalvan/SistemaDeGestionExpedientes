using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SGE.WebAPI.Servicios;
using SGE.Aplicacion.Usuarios;

namespace SGE.WebAPI.Dependencias;

public static class InyeccionDeDependencias
{
    public static IServiceCollection AddAutenticacionJWT(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, ServicioDeToken>();

        var jwtKey = configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
        {
            throw new InvalidOperationException("Jwt:Key debe configurarse con al menos 32 caracteres mediante configuración segura.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opciones =>
            {
                opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    )
                };
            });

        services.AddAuthorization();
        
        return services;
    }
}
