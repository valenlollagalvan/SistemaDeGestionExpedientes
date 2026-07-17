using System;
using SGE.Dominio.Comun;

namespace SGE.Dominio.ValueObjects;

public record class Caratula
{
    public string Texto { get; }

    public Caratula(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            throw new DominioException("La carátula no puede ser vacía.");

        Texto = texto;
    }
}
