using EventStack.Domain;

namespace EventStack.Infrastructure.Testing.Doubles
{
    public class DummyEntity : IEntity
    {
        public DummyEntity(int id) => Id = id;

        /// <inheritdoc />
        public object Id { get; }
    }
}