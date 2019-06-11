namespace EventStack.Domain
{
    /// <inheritdoc />
    public interface IEntity<out TId> : IEntity
    {
        new TId Id { get; }
    }
}