namespace EventStack.Domain
{
    public interface IVersionedEntity : IEntity
    {
        long Version { get; }
    }
}