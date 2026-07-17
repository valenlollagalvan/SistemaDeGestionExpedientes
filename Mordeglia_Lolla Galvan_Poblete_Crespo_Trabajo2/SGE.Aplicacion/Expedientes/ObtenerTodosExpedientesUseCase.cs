using System;
using System.Linq;
using SGE.Aplicacion;
using SGE.Dominio;

using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class ObtenerTodosExpedientesUseCase
{
    private readonly IExpedienteRepository _repository;
   public ObtenerTodosExpedientesUseCase(IExpedienteRepository repository)
    {
        _repository = repository;
    }
    public ObtenerTodosExpedienteResponse Ejecutar()
    {
        var expedientes =  _repository.ObtenerTodosExpedientes();   
        var expedientesDTO = expedientes.Select(e => new ExpedienteDTO(
            e.Id,
            e.Caratula.Texto, 
            e.FechaCreacion,
            e.FechaUltimaModificacion,
            e.UsuarioUltimoCambio,
            e.EstadoExpediente.ToString()
        )).ToList();
        return new ObtenerTodosExpedienteResponse(expedientesDTO);
    }
}

