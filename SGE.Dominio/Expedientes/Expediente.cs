using System;
using SGE.Dominio.ValueObjects;
using SGE.Dominio.Enums;
using SGE.Dominio.Comun;

namespace SGE.Dominio.Expedientes;

public class Expediente
{
    public Guid Id { get; private set; }
    public Caratula Caratula { get; private set; } = null!;
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }
    public Estado EstadoExpediente { get; private set; }

    public Expediente(Caratula caratula, Guid usuarioCreacion)
    {
        if (caratula is null) throw new DominioException("Carátula es obligatoria.");
        if (usuarioCreacion == Guid.Empty) throw new DominioException("Usuario de creación inválido.");

        Id = Guid.NewGuid();
        Caratula = caratula;
        FechaCreacion = DateTime.UtcNow;
        ActualizarAuditoria(usuarioCreacion);
        EstadoExpediente = Estado.RecienIniciado;
    }

    private Expediente() { }
    private void ActualizarAuditoria(Guid usuarioId)
    {
        FechaUltimaModificacion = DateTime.UtcNow;
        UsuarioUltimoCambio = usuarioId;
    }

    public void ActualizarCaratula(Caratula nuevaCaratula, Guid usuarioId)
    {
        if (nuevaCaratula is null) throw new DominioException("Carátula es obligatoria.");
        if (usuarioId == Guid.Empty) throw new DominioException("Usuario inválido.");

        Caratula = nuevaCaratula;
        ActualizarAuditoria(usuarioId);
    }

    public bool ActualizarEstadoPorTramite(EtiquetaTramite? ultimaEtiqueta, Guid usuarioId)
    {
        if (usuarioId == Guid.Empty) throw new DominioException("Usuario inválido.");

        Estado nuevoEstado = ultimaEtiqueta switch
        {
            EtiquetaTramite.Resolucion => Estado.ConResolucion,
            EtiquetaTramite.PaseAEstudio => Estado.ParaResolver,
            EtiquetaTramite.PaseAlArchivo => Estado.Finalizado,
            _ => Estado.RecienIniciado
        };

        if (nuevoEstado == EstadoExpediente) return false;

        EstadoExpediente = nuevoEstado;
        ActualizarAuditoria(usuarioId);
        return true;
    }

    public void CambiarEstadoManual(Estado nuevoEstado, Guid usuarioId)
    {
        if (!Enum.IsDefined(typeof(Estado), nuevoEstado)) throw new DominioException("Estado inválido.");
        if (usuarioId == Guid.Empty) throw new DominioException("Usuario inválido.");

        EstadoExpediente = nuevoEstado;
        ActualizarAuditoria(usuarioId);
    }
}