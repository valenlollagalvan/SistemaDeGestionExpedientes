using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Usuarios;
using SGE.Infraestructura.Autorizaciones;
using SGE.Infraestructura.Datos;
using SGE.Infraestructura.Expedientes;
using SGE.Infraestructura.Expedientes.Usuarios;
using SGE.Infraestructura.HashContrasenas;
using SGE.Infraestructura.UDT;

namespace SGE.Infraestructura.Extensiones;

public static class InfraestructuraExtensions
{
    public static IServiceCollection AddInfraestructura(this IServiceCollection servicios, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SgeDb") ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'SgeDb' en la configuración.");

        servicios.AddDbContext<SgeContext>(opciones => opciones.UseSqlite(connectionString));
        servicios.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
        servicios.AddScoped<IExpedienteRepository, ExpedienteRepository>();
        servicios.AddScoped<ITramiteRepository, TramiteRepository>();
        servicios.AddScoped<IUsuarioRepository, UsuarioRepository>();
        servicios.AddScoped<IHashContrasenaService, HashContrasenaService>();
        servicios.AddScoped<IAutorizacionService, AutorizacionService>();
        servicios.AddScoped<IActualizacionEstadoExpedienteService, ActualizacionEstadoExpedienteService>();

        return servicios;
    }

}
