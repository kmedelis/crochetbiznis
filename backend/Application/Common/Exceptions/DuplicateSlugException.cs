namespace Crochetbiznis.Application.Common.Exceptions;

public sealed class DuplicateSlugException : Exception
{
    public string Slug { get; }

    public DuplicateSlugException(string slug)
        : base($"A product with slug '{slug}' already exists.")
        => Slug = slug;

    public DuplicateSlugException(string slug, Exception? innerException)
        : base($"A product with slug '{slug}' already exists.", innerException)
        => Slug = slug;
}