using EventStack.Domain;

namespace EventStack.Infrastructure.EventSourcing
{
    public interface IAggregateFactory<out TAggregate>
        where TAggregate : IAggregateRoot
    {
        TAggregate Create();
    }
}