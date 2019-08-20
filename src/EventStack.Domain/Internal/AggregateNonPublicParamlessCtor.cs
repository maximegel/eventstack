using System;

namespace EventStack.Domain.Internal
{
    internal class AggregateNonPublicParamlessCtor<TAggregate> : IAggregateFactory<TAggregate>
        where TAggregate : IAggregateRoot
    {
        public TAggregate Create() => (TAggregate) Activator.CreateInstance(typeof(TAggregate), true);
    }
}