using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using EventStack.Domain;
using EventStack.Infrastructure.InMemory.Internal;
using RailSharp;

namespace EventStack.Infrastructure.InMemory
{
    public class InMemoryStorage
    {
        private readonly List<Action<IDictionary<string, ImmutableList<IEntity>>>> _changes =
            new List<Action<IDictionary<string, ImmutableList<IEntity>>>>();

        private readonly IDictionary<string, ImmutableList<IEntity>> _data =
            new ConcurrentDictionary<string, ImmutableList<IEntity>>();

        public static InMemoryStorage Empty => new InMemoryStorage();

        public void AddOrUpdate(string collectionKey, IEntity entity) =>
            _changes.Add(
                data => data[collectionKey] = List(data, collectionKey)
                    .Select((e, index) => (e, index))
                    .TryFirst(pair => pair.e.Equals(entity))
                    .Map(pair => pair.index)
                    .Map(index => List(data, collectionKey).SetItem(index, entity))
                    .Reduce(List(data, collectionKey).Add(entity)));

        public void AddOrUpdateRange(string collectionKey, IEnumerable<IEntity> entities) =>
            entities.ToList().ForEach(entity => AddOrUpdate(collectionKey, entity));

        public Option<TEntity> Find<TEntity, TId>(string collectionKey, TId id)
            where TEntity : IEntity<TId> =>
            _data.TryGetValue(collectionKey)
                .Map(entities => entities.OfType<TEntity>())
                .FlatMap(entities => entities.TryFirst(entity => entity.Id.Equals(id)));

        public IEnumerable<TEntity> List<TEntity, TId>(string collectionKey)
            where TEntity : IEntity<TId> =>
            List(_data, collectionKey).Select(entity => (TEntity) entity);

        public void Remove(string collectionKey, IEntity entity) =>
            _changes.Add(data => data[collectionKey] = List(data, collectionKey).RemoveAll(e => e.Equals(entity)));

        public void RemoveRange(string collectionKey, IEnumerable<IEntity> entities) =>
            entities.ToList().ForEach(entity => Remove(collectionKey, entity));

        public void SaveChanges()
        {
            lock (_data)
            {
                // We try to execute all pending actions on a copy of the current storage before running them on the real storage
                // to make sure everything will work properly without error. This ensures that the transaction will be atomic. 
                var dataCopy = new Dictionary<string, ImmutableList<IEntity>>(_data);
                _changes.ForEach(action => action(dataCopy));

                _changes.ForEach(action => action(_data));
                _changes.Clear();
            }
        }

        private static ImmutableList<IEntity> List(
            IDictionary<string, ImmutableList<IEntity>> data,
            string collectionKey) =>
            data.TryGetValue(collectionKey).Reduce(ImmutableList<IEntity>.Empty);
    }
}