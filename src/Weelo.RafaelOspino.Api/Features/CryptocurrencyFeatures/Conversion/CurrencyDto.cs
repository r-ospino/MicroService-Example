namespace Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.Conversion
{
    /// <summary>
    /// Represents a minimal currency info.
    /// </summary>
    /// <param name="Symbol">Currency symbol.</param>
    /// <param name="Amount">Currency amount.</param>
    public record CurrencyDto(string Symbol, decimal Amount)
    {
    }
}
