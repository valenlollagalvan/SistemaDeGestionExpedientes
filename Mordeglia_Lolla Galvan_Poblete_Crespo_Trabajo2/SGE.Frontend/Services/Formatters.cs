namespace SGE.Frontend.Services;

public static class Formatters
{
    public static string Estado(string? value)
    {
        return value switch
        {
            "RecienIniciado" => "Recien iniciado",
            "ParaResolver" => "Para resolver",
            "ConResolucion" => "Con resolucion",
            "EnNotificacion" => "En notificacion",
            "Finalizado" => "Finalizado",
            null or "" => "Sin estado",
            _ => value
        };
    }

    public static string Etiqueta(string? value)
    {
        return value switch
        {
            "EscritoPresentado" => "Escrito presentado",
            "PaseAEstudio" => "Pase a estudio",
            "Resolucion" => "Resolucion",
            "Notificacion" => "Notificacion",
            "PaseAlArchivo" => "Pase al archivo",
            "Despacho" => "Despacho",
            null or "" => "Sin etiqueta",
            _ => value
        };
    }

    public static string Fecha(DateTime value)
    {
        return value.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
    }

    public static string ShortId(Guid value)
    {
        return value.ToString("N")[..8].ToUpperInvariant();
    }
}
