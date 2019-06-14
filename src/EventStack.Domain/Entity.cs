using System;

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
    public abstract class Entity<TId> : IEntity,
        IEquatable<IEntity>
    {
        private readonly string _id;

        protected Entity(TId id) => _id = id == null ? throw new ArgumentNullException(nameof(id)) : _id;

        protected Entity() { }

        string IEntity.Id => _id;

        /// <inheritdoc />
        public bool Equals(IEntity other) => other != null && ((IEntity) this).Id == other.Id;

        public static bool operator ==(Entity<TId> left, Entity<TId> right) => Equals(left, right);

        public static bool operator !=(Entity<TId> left, Entity<TId> right) => !(left == right);

        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj) ||
            !(obj is null) && GetType() == obj.GetType() && Equals(obj as IEntity);

        public override int GetHashCode() =>
            unchecked((13 * GetType().GetHashCode()) ^ ((IEntity) this).Id?.GetHashCode() ?? 0);

        public override string ToString() => $"{GetType()}#{((IEntity) this).Id}";
    }
}