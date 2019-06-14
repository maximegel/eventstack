using EventStack.Domain;

namespace EventStack.Infrastructure.InMemory.Tests.Doubles
{
    public class DummyEntity : IEntity
    {
        public DummyEntity(string id) => Id = id;

        /// <inheritdoc />
        public string Id { get; }
    }
}