namespace Weelo.RafaelOspino.Domain
{
    /// <summary>
    /// Represents a CryptoCurrency entity.
    /// </summary>
    public class CryptoCurrency
    {
        /// <summary>
        /// Gets or sets the id of the Cryptocurrency.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets 3 letters code of the Cryptocurrency.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the human readable name of the Cryptocurrency.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the normalized and unique name of the Cryptocurrency.
        /// </summary>
        public string NameId { get; set; }

        /// <summary>
        /// Gets or sets the Price in USD (United States Dollar) for the Cryptocurrency.
        /// </summary>
        public decimal? PriceUsd { get; set; }

        /// <summary>
        /// Gets or sets the price in BTC (Bitcoin) for the Cryptocurrency.
        /// </summary>
        public decimal? PriceBtc { get; set; }

        /// <summary>
        /// Gets or sets the ranking of the Cryptocurrency in the whole list.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the price change rate in 24 hours for the Cryptocurrency.
        /// </summary>
        public float PercentChange24h { get; set; }

        /// <summary>
        /// Gets or sets the price change rate in 1 hour for the Cryptocurrency.
        /// </summary>
        public float PercentChange1h { get; set; }

        /// <summary>
        /// Gets or sets the price change rate in 7 days for the Cryptocurrency.
        /// </summary>
        public float PercentChange7d { get; set; }

        /// <summary>
        /// Gets or sets the market value in dollars for the Cryptocurrency.
        /// </summary>
        /// <value>Equals to CirculatingSupply * PriceUsd.</value>
        public decimal? MarketCapUsd { get; set; }

        /// <summary>
        /// Gets or sets the amount in dollars of Cryptocurrency traded in 24 hours.
        /// </summary>
        public decimal? Volume24 { get; set; }

        /// <summary>
        /// Gets or sets the amount of Cryptocurrency traded in 24 hours.
        /// </summary>
        public decimal? Volume24Native { get; set; }

        /// <summary>
        /// Gets or sets the criculating amount of the CryptoCurrency.
        /// </summary>
        public decimal? CirculatingSupply { get; set; }

        /// <summary>
        /// Gets or sets the total existent amount of the CryptoCurrency.
        /// </summary>
        public decimal? TotalSupply { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount that can exist for the Cryptocurrency.
        /// </summary>
        public decimal? MaxSupply { get; set; }

        /// <summary>
        /// Gets a value indicating whether the CryptoCurrency has a valid conversion rate
        /// </summary>
        public bool HasValidConversionRate => PriceUsd.HasValue && PriceUsd > 0;

        /// <summary>
        /// Converts an amount from the CryptoCurrency to USD (United States Dollar).
        /// </summary>
        /// <param name="amount">The amount to be converted.</param>
        /// <exception cref="DomainException">Thron when the CryptoCurrency does not have a valid conversion rate.</exception>
        /// <returns>The amount in USD.</returns>
        public decimal ConvertToUsd(decimal amount)
        {
            EnsureCanConvert();
            return PriceUsd.Value * amount;
        }

        /// <summary>
        /// Converts an amount from USD (United States Dollar) to the CryptoCurrency.
        /// </summary>
        /// <param name="amount">The amount to be converted.</param>
        /// <exception cref="DomainException">Thron when the CryptoCurrency does not have a valid conversion rate.</exception>
        /// <returns>The amount in CryptoCurrency.</returns>
        public decimal ConvertFromUsd(decimal amount)
        {
            EnsureCanConvert();
            return amount / PriceUsd.Value;
        }

        private void EnsureCanConvert()
        {
            if (!HasValidConversionRate)
            {
                throw new DomainException($"Unable to Convert, does not have a valid conversion rate({PriceUsd})");
            }
        }

    }
}