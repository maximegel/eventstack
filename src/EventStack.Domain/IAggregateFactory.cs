namespace EventStack.Domain
{
    public interface IAggregateFactory<out TAggregate>
        where TAggregate : IAggregateRoot
    {
        TAggregate Create();
    }
}