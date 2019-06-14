using System;
using System.Threading;
using EventStack.Domain;
using FluentAssertions;
using Moq;
using RailSharp;
using Xunit;

namespace EventStack.Infrastructure.Testing
{
    public abstract class WritableRepositoryTests<TEntity>
        where TEntity : class, IEntity
    {
        protected abstract IWritableRepository<TEntity> CreateRepository(params TEntity[] storedEntities);

        protected abstract TEntity CreateEntity(string id);

        private static IUnitOfWork CreateUnitOfWorkMock(IUnitOfWorkParticipant participant)
        {
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns((CancellationToken cancellationToken) => participant.SaveAsync(cancellationToken));
            return mock.Object;
        }

        [Fact]
        public void AddOrUpdate_WhenNoCommit_DoesNothing()
        {
            var repository = CreateRepository();
            var entity = CreateEntity("1");

            repository.AddOrUpdate(entity);
            var storedEntityFound = repository.TryFindAsync("1").Result.Map(_ => true).Reduce(false);

            storedEntityFound.Should().BeFalse();
        }

        [Fact]
        public void AddOrUpdate_WithExistingEntity_Updates()
        {
            var repository = CreateRepository(
                CreateEntity("1"),
                CreateEntity("3"),
                CreateEntity("4"));
            var entity = CreateEntity("3");
            var unitOfWorkMock = CreateUnitOfWorkMock((IUnitOfWorkParticipant) repository);

            repository.AddOrUpdate(entity);
            var storedEntityFound = repository.TryFindAsync("3").Result.Reduce(() => null);
            unitOfWorkMock.CommitAsync(CancellationToken.None).Wait();

            storedEntityFound?.Id.Should().Be("3");
        }

        [Fact]
        public void AddOrUpdate_WithNullEntity_Throws()
        {
            var repository = CreateRepository();

            Action act = () => repository.AddOrUpdate(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddOrUpdate_WithUnexistingEntity_Adds()
        {
            var repository = CreateRepository(
                CreateEntity("1"),
                CreateEntity("3"),
                CreateEntity("4"));
            var entity = CreateEntity("2");
            var unitOfWorkMock = CreateUnitOfWorkMock((IUnitOfWorkParticipant) repository);

            repository.AddOrUpdate(entity);
            var storedEntityFound = repository.TryFindAsync("2").Result.Reduce(() => null);
            unitOfWorkMock.CommitAsync(CancellationToken.None).Wait();

            storedEntityFound?.Id.Should().Be("2");
        }

        [Fact]
        public void Remove_WhenNoCommit_DoesNothing()
        {
            var repository = CreateRepository(
                CreateEntity("1"),
                CreateEntity("3"),
                CreateEntity("4"));
            var entity = CreateEntity("3");

            repository.Remove(entity);
            var storedEntityFound = repository.TryFindAsync("1").Result.Map(_ => true).Reduce(false);

            storedEntityFound.Should().BeTrue();
        }

        [Fact]
        public void Remove_WithExistingEntity_Removes()
        {
            var repository = CreateRepository(
                CreateEntity("1"),
                CreateEntity("3"),
                CreateEntity("4"));
            var entity = CreateEntity("3");
            var unitOfWorkMock = CreateUnitOfWorkMock((IUnitOfWorkParticipant) repository);

            repository.Remove(entity);
            var storedEntityFound = repository.TryFindAsync("1").Result.Map(_ => true).Reduce(false);
            unitOfWorkMock.CommitAsync(CancellationToken.None).Wait();

            storedEntityFound.Should().BeTrue();
        }

        [Fact]
        public void Remove_WithNullEntity_Throws()
        {
            var repository = CreateRepository();

            Action act = () => repository.Remove(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Remove_WithUnexistingEntity_DoesNothing()
        {
            var repository = CreateRepository(
                CreateEntity("1"),
                CreateEntity("3"),
                CreateEntity("4"));
            var entity = CreateEntity("3");

            repository.Remove(entity);
            var storedEntityFound = repository.TryFindAsync("1").Result.Map(_ => true).Reduce(false);

            storedEntityFound.Should().BeTrue();
        }

        [Fact]
        public void TryFindAsync_WithExistingId_ReturnsEntity()
        {
            var repository = CreateRepository(
                CreateEntity("1"),
                CreateEntity("3"),
                CreateEntity("4"));

            var storedEntityFound = repository.TryFindAsync("3").Result.Reduce(() => null);

            storedEntityFound.Id.Should().Be("3");
        }

        [Fact]
        public void TryFindAsync_WithNullId_Throws()
        {
            var repository = CreateRepository();

            Action act = () => repository.TryFindAsync(null).Wait();

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TryFindAsync_WithUnexistingId_ReturnsNone()
        {
            var repository = CreateRepository(
                CreateEntity("1"),
                CreateEntity("3"),
                CreateEntity("4"));

            var storedEntityFound = repository.TryFindAsync("2").Result.Map(_ => true).Reduce(false);

            storedEntityFound.Should().BeFalse();
        }
    }
}