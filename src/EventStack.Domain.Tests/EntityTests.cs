using Moq;

namespace EventStack.Domain.Tests
{
    public class EntityTests : Testing.EntityTests
    {
        /// <inheritdoc />
        protected override IEntity CreateEntity(string id)
        {
            var mock = new Mock<Entity<string>> {CallBase = true};
            mock.As<IEntity>().SetupGet(entity => entity.Id).Returns(() => id);
            return mock.Object;
        }
    }
}