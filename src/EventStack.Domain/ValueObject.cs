using System;
using System.Collections.Generic;
using System.Linq;

namespace EventStack.Domain
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents an objects without conceptual identity. Value objects also describe a characteristic of a thing and are
    ///     distinguishable only by the state of their properties.
    ///     <see href="https://lostechies.com/jimmybogard/2008/05/21/entities-value-objects-aggregates-and-roots/" />
    ///     <para>
    ///         The value objects never changes their properties from the moment it is created until the moment it is
    ///         destroyed. Following this rule, each operation of the value object class must return a new instance of that
    ///         value object instead of modifying it.
    ///     </para>
    /// </summary>
    /// <example>
    ///     An amount of money is a good example of a value object, because at the opposite of a person an amount of money can
    ///     be defined by its properties (in this case, the Amount and CurrencySymbol properties).
    ///     <code>
    ///          public sealed class MoneyAmount : ValueObject
    ///          {
    ///              public MoneyAmount(decimal amount, string currencySymbol)
    ///              {
    ///                  Amount = amount;
    ///                  CurrencySymbol = currencySymbol;
    ///              }
    ///              public decimal Amount { get; }
    ///              public string CurrencySymbol { get; }
    ///              // We create a new instance instead of changing properties to conserve immutability.
    ///              public static MoneyAmount operator *(MoneyAmount moneyAmount, decimal factor) =&gt;
    ///                  new MoneyAmount(moneyAmount.Amount * factor, moneyAmount.CurrencySymbol);
    ///              protected override IEnumerable&lt;object&gt; GetEqualityValues() =&gt;
    ///                  new object[] {Amount, CurrencySymbol};    
    ///          }
    ///       </code>
    /// </example>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        public bool Equals(ValueObject other) =>
            other != null &&
            GetEqualityValues().SequenceEqual(other.GetEqualityValues());

        public static bool operator ==(ValueObject left, ValueObject right) => Equals(left, right);

        public static bool operator !=(ValueObject left, ValueObject right) => !(left == right);

        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj) ||
            !(obj is null) && GetType() == obj.GetType() && Equals(obj as ValueObject);

        public override int GetHashCode() =>
            unchecked(GetEqualityValues().Aggregate(17, (current, obj) => (current * 23) ^ (obj?.GetHashCode() ?? 0)));

        protected abstract IEnumerable<object> GetEqualityValues();
    }
}