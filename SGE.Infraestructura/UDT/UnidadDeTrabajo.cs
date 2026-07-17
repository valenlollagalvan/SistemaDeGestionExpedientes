using SGE.Aplicacion.Comun;
using SGE.Infraestructura.Datos;


namespace SGE.Infraestructura.UDT;

public sealed class UnidadDeTrabajo : IUnidadDeTrabajo
{
    private readonly SgeContext _context;

    public UnidadDeTrabajo(SgeContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Guardar()
    {
        _context.SaveChanges();
    }
}
