using Weelo.RafaelOspino.Domain;

namespace Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetById
{
    /// <summary>
    /// Represents a response for a <see cref="GetCryptoCurrencyByIdQuery"/>
    /// </summary>
    public record CryptoCurrencyDetailDto
    {
        /// <summary>
        /// Id of the Cryptocurrency.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// 3 letters code of the Cryptocurrency.
        /// </summary>
        public string Symbol { get; init; }

        /// <summary>
        /// Human readable name of the Cryptocurrency.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Normalize and unique name of the Cryptocurrency.
        /// </summary>
        public string NameId { get; init; }

        /// <summary>
        /// Price in USD (United States Dollar) for the Cryptocurrency.
        /// </summary>
        public decimal? PriceUsd { get; init; }

        /// <summary>
        /// Price in BTC (Bitcoin) for the Cryptocurrency.
        /// </summary>
        public decimal? PriceBtc { get; init; }

        /// <summary>
        /// Ranking of the Cryptocurrency in the whole list.
        /// </summary>
        public int Rank { get; init; }

        /// <summary>
        /// Price change rate in 24 hours for the Cryptocurrency.
        /// </summary>
        public float PercentChange24h { get; init; }

        /// <summary>
        /// Price change rate in 1 hour for the Cryptocurrency.
        /// </summary>
        public float PercentChange1h { get; init; }

        /// <summary>
        /// Price change rate in 7 days for the Cryptocurrency.
        /// </summary>
        public float PercentChange7d { get; init; }

        /// <summary>
        /// Market value in dollars for the Cryptocurrency.
        /// </summary>
        /// <value>Equals to CirculatingSupply * PriceUsd.</value>
        public decimal? MarketCapUsd { get; init; }

        /// <summary>
        /// Amount in dollars of Cryptocurrency traded in 24 hours.
        /// </summary>
        public decimal? Volume24 { get; init; }

        /// <summary>
        /// Amount of Cryptocurrency traded in 24 hours.
        /// </summary>
        public decimal? Volume24Native { get; init; }

        /// <summary>
        /// The criculating amount of the CryptoCurrency.
        /// </summary>
        public decimal? CirculatingSupply { get; init; }

        /// <summary>
        /// The total existent amount of the CryptoCurrency.
        /// </summary>
        public decimal? TotalSupply { get; init; }

        /// <summary>
        /// The maximum amount that can exist for the Cryptocurrency.
        /// </summary>
        public decimal? MaxSupply { get; init; }

        /// <summary>
        /// Transform <see cref="CryptoCurrency"/> entity to a <see cref="CryptoCurrencyDetailDto"/>
        /// </summary>
        /// <param name="from">Source entity</param>
        /// <returns>null if <paramref name="from"/> is null; otherwise, a <see cref="CryptoCurrencyDetailDto"/></returns>
        public static CryptoCurrencyDetailDto FromEntity(CryptoCurrency from)
        {
            if(from is null)
            {
                return null;
            }

            return new CryptoCurrencyDetailDto
            {
                Id = from.Id,
                Symbol = from.Symbol,
                Name = from.Name,
                NameId = from.NameId,
                PriceUsd = from.PriceUsd,
                PriceBtc = from.PriceBtc,
                Rank = from.Rank,
                PercentChange24h = from.PercentChange24h,
                PercentChange1h = from.PercentChange1h,
                PercentChange7d = from.PercentChange7d,
                MarketCapUsd = from.MarketCapUsd,
                Volume24 = from.Volume24,
                Volume24Native = from.Volume24Native,
                CirculatingSupply = from.CirculatingSupply,
                TotalSupply = from.TotalSupply,
                MaxSupply = from.MaxSupply
            };
        }
    }
}
