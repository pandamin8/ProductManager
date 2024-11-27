namespace ProductManager.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string resourceType, int resourceIdentifier) : base(
        $"{resourceType} {resourceIdentifier} not found.")
    { }

    public NotFoundException(string resourceType, string resourceIdentifier) : base(
        $"{resourceType} {resourceIdentifier} not found.")
    { }
}
