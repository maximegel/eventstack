using System;
using EventStack.Domain;
using EventStack.Domain.EventSourcing;

namespace EventStack.Infrastructure.EventSourcing
{
    public class EventSourcedRepositoryBuilder<TAggregate, TId> :
        EventSourcedRepositoryBuilder<TAggregate, TId>.IInitialized,
        EventSourcedRepositoryBuilder<TAggregate, TId>.IBuildable
        where TAggregate : class, IAggregateRoot<TId>, IEventSource
    {
        private Func<TAggregate> _aggregateFactory = Factories.NonPublicParamlessCtor<TAggregate>;
        private IEventStore<EventDescriptor> _eventStore;
        private Func<TId, Type, string> _streamIdResolver = (id, _) => id.ToString();

        public IWriteOnlyRepository<TAggregate, TId> Build() =>
            new EventSourcedRepository<TAggregate, TId>
                {
                    AggregateFactory = _aggregateFactory,
                    EventStore = _eventStore,
                    StreamIdResolver = _streamIdResolver
                }
                .UseGuardClauses();

        public IBuildable UseAggregateFactory(Func<TAggregate> factory)
        {
            _aggregateFactory = factory;
            return this;
        }

        public IBuildable UseStreamIdResolver(Func<TId, Type, string> resolver)
        {
            _streamIdResolver = resolver;
            return this;
        }

        public IBuildable UseEventStore(IEventStore<EventDescriptor> store)
        {
            _eventStore = store;
            return this;
        }

        internal static IInitialized Initialize() => new EventSourcedRepositoryBuilder<TAggregate, TId>();

        public interface IInitialized
        {
            IBuildable UseEventStore(IEventStore<EventDescriptor> store);
        }

        public interface IBuildable
        {
            IWriteOnlyRepository<TAggregate, TId> Build();

            IBuildable UseAggregateFactory(Func<TAggregate> factory);

            IBuildable UseStreamIdResolver(Func<TId, Type, string> resolver);
        }
    }
}