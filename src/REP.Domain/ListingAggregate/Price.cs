namespace REP.Domain.ListingAggregate;

public record Price
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    public Price(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Price amount cannot be negative.", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be null or empty.", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpper(); // Normalize currency to uppercase
    }
    public override string ToString()
    {
        return $"{Amount} {Currency}";
    }
}