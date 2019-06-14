using EventStack.Domain;
using EventStack.Infrastructure.InMemory.Tests.Doubles;
using EventStack.Infrastructure.Testing;

namespace EventStack.Infrastructure.InMemory.Tests
{
    public class InMemoryWritableRepositoryTests : WritableRepositoryTests<DummyEntity>
    {
        /// <inheritdoc />
        protected override DummyEntity CreateEntity(string id) => new DummyEntity(id);

        /// <inheritdoc />
        protected override IWritableRepository<DummyEntity> CreateRepository(params DummyEntity[] storedEntities) =>
            InMemoryWritableRepository.From(storedEntities);
    }
}