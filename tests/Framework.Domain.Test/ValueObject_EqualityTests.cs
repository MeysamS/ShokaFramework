using FluentAssertions;
using Shoka.Domain.Values;

namespace Framework.Domain.Test;
public class ValueObject_EqualityTests
{
    [Fact]
    public void value_object_equlity()
    {
        var money1 = new Money(1000, "USD");
        var money2 = new Money(1000, "USD");
        money1.Equals(money2).Should().BeTrue();
    }

    private class Money : ValueObject
    {
        public long Amount { get; private set; }
        public string Currency { get; private set; }

        public Money(long amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            yield return Amount;
            yield return Currency;

        }
    }
}
