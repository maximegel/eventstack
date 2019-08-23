namespace EventStack.Domain.EventSourcing
{
    public class EventDescriptor
    {
        public EventDescriptor(long version, IDomainEvent data)
        {
            Version = version;
            Data = data;
        }

        public IDomainEvent Data { get; }

        public long Version { get; }
    }
}