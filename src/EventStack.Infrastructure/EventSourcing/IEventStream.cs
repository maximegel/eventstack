using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStack.Domain;

namespace EventStack.Infrastructure.EventSourcing
{
    public interface IEventStream<TEvent> : IEntity
        where TEvent : class
    {
        IEventStream<TEvent> Append(IEnumerable<TEvent> events);

        Task<TAccumulate> AggregateAsync<TAccumulate>(TAccumulate seed, Func<TAccumulate, TEvent, TAccumulate> func);
    }
}