using System;
using EventStack.Domain;
using EventStack.Domain.EventSourcing;

namespace EventStack.Infrastructure.InMemory.EventSourcing
{
    public class InMemoryEventData : Entity<Guid>
    {
        public InMemoryEventData(string streamId, EventDescriptor evnt)
            : this(streamId, evnt.Version, evnt.Data)
        {
        }

        public InMemoryEventData(string streamId, long sequenceNumber, IDomainEvent data)
            : base(Guid.NewGuid())
        {
            StreamId = streamId;
            SequenceNumber = sequenceNumber;
            Data = data;
        }

        public IDomainEvent Data { get; }

        public long SequenceNumber { get; }

        public string StreamId { get; }
    }
}