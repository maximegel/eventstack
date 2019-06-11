using Moq;

namespace EventStack.Domain.Tests
{
    public class EntityTests : Testing.EntityTests
    {
        /// <inheritdoc />
        protected override IEntity CreateEntity(object id)
        {
            var mock = new Mock<Entity<object>> { CallBase = true };
            mock.As<IEntity>().SetupGet(entity => entity.Id).Returns(() => id);
            return mock.Object;
        }
    }
}