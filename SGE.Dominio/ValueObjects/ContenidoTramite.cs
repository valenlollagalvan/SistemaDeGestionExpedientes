using System;
using SGE.Dominio.Comun;

namespace SGE.Dominio.ValueObjects;

public record class ContenidoTramite
{
    public string Texto { get; }

    public ContenidoTramite(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            throw new DominioException("El contenido del trámite no puede ser vacío.");

        Texto = texto;
    }
}
