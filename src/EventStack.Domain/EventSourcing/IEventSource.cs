using System;
using System.Collections.Generic;

namespace EventStack.Domain.EventSourcing
{
    public interface IEventSource : IEntity
    {
        IEventSource Apply(IDomainEvent @event);

        IEventSource Commit(Action<IEnumerable<IDomainEvent>> handler);
    }
}