namespace EventStack.Domain
{
    public interface IVersioned
    {
        long Version { get; }
    }
}