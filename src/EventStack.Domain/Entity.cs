using System;
using System.Collections.Generic;

namespace EventStack.Domain
{
    /// <inheritdoc cref="IEntity" />
    /// <summary>
    ///     Represents an object that is not fundamentally defined by its properties, but rather by its thread of continuity
    ///     and its identity.
    ///     <see href="https://lostechies.com/jimmybogard/2008/05/21/entities-value-objects-aggregates-and-roots/" />
    /// </summary>
    /// <example>
    ///     A person is a good example of an entity, because a person can't be defined by its properties (name, sex, address
    ///     etc.).
    ///     <code>
    ///          public class Person : Entity;
    ///          {
    ///              // Constructs a existing person.
    ///              public Person(Guid id, string name) :
    ///                  base(id) =&gt;
    ///                  Name = name;
    /// 
    ///              // Constructs a new person.
    ///              public Person(string name) :
    ///                  this(Guid.NewGuid(), name)
    ///              {
    ///              }
    /// 
    ///              public string Name { get; }
    ///          }
    ///      </code>
    /// </example>
    /// <typeparam name="TId"></typeparam>
    public abstract class Entity<TId> :
        IEntity<TId>
    {
        protected Entity(TId id) => Id = id == null ? throw new ArgumentNullException(nameof(id)) : id;

        protected Entity() { }

        /// <inheritdoc />
        object IEntity.Id => Id;

        public TId Id { get; }

        public static bool operator ==(Entity<TId> left, Entity<TId> right) => Equals(left, right);

        public static bool operator !=(Entity<TId> left, Entity<TId> right) => !(left == right);


        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return GetType() == obj.GetType() &&
                   EqualityComparer<object>.Default.Equals((this as IEntity).Id, (obj as IEntity)?.Id);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (13 * GetType().GetHashCode()) ^ (this as IEntity).Id?.GetHashCode() ?? 0;
            }
        }

        public override string ToString() => $"{GetType()}#{Id}";
    }
}