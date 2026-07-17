using Microsoft.EntityFrameworkCore;

namespace SGE.Infraestructura.Datos;

public abstract class Repository<T> where T : class
{
    protected readonly SgeContext _context;
    protected readonly DbSet<T> _dbSet;

    protected Repository(SgeContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<T>();
    }

    public virtual void Agregar(T entidad)
    {
        _dbSet.Add(entidad);
    }

    public virtual void Eliminar(Guid id)
    {
        var entidad = ObtenerPorId(id);
        if (entidad is not null)
        {
            _dbSet.Remove(entidad);
        }
    }

    public virtual T? ObtenerPorId(Guid id)
    {
        return _dbSet.Find(id);
    }

    public virtual IEnumerable<T> ObtenerTodos()
    {
        return _dbSet.ToList();
    }
}
