using Microsoft.EntityFrameworkCore;
using SGE.Aplicacion.Tramites;
using SGE.Infraestructura.Datos;
using SGE.Dominio.Tramites;

namespace SGE.Infraestructura;

public class TramiteRepository(SgeContext context) : ITramiteRepository
{
    public void AgregarTramite(Tramite tramite)
    {
        context.Tramites.Add(tramite);
    }

    public Tramite? ObtenerTramiteId(Guid id)
    {
        return context.Tramites.Find(id);
    }

    public void EliminarTramite(Guid id)
    {
        var tramite = ObtenerTramiteId(id);
        if (tramite is not null)
        {
            context.Tramites.Remove(tramite);
        }
    }

    public IEnumerable<Tramite> ObtenerTramitesPorExpediente(Guid expedienteId)
    {
        return TramitesVigentes()
            .Where(tramite => tramite.ExpedienteId == expedienteId)
            .OrderBy(tramite => tramite.FechaCreacion)
            .ToList();
    }

    public void EliminarTramitePorExpediente(Guid expedienteId)
    {
        context.Tramites.RemoveRange(ObtenerTramitesPorExpediente(expedienteId));
    }

    public IEnumerable<Tramite> ObtenerTodos()
    {
        return TramitesVigentes()
            .OrderBy(tramite => tramite.FechaCreacion)
            .ToList();
    }

    private IEnumerable<Tramite> TramitesVigentes()
    {
        var idsEliminados = context.Tramites.Local
            .Where(tramite => context.Entry(tramite).State == EntityState.Deleted)
            .Select(tramite => tramite.Id)
            .ToHashSet();

        var tramitesLocales = context.Tramites.Local
            .Where(tramite => context.Entry(tramite).State != EntityState.Deleted);

        return tramitesLocales
            .Concat(context.Tramites.Where(tramite => !idsEliminados.Contains(tramite.Id)))
            .DistinctBy(tramite => tramite.Id);
    }
}
