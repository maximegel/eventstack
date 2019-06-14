using EventStack.Infrastructure.EventSourcing;
using EventStack.Infrastructure.EventSourcing.Aggregation;
using EventStack.Infrastructure.Tests.Doubles;
using Xunit;

namespace EventStack.Infrastructure.Tests
{
    public class EventSourcedRepositoryTests
    {
        [Fact]
        public void Test1()
        {
            var repository = EventSourcedRepositoryBuilder<DummyAggregateRoot, DummyEvent>
                .For(null)
                .UseAggregator(flows => flows.ReadAllFromBeginning().WithAggregateFactory(null))
                .Build();
        }
    }
}