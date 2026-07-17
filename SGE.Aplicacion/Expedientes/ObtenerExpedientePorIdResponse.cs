using SGE.Aplicacion.Tramites;
namespace SGE.Aplicacion.Expedientes;

public record class ObtenerExpedientePorIdResponse(
    Guid Id,
    string Caratula,
    DateTime FechaCreacion,
    DateTime FechaUltimaModificacion,
    Guid UsuarioUltimoCambio,
    string Estado,
    List<TramiteDTO> Tramites 
);