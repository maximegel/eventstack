using Moq;

namespace EventStack.Domain.Tests
{
    public class EntityTests : Testing.EntityTests
    {
        /// <inheritdoc />
        protected override IEntity<string> CreateEntity(string id)
        {
            var mock = new Mock<Entity<string>> {CallBase = true};
            mock.As<IEntity<string>>().SetupGet(obj => obj.Id).Returns(() => id);
            return mock.Object;
        }
    }
}