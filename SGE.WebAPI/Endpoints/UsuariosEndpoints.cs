using System.Security.Claims;
using SGE.Aplicacion;
using SGE.Aplicacion.Usuarios;
using SGE.WebAPI.Servicios;
using SGE.Aplicacion.Autorizacion;

namespace SGE.WebAPI.Endpoints;

public static class UsuariosEndpoints
{
    public static void MapUsuariosEndpoints(this IEndpointRouteBuilder app)
    {
        var usuariosApi = app.MapGroup("/api/usuarios").WithTags("Gestión de Usuarios");

        usuariosApi.MapGet("/", (ClaimsPrincipal user, ListarUsuariosUseCase useCase) =>
        {
            var idUsuario = user.GetUserId();
            var request = new ListarUsuariosRequest(idUsuario);
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        }).RequireAuthorization();

        usuariosApi.MapGet("/{id:guid}", (Guid id, ClaimsPrincipal user, IUsuarioRepository usuarioRepository, ObtenerUsuarioPorIdUseCase useCase) =>
        {   
            var idSolicitante = user.GetUserId();
            var solicitante = usuarioRepository.ObtenerPorId(idSolicitante);
            if (id != idSolicitante && solicitante?.EsAdministrador != true)
            {
                throw new AutorizacionException("No tiene permiso para consultar los datos de este usuario.");
            }
            var request = new ObtenerUsuarioPorIdRequest(id);
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        }).RequireAuthorization();

        usuariosApi.MapDelete("/{id:guid}", (Guid id, ClaimsPrincipal user, EliminarUsuarioUseCase useCase) =>
        {
            try
            {
                var idAdmin = user.GetUserId();
                if (id == idAdmin)
                {
                    throw new AutorizacionException("Un administrador no puede eliminar su propia cuenta.");
                }
                var request = new EliminarUsuarioRequest(id, idAdmin);
                useCase.Ejecutar(request);
                return Results.NoContent(); 
            } catch (EntidadNoEncontradaException )
            {
                return Results.NotFound(new { message = $"Usuario con ID {id} no encontrado" });
            }
        }).RequireAuthorization();

        usuariosApi.MapPut("/{id:guid}/permisos", (Guid id, ModificarPermisosUsuarioRequest request, ClaimsPrincipal user, ModificarPermisosUsuarioUseCase useCase) =>
        {
            try
            {
                var idAdmin = user.GetUserId();
                var requestConIds = request with { IdUsuarioObjetivo = id, IdUsuarioEjecutor = idAdmin };
                useCase.Ejecutar(requestConIds);
                return Results.NoContent();
            } catch (EntidadNoEncontradaException )
            {
                return Results.NotFound(new { message = $"Usuario con ID {id} no encontrado" });
            }
        }).RequireAuthorization();

        usuariosApi.MapPut("/mis-datos", (ModificarMisDatosRequest request, ClaimsPrincipal user, ModificarMisDatosUseCase useCase) =>
        {
            var idUsuario = user.GetUserId();
            var requestConIds = request with { IdUsuarioToken = idUsuario, IdUsuarioObjetivo = idUsuario };
            useCase.Ejecutar(requestConIds);
            return Results.NoContent();
        }).RequireAuthorization();
    }
}
