using System.Collections.Generic;

namespace EventStack.Domain.EventSourcing
{
    public interface IEventSource
    {
        IEventSource Apply(EventDescriptor @event);

        IReadOnlyCollection<EventDescriptor> Commit();
    }
}