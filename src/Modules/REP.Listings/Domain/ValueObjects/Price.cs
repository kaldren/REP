namespace REP.Listings.Domain.ValueObjects;

public record Price
{
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }

    public Price(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new ArgumentException("Price amount cannot be negative.", nameof(amount));

        Amount = amount;
        Currency = currency;
    }

    public override string ToString()
    {
        return $"{Amount} {Currency}";
    }
}