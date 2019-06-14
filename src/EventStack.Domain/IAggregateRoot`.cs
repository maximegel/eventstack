using System.Collections.Generic;

namespace EventStack.Domain
{
    /// <inheritdoc />
    public interface IAggregateRoot<out TSelf, TEvent> : IAggregateRoot
        where TSelf : IAggregateRoot<TSelf, TEvent>
    {
        TSelf Apply(TEvent @event);

        IReadOnlyCollection<TEvent> Commit();
    }
}