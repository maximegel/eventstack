using System;

namespace EventStack.Domain
{
    /// <inheritdoc cref="IEntity" />
    /// <typeparam name="TId"></typeparam>
    public interface IEntity<TId> : IEntity,
        IEquatable<IEntity<TId>>
    {
        /// <summary>
        ///     Identifier that make the entity unique.
        /// </summary>
        TId Id { get; }
    }
}