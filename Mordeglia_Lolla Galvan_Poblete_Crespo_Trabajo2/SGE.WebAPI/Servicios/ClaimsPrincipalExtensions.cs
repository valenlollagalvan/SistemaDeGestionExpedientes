using System.Security.Claims;

namespace SGE.WebAPI.Servicios;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst("ID");
        if (claim is null || !Guid.TryParse(claim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("No se pudo determinar el usuario autenticado.");
        }

        return userId;
    }
}
