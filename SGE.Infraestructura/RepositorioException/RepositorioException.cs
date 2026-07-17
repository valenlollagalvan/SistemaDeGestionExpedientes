namespace SGE.Infraestructura.RepositorioException;

public class RepositorioException : Exception
{
    public RepositorioException(string? message) : base(message)
    {
    }

    public RepositorioException(string? message, Exception innerException) : base(message, innerException)
    {
    }

}
