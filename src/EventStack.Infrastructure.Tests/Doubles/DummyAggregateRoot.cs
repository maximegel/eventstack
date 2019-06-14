using System.Collections.Generic;
using EventStack.Domain;

namespace EventStack.Infrastructure.Tests.Doubles
{
    public class DummyAggregateRoot : IAggregateRoot<DummyAggregateRoot, DummyEvent>
    {
        public DummyAggregateRoot(string id, long version)
        {
            Id = id;
            Version = version;
        }

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public long Version { get; }

        /// <inheritdoc />
        public DummyAggregateRoot Apply(DummyEvent @event) => throw new System.NotImplementedException();

        /// <inheritdoc />
        public IReadOnlyCollection<DummyEvent> Commit() => throw new System.NotImplementedException();
    }
}