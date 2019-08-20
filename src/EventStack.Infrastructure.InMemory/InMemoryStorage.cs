using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EventStack.Domain;
using RailSharp;

namespace EventStack.Infrastructure.InMemory
{
    public class InMemoryStorage
    {
        private readonly List<Action<IDictionary<(object, object), IEntity>>> _changes =
            new List<Action<IDictionary<(object, object), IEntity>>>();

        private readonly IDictionary<(object collectionKey, object entityId), IEntity> _data =
            new ConcurrentDictionary<(object collectionKey, object entityId), IEntity>();

        public static InMemoryStorage Empty => new InMemoryStorage();

        public void AddOrUpdate(object collectionKey, IEntity entity) =>
            _changes.Add(data => data[(collectionKey, entity.Id)] = entity);

        public void AddOrUpdateRange(object collectionKey, IEnumerable<IEntity> entities) =>
            entities.ToList().ForEach(entity => AddOrUpdate(collectionKey, entity));

        public IEnumerable<IEntity> List(object collectionKey) =>
            _data.Where(pair => pair.Key.collectionKey == collectionKey).Select(pair => pair.Value);

        public void Remove(object collectionKey, IEntity entity) =>
            _changes.Add(data => data.Remove((collectionKey, entity.Id)));

        public void SaveChanges()
        {
            lock (_data)
            {
                // We try to execute all pending actions on a copy of the current storage before running them on the real storage
                // to make sure everything will work properly without error. This ensures that the transaction will be atomic. 
                var dataCopy = new Dictionary<(object, object), IEntity>(_data);
                _changes.ForEach(action => action(dataCopy));

                _changes.ForEach(action => action(_data));
                _changes.Clear();
            }
        }

        public Option<TEntity> TryFind<TEntity>(object collectionKey, object id) =>
            TryFind(collectionKey, id).Map(entity => (TEntity) entity);

        public Option<IEntity> TryFind(object collectionKey, object id) =>
            _data.TryGetValue((collectionKey, id));
    }
}