using SGE.Aplicacion.Usuarios;

namespace SGE.WebAPI.Endpoints;

public static class AutorizacionEndpoints
{
    public static void MapAutorizacionEndpoints(this IEndpointRouteBuilder app)
    {
        var authApi = app.MapGroup("/api/auth").WithTags("Autenticación");

        authApi.MapPost("/login", (LoginRequest request, LoginUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        }).RequireRateLimiting("auth");
        
        authApi.MapPost("/registrar", (RegistrarUsuarioRequest request, RegistrarUsuarioUseCase useCase) => 
        {
            var response = useCase.Ejecutar(request);
            return Results.Created($"/api/usuarios/{response.IdUsuario}", response);
        }).RequireRateLimiting("auth");
    }
}
