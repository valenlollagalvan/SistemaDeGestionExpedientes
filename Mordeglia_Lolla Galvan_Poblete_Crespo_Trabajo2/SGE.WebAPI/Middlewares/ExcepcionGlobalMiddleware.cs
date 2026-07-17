using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Comun;

namespace SGE.WebAPI.Middlewares;

public class ExcepcionGlobalMiddleware : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var traceId = httpContext.TraceIdentifier;
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
            Extensions = { ["traceId"] = traceId }
        };

        if (exception is EntidadNoEncontradaException)
        {
            problemDetails.Title = "Recurso no encontrado";
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Detail = exception.Message;
        }
        else if (exception is UnauthorizedAccessException)
        {
            problemDetails.Title = "No autorizado";
            problemDetails.Status = StatusCodes.Status401Unauthorized;
            problemDetails.Detail = exception.Message;
        }
        else if (exception is AutorizacionException)
        {
            problemDetails.Title = "Acceso denegado";
            problemDetails.Status = StatusCodes.Status403Forbidden;
            problemDetails.Detail = exception.Message;
        }
        else if (exception is ArgumentException)
        {
            problemDetails.Title = "Argumento inválido";
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Detail = exception.Message;
        }
        else if (exception is DominioException)
        {
            problemDetails.Title = "Error de dominio";
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Detail = exception.Message;
        }
        else
        {
            problemDetails.Title = "Error interno del servidor";
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Detail = $"Ha ocurrido un error inesperado. Consulte el identificador {traceId}.";
        }

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
