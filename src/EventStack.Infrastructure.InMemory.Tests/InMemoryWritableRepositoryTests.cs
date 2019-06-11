using EventStack.Infrastructure.Testing;
using EventStack.Infrastructure.Testing.Doubles;

namespace EventStack.Infrastructure.InMemory.Tests
{
    public class InMemoryWritableRepositoryTests : WritableRepositoryTests<InMemoryWritableRepository<DummyEntity>>
    {
        /// <inheritdoc />
        protected override InMemoryWritableRepository<DummyEntity>
            CreateRepository(params DummyEntity[] storedEntities) => InMemoryWritableRepository.From(storedEntities);
    }
}