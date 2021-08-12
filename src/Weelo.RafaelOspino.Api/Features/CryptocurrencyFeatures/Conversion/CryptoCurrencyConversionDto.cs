namespace Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.Conversion
{
    /// <summary>
    /// Represents a response for a <see cref="Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.Conversion.CryptoCurrencyConversionQuery"/>
    /// </summary>
    /// <param name="From">Source currency.</param>
    /// <param name="To">Destination currency.</param>
    public record CryptoCurrencyConversionDto(CurrencyDto From, CurrencyDto To);
}
