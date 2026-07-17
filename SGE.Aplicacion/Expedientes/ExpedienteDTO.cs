using System;

namespace SGE.Aplicacion.Expedientes;

public record class ExpedienteDTO (Guid Id, string Caratula, DateTime FechaCreacion, DateTime 
                                  FechaUltimaModificacion, Guid UsuarioUltimoCambio, string? Estado);
