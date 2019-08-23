using System;
using System.Threading.Tasks;
using EventStack.Domain;
using EventStack.Infrastructure.Testing.Doubles;
using FluentAssertions;
using RailSharp;
using Xunit;

namespace EventStack.Infrastructure.Testing
{
    public abstract class WriteOnlyRepositoryTests<TFixture, TAggregate> : IClassFixture<TFixture>
        where TFixture : WriteOnlyRepositoryFixture<TAggregate>
        where TAggregate : class, IDummyAggregateRoot
    {
        protected WriteOnlyRepositoryTests(TFixture fixture)
        {
            Repository = fixture.Repository;
            CreateAggregate = fixture.CreateAggregate;
        }

        protected Func<int, TAggregate> CreateAggregate { get; }

        protected IWriteOnlyRepository<TAggregate, int> Repository { get; }

        [Fact]
        public async Task DeleteAsync_WithExisting_DeletesExisting()
        {
            var toRemove = CreateAggregate(4);

            var foundBefore = (await Repository.FindAsync(4)).Map(_ => true).Reduce(false);
            await Repository.DeleteAsync(toRemove);
            var foundAfter = (await Repository.FindAsync(4)).Map(_ => true).Reduce(false);

            foundBefore.Should().BeTrue();
            foundAfter.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_WithUnexisting_DoesNothing()
        {
            var toRemove = CreateAggregate(8);

            var foundBefore = (await Repository.FindAsync(8)).Map(_ => true).Reduce(false);
            await Repository.DeleteAsync(toRemove);
            var foundAfter = (await Repository.FindAsync(8)).Map(_ => true).Reduce(false);

            foundBefore.Should().BeFalse();
            foundAfter.Should().BeFalse();
        }

        [Fact]
        public async Task FindAsync_WithExistingId_ReturnsSome()
        {
            var found = (await Repository.FindAsync(6)).Reduce(() => null);

            found?.Id.Should().Be(6);
        }

        [Fact]
        public async Task FindAsync_WithUnexistingId_ReturnsNone()
        {
            var found = (await Repository.FindAsync(7)).Map(_ => true).Reduce(false);

            found.Should().BeFalse();
        }

        [Fact]
        public async Task SaveAsync_WithExisting_UpdatesExisting()
        {
            var toUpdate = (await Repository.FindAsync(1)).Reduce(() => null);

            toUpdate?.Id.Should().Be(1);
            toUpdate?.Foo.Should().BeNullOrEmpty();

            toUpdate?.UpdateFoo("Bar");
            await Repository.SaveAsync(toUpdate);
            var foundAfter = (await Repository.FindAsync(1)).Reduce(() => null);

            foundAfter?.Id.Should().Be(1);
            foundAfter?.Foo.Should().Be("Bar");
        }

        [Fact]
        public async Task SaveAsync_WithUnexisting_AddsNew()
        {
            var toAdd = CreateAggregate(2);

            var foundBefore = (await Repository.FindAsync(2)).Map(_ => true).Reduce(false);
            await Repository.SaveAsync(toAdd);
            var foundAfter = Repository.FindAsync(2).Result.Reduce(() => null);

            foundBefore.Should().BeFalse();
            foundAfter?.Id.Should().Be(2);
        }
    }
}