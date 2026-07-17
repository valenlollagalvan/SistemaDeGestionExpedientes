using System;
//este nuget lo tienen que desgargar desde apliacion en la consola para que funcione el AddScoped
//dotnet add package Microsoft.Extensions.DependencyInjection.Abstractions
using Microsoft.Extensions.DependencyInjection;
using SGE.Aplicacion.Expedientes; 
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Usuarios;

namespace SGE.Aplicacion;

public static class Extensiones
{
    public static IServiceCollection AddAplicacion(this IServiceCollection servicios)
    {
        // Casos de Uso de Expedientes
        servicios.AddScoped<AltaExpedienteUseCase>();
        servicios.AddScoped<ActualizarCaratulaExpedienteUseCase>();
        servicios.AddScoped<ActualizarEstadoExpedienteUseCase>();
        servicios.AddScoped<EliminarExpedienteUseCase>();
        servicios.AddScoped<ObtenerExpedientePorIdUseCase>();
        servicios.AddScoped<ObtenerTodosExpedientesUseCase>();

        // Casos de Uso de Usuarios y Seguridad
        servicios.AddScoped<RegistrarUsuarioUseCase>();
        servicios.AddScoped<LoginUseCase>();
        servicios.AddScoped<ListarUsuariosUseCase>();
        servicios.AddScoped<ObtenerUsuarioPorIdUseCase>();
        servicios.AddScoped<EliminarUsuarioUseCase>();
        servicios.AddScoped<ModificarPermisosUsuarioUseCase>();
        servicios.AddScoped<ModificarMisDatosUseCase>();

        // Casos de Uso de Trámites
        servicios.AddScoped<AgregarTramiteUseCase>();
        servicios.AddScoped<ModificarTramiteUseCase>();
        servicios.AddScoped<EliminarTramiteUseCase>();
        servicios.AddScoped<ListarTramitesUseCase>();
        
        return servicios;
    }
}
