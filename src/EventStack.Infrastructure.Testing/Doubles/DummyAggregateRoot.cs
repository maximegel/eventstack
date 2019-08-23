using EventStack.Domain;

namespace EventStack.Infrastructure.Testing.Doubles
{
    public class DummyAggregateRoot : Entity<int>,
        IDummyAggregateRoot
    {
        public DummyAggregateRoot(int id)
            : base(id)
        {
        }

        public string Foo { get; private set; }

        public void UpdateFoo(string value) => Foo = value;
    }
}