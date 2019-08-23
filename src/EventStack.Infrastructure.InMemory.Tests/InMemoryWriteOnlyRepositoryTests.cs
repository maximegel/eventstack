using EventStack.Infrastructure.Testing;
using EventStack.Infrastructure.Testing.Doubles;

namespace EventStack.Infrastructure.InMemory.Tests
{
    public class InMemoryWriteOnlyRepositoryTests :
        WriteOnlyRepositoryTests<InMemoryWriteOnlyRepositoryFixture, DummyAggregateRoot>
    {
        /// <inheritdoc />
        public InMemoryWriteOnlyRepositoryTests()
            : base(new InMemoryWriteOnlyRepositoryFixture())
        {
        }
    }
}