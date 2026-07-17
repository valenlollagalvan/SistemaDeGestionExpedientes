using System.Security.Claims;
using SGE.Aplicacion;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Usuarios;
using SGE.WebAPI.Servicios;

namespace SGE.WebAPI.Endpoints;

public static class TramiteEndpoints
{
    public static void MapTramiteEndpoints(this IEndpointRouteBuilder app)
    {
        var tramitesApi = app.MapGroup("/api/tramites").WithTags("Gestión de Trámites");

        tramitesApi.MapPost("/", (AgregarTramiteRequest request, ClaimsPrincipal user, AgregarTramiteUseCase useCase) => 
        {
            try {
                var idUsuario = user.GetUserId();
                var requestConIdUsuario = request with { IdUsuario = idUsuario };
                var response = useCase.Ejecutar(requestConIdUsuario);
                return Results.Created($"/api/tramites/{response.Id}", response);
            } catch (EntidadNoEncontradaException ) {
                return Results.NotFound(new { message = "No se pudo agregar el trámite" });
            }

        }).RequireAuthorization();

        tramitesApi.MapGet("/{expedienteId:guid}", (Guid expedienteId, ListarTramitesUseCase useCase) => 
        {
            try
            {
                var request = new ListarTramiteRequest(expedienteId);
                var response = useCase.Ejecutar(request);
                return Results.Ok(response);
            } catch (EntidadNoEncontradaException ) {
                return Results.NotFound(new { message = $"Expediente con ID {expedienteId} no encontrado" });
            }
        }).RequireAuthorization();

        tramitesApi.MapPut("/{id:guid}", (Guid id, ModificarTramiteRequest request, ClaimsPrincipal user, ModificarTramiteUseCase useCase) => 
        {
            try
            {
                var idUsuario = user.GetUserId();
                var requestConIdUsuario = request with { IdUsuario = idUsuario };
                useCase.Ejecutar(requestConIdUsuario);
                return Results.NoContent();
            } catch (EntidadNoEncontradaException ) {
                return Results.NotFound(new { message = $"Trámite con ID {id} no encontrado" });
            }
        }).RequireAuthorization();

        tramitesApi.MapDelete("/{id:guid}", (Guid id, ClaimsPrincipal user, EliminarTramiteUseCase useCase) => 
        {
            try
            {
                var request = new EliminarTramiteRequest(id, user.GetUserId());
                useCase.Ejecutar(request);
                return Results.NoContent();
            } catch (EntidadNoEncontradaException ) {
                return Results.NotFound(new { message = $"Trámite con ID {id} no encontrado" });
            }
        }).RequireAuthorization();
    }
}