using System;
using System.Collections.Generic;
using EventStack.Domain;

namespace EventStack.Infrastructure.EventSourcing
{
    public class EventStream<TEvent> : Entity<string>
        where TEvent : class
    {
        public void Append(IEnumerable<TEvent> events) { throw new NotImplementedException(); }

        public IAsyncEnumerable<TEvent> ReadForward() => throw new NotImplementedException();
    }
}