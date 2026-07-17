using SGE.Aplicacion.Expedientes;
using SGE.Infraestructura.Datos;
using SGE.Dominio.Expedientes;

namespace SGE.Infraestructura.Expedientes;

public class ExpedienteRepository (SgeContext context) : IExpedienteRepository
{
    public void Agregar(Expediente expediente)
    {
        context.Expedientes.Add(expediente);
    }

    public void Eliminar(Guid id)
    {
        var expediente = ObtenerExpedientePorId(id);
        if (expediente is not null)
        {
            context.Expedientes.Remove(expediente);
        }
    }

    public Expediente? ObtenerExpedientePorId(Guid id)
    {
        return context.Expedientes.Find(id);
    }

    public IEnumerable<Expediente> ObtenerTodosExpedientes()
    {
        return context.Expedientes
            .OrderBy(expediente => expediente.FechaCreacion)
            .ToList();
    }
}
