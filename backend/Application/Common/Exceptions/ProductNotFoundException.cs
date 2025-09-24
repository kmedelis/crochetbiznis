namespace Crochetbiznis.Application.Common.Exceptions;

public sealed class ProductNotFoundException : Exception
{
    public int Id { get; }

    public ProductNotFoundException(int id)
        : base($"Product with id '{id}' was not found.") => Id = id;

    public ProductNotFoundException(int id, Exception? inner)
        : base($"Product with id '{id}' was not found.", inner) => Id = id;
}
