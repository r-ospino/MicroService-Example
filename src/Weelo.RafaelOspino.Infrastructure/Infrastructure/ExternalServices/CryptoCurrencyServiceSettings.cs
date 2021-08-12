namespace Weelo.RafaelOspino.Infrastructure.ExternalServices
{
    /// <summary>
    /// Settings for the CoinLore service.
    /// </summary>
    public record CryptoCurrencyServiceSettings 
    {
        /// <summary>
        /// Gets or init the CoinLore service base URL
        /// </summary>
        public string BaseUrl { get; init; }
    }
}
