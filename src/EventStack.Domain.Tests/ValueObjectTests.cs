using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Xunit;

namespace EventStack.Domain.Tests
{
    public class ValueObjectTests
    {
        private static ValueObject CreateValueObject(params object[] equalityValues)
        {
            var mock = new Mock<ValueObject> {CallBase = true};
            mock.Protected().Setup<IEnumerable<object>>("GetEqualityValues").Returns(() => equalityValues);
            return mock.Object;
        }


        [Fact]
        public void EqualityOperator_WithEquivalent_ReturnsTrue()
        {
            var value1 = CreateValueObject("a", 1, true);
            var value2 = CreateValueObject("a", 1, true);

            (value1 == value2).Should().BeTrue();
        }

        [Fact]
        public void EqualityOperator_WithNonEquivalent_ReturnsFalse()
        {
            var value1 = CreateValueObject("a", 1, true);
            var value2 = CreateValueObject("b", 2, false);
            var value3 = CreateValueObject("a", 1, false);
            var value4 = CreateValueObject(null, 1, true);

            (value1 == value2).Should().BeFalse();
            (value1 == value3).Should().BeFalse();
            (value1 == value4).Should().BeFalse();
        }

        [Fact]
        public void EqualityOperator_WithNull_ReturnsFalse()
        {
            var value1 = CreateValueObject("a", 1, true);

            (value1 == null).Should().BeFalse();
            (null == value1).Should().BeFalse();
        }

        [Fact]
        public void Equals_WithEquivalent_ReturnsTrue()
        {
            var value1 = CreateValueObject("a", 1, true);
            var value2 = CreateValueObject("a", 1, true);

            value1.Equals(value2).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithNonEquivalent_ReturnsFalse()
        {
            var value1 = CreateValueObject("a", 1, true);
            var value2 = CreateValueObject("b", 2, false);
            var value3 = CreateValueObject("a", 1, false);
            var value4 = CreateValueObject(null, 1, true);

            value1.Equals(value2).Should().BeFalse();
            value1.Equals(value3).Should().BeFalse();
            value1.Equals(value4).Should().BeFalse();
        }

        [Fact]
        public void Equals_WithNull_ReturnsFalse()
        {
            var value1 = CreateValueObject("a", 1, true);

            value1.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_WithEquivalent_ReturnsSame()
        {
            var hashCode1 = CreateValueObject("a", 1, true).GetHashCode();
            var hashCode2 = CreateValueObject("a", 1, true).GetHashCode();

            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_WithNonEquivalent_ReturnsDifferent()
        {
            var hashCode1 = CreateValueObject("a", 1, true).GetHashCode();
            var hashCode2 = CreateValueObject("b", 2, false).GetHashCode();
            var hashCode3 = CreateValueObject("a", 1, false).GetHashCode();
            var hashCode4 = CreateValueObject(null, 1, true).GetHashCode();

            hashCode1.Should().NotBe(hashCode2);
            hashCode1.Should().NotBe(hashCode3);
            hashCode1.Should().NotBe(hashCode4);
        }

        [Fact]
        public void InequalityOperator_WithEquivalent_ReturnsFalse()
        {
            var value1 = CreateValueObject("a", 1, true);
            var value2 = CreateValueObject("a", 1, true);

            (value1 != value2).Should().BeFalse();
        }

        [Fact]
        public void InequalityOperator_WithNonEquivalent_ReturnsTrue()
        {
            var value1 = CreateValueObject("a", 1, true);
            var value2 = CreateValueObject("b", 2, false);
            var value3 = CreateValueObject("a", 1, false);
            var value4 = CreateValueObject(null, 1, true);

            (value1 != value2).Should().BeTrue();
            (value1 != value3).Should().BeTrue();
            (value1 != value4).Should().BeTrue();
        }

        [Fact]
        public void InequalityOperator_WithNull_ReturnsTrue()
        {
            var value1 = CreateValueObject("a", 1, true);

            (value1 != null).Should().BeTrue();
            (null != value1).Should().BeTrue();
        }
    }
}