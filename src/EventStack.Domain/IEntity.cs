﻿namespace EventStack.Domain
{
    /// <summary>
    ///     Represents an object that is not fundamentally defined by its properties, but rather by its thread of continuity
    ///     and its identity.
    ///     <see href="https://lostechies.com/jimmybogard/2008/05/21/entities-value-objects-aggregates-and-roots/" />
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        ///     Identifier that make the entity unique.
        /// </summary>
        object Id { get; }
    }
}