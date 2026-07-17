using System.Text.Json.Serialization;

namespace SGE.Frontend.Models;

public record LoginRequest(string CorreoElectronico, string Contrasena);

public record LoginResponse(string Token);

public record RegistrarUsuarioRequest(string Nombre, string CorreoElectronico, string Contrasena);

public record RegistrarUsuarioResponse(Guid IdUsuario);

public record ExpedienteDto(
    Guid Id,
    string Caratula,
    DateTime FechaCreacion,
    DateTime FechaUltimaModificacion,
    Guid UsuarioUltimoCambio,
    string? Estado);

public record ObtenerTodosExpedienteResponse(IEnumerable<ExpedienteDto> Expedientes);

public record ObtenerExpedientePorIdResponse(
    Guid Id,
    string Caratula,
    DateTime FechaCreacion,
    DateTime FechaUltimaModificacion,
    Guid UsuarioUltimoCambio,
    string? Estado);

public record AltaExpedienteRequest(string Caratula, Guid IdUsuario);

public record AltaExpedienteResponse(Guid IdExpediente);

public record ModificarCaratulaExpedienteRequest(Guid IdExpediente, Guid IdUsuario, string NuevaCaratula);

public record ModificarEstadoExpedienteRequest(Guid IdExpediente, Guid IdUsuario, string? EstadoNuevo);

public record TramiteDto(
    Guid Id,
    Guid ExpedienteId,
    string? Contenido,
    string? Etiqueta,
    DateTime FechaCreacion,
    DateTime FechaUltimaModificacion,
    Guid UsuarioUltimoCambio);

public record ListarTramitesResponse(IEnumerable<TramiteDto> Tramites);

public record AgregarTramiteRequest(
    Guid ExpedienteId,
    [property: JsonPropertyName("contenido")] string? Contenido,
    [property: JsonPropertyName("etiqueta")] string? Etiqueta,
    Guid IdUsuario);

public record AgregarTramiteResponse(Guid Id);

public record ModificarTramiteRequest(Guid Id, string? Contenido, string? Etiqueta, Guid IdUsuario);

public record UsuarioDto(
    Guid Id,
    string Nombre,
    string CorreoElectronico,
    bool EsAdministrador,
    IEnumerable<int> Permisos);

public record ListarUsuariosResponse(IEnumerable<UsuarioDto> Usuarios);

public record ObtenerUsuarioPorIdResponse(UsuarioDto Usuario);

public record ModificarMisDatosRequest(
    Guid IdUsuarioToken,
    Guid IdUsuarioObjetivo,
    string Nombre,
    string CorreoElectronico,
    string? NuevaContrasena);

public record ModificarPermisosUsuarioRequest(
    Guid IdUsuarioObjetivo,
    Guid IdUsuarioEjecutor,
    IEnumerable<int> PermisosAAsignar,
    IEnumerable<int> PermisosARemover);

public record PermisoOption(int Valor, string Nombre, string Descripcion);

public static class CatalogosSge
{
    public static readonly string[] Estados =
    [
        "RecienIniciado",
        "ParaResolver",
        "ConResolucion",
        "EnNotificacion",
        "Finalizado"
    ];

    public static readonly string[] EtiquetasTramite =
    [
        "EscritoPresentado",
        "PaseAEstudio",
        "Despacho",
        "Resolucion",
        "Notificacion",
        "PaseAlArchivo"
    ];

    public static readonly PermisoOption[] Permisos =
    [
        new(0, "ExpedienteAlta", "Alta de expedientes"),
        new(1, "ExpedienteBaja", "Baja de expedientes"),
        new(2, "ExpedienteModificacion", "Modificacion de expedientes"),
        new(3, "TramiteAlta", "Alta de tramites"),
        new(4, "TramiteBaja", "Baja de tramites"),
        new(5, "TramiteModificacion", "Modificacion de tramites")
    ];
}
