using System;
using System.Collections.Generic;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public interface IExpedienteRepository
{
    void Agregar(Expediente expediente);
    void Eliminar(Guid id);
    Expediente? ObtenerExpedientePorId(Guid id);
    IEnumerable<Expediente>ObtenerTodosExpedientes();
}
