using SGE.Dominio.ValueObjects;
using SGE.Dominio.Enums;
using SGE.Dominio.Comun;

namespace SGE.Dominio.Tramites;

public class Tramite
{
    public Guid Id { get; private set; }
    public Guid ExpedienteId { get; private set; }
    public EtiquetaTramite Etiqueta { get; private set; }
    public ContenidoTramite Contenido { get; private set; } = null!;
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }

    public Tramite(Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioCreacion)
    {
        ValidarExpedienteId(expedienteId);
        ValidarEtiqueta(etiqueta);
        ValidarContenido(contenido);
        ValidarUsuario(usuarioCreacion, "Usuario de creación inválido.");

        Id = Guid.NewGuid();
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido;
        FechaCreacion = DateTime.UtcNow;
        ActualizarAuditoria(usuarioCreacion);
    }

    private Tramite()
    {
    }

    public void Modificar(ContenidoTramite nuevoContenido, EtiquetaTramite nuevaEtiqueta, Guid usuarioId)
    {
        ValidarContenido(nuevoContenido);
        ValidarEtiqueta(nuevaEtiqueta);
        ValidarUsuario(usuarioId, "Usuario inválido.");

        Contenido = nuevoContenido;
        Etiqueta = nuevaEtiqueta;
        ActualizarAuditoria(usuarioId);
    }

    public void ActualizarContenido(ContenidoTramite nuevoContenido, Guid usuarioId)
    {
        ValidarContenido(nuevoContenido);
        ValidarUsuario(usuarioId, "Usuario inválido.");

        Contenido = nuevoContenido;
        ActualizarAuditoria(usuarioId);
    }

    public void CambiarEtiqueta(EtiquetaTramite nuevaEtiqueta, Guid usuarioId)
    {
        ValidarEtiqueta(nuevaEtiqueta);
        ValidarUsuario(usuarioId, "Usuario inválido.");

        Etiqueta = nuevaEtiqueta;
        ActualizarAuditoria(usuarioId);
    }

    private void ActualizarAuditoria(Guid usuarioId)
    {
        FechaUltimaModificacion = DateTime.UtcNow;
        if (FechaUltimaModificacion < FechaCreacion)
        {
            FechaUltimaModificacion = FechaCreacion;
        }

        UsuarioUltimoCambio = usuarioId;
    }

    private static void ValidarExpedienteId(Guid expedienteId)
    {
        if (expedienteId == Guid.Empty)
        {
            throw new DominioException("ExpedienteId inválido.");
        }
    }

    private static void ValidarEtiqueta(EtiquetaTramite etiqueta)
    {
        if (!Enum.IsDefined(typeof(EtiquetaTramite), etiqueta))
        {
            throw new DominioException("Etiqueta inválida.");
        }
    }

    private static void ValidarContenido(ContenidoTramite contenido)
    {
        if (contenido is null)
        {
            throw new DominioException("Contenido es obligatorio.");
        }
    }

    private static void ValidarUsuario(Guid usuarioId, string mensajeError)
    {
        if (usuarioId == Guid.Empty)
        {
            throw new DominioException(mensajeError);
        }
    }
}
