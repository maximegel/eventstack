using EventStack.Domain.Internal;

namespace EventStack.Domain
{
    public static class AggregateFactories
    {
        public static IAggregateFactory<TAggregate> NonPublicParamlessCtor<TAggregate>()
            where TAggregate : IAggregateRoot =>
            new AggregateNonPublicParamlessCtor<TAggregate>();

        public static IAggregateFactory<TAggregate> ParamlessCtor<TAggregate>()
            where TAggregate : IAggregateRoot, new() =>
            new AggregateParamlessCtor<TAggregate>();
    }
}