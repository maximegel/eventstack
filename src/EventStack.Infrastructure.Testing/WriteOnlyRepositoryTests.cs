using System.Collections.Generic;
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
            fixture.Seed(Seed);
            Repository = fixture.Repository;
            UnitOfWork = fixture.UnitOfWork;
        }

        private IEnumerable<TAggregate> Seed => new[]
        {
            // XXX: Id=1 is updated by a test bellow.
            CreateAggregate(1),
            // XXX: Id=2 is added by a test bellow.
            // XXX: Id=3 could be added by a test bellow.
            // XXX: Id=4 is removed by a test bellow.
            CreateAggregate(4),
            // XXX: Id=5 could be removed by a test bellow.
            CreateAggregate(5),
            CreateAggregate(6)
            // XXX: Id=7 should not exist.
        };

        protected IUnitOfWork UnitOfWork { get; }

        protected IWriteOnlyRepository<TAggregate> Repository { get; }

        protected abstract TAggregate CreateAggregate(int id);

        [Fact]
        public void AddOrUpdate_WhenCommittingWithExisting_UpdatesExisting()
        {
            var toUpdate = CreateAggregate(1);
            toUpdate.UpdateFoo("Bar");

            Repository.AddOrUpdate(toUpdate);
            UnitOfWork.CommitAsync().Wait();
            var stored = Repository.TryFindAsync(1).Result.Reduce(() => null);

            stored?.Id.Should().Be(1);
            stored?.Foo.Should().Be("Bar");
        }

        [Fact]
        public void AddOrUpdate_WhenCommittingWithUnexisting_AddsNew()
        {
            var toAdd = CreateAggregate(2);

            Repository.AddOrUpdate(toAdd);
            var stored = Repository.TryFindAsync(2).Result.Reduce(() => null);
            UnitOfWork.CommitAsync().Wait();

            stored?.Id.Should().Be(2);
        }

        [Fact]
        public void AddOrUpdate_WithoutCommitting_DoesNothing()
        {
            var toAddOrUpdate = CreateAggregate(3);

            Repository.AddOrUpdate(toAddOrUpdate);
            var stored = Repository.TryFindAsync(3).Result.Map(_ => true).Reduce(false);

            stored.Should().BeFalse();
        }

        [Fact]
        public void Remove_WhenCommittingWithExisting_RemovesExisting()
        {
            var toRemove = CreateAggregate(4);

            Repository.Remove(toRemove);
            UnitOfWork.CommitAsync().Wait();
            var stored = Repository.TryFindAsync(4).Result.Map(_ => true).Reduce(false);

            stored.Should().BeFalse();
        }

        [Fact]
        public void Remove_WithoutCommitting_DoesNothing()
        {
            var toRemove = CreateAggregate(5);

            Repository.Remove(toRemove);
            var stored = Repository.TryFindAsync(5).Result.Map(_ => true).Reduce(false);

            stored.Should().BeTrue();
        }

        [Fact]
        public void TryFindAsync_WithExistingId_ReturnsEntity()
        {
            var stored = Repository.TryFindAsync(6).Result.Reduce(() => null);

            stored.Id.Should().Be(6);
        }

        [Fact]
        public void TryFindAsync_WithUnexistingId_ReturnsNone()
        {
            var stored = Repository.TryFindAsync(7).Result.Map(_ => true).Reduce(false);

            stored.Should().BeFalse();
        }
    }
}