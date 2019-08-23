using FluentAssertions;
using Xunit;

namespace EventStack.Domain.Testing
{
    public abstract class EntityTests
    {
        protected abstract IEntity<string> CreateEntity(string id);

        [Fact]
        public void Equals_WithEquivalent_ReturnsTrue()
        {
            var entity1 = CreateEntity("1");
            var entity2 = CreateEntity("1");

            entity1.Equals(entity2).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithNonEquivalent_ReturnsFalse()
        {
            var entity1 = CreateEntity("1");
            var entity2 = CreateEntity("2");
            var entity3 = CreateEntity(null);

            entity1.Equals(entity2).Should().BeFalse();
            entity1.Equals(entity3).Should().BeFalse();
        }

        [Fact]
        public void Equals_WithNull_ReturnsFalse()
        {
            var entity1 = CreateEntity("1");

            entity1.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_WithEquivalent_ReturnsSame()
        {
            var hashCode1 = CreateEntity("1").GetHashCode();
            var hashCode2 = CreateEntity("1").GetHashCode();

            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_WithNonEquivalent_ReturnsDifferent()
        {
            var hashCode1 = CreateEntity("1").GetHashCode();
            var hashCode2 = CreateEntity("2").GetHashCode();
            var hashCode3 = CreateEntity(null).GetHashCode();

            hashCode1.Should().NotBe(hashCode2);
            hashCode1.Should().NotBe(hashCode3);
        }
    }
}