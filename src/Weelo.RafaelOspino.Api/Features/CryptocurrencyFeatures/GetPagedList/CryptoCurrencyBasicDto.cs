using Weelo.RafaelOspino.Domain;

namespace Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetPagedList
{
    /// <summary>
    /// Represents a response for a <see cref="GetCryptoCurrencyPagedListQuery"/>
    /// </summary>
    public record CryptoCurrencyBasicDto
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
        /// Price in USD (United States Dollar) for the Cryptocurrency.
        /// </summary>
        public decimal? PriceUsd { get; init; }

        /// <summary>
        /// Ranking of the Cryptocurrency in the whole list.
        /// </summary>
        public int Rank { get; init; }

        /// <summary>
        /// Price change rate in 24 hours for the Cryptocurrency.
        /// </summary>
        public string PercentChange24h { get; init; }

        /// <summary>
        /// Price change rate in 1 hour for the Cryptocurrency.
        /// </summary>
        public string PercentChange1h { get; init; }

        /// <summary>
        /// Price change rate in 7 days for the Cryptocurrency.
        /// </summary>
        public string PercentChange7d { get; init; }

        /// <summary>
        /// Transform <see cref="CryptoCurrency"/> entity to a <see cref="CryptoCurrencyBasicDto"/>
        /// </summary>
        /// <param name="from">Source entity</param>
        /// <returns>null if <paramref name="from"/> is null; otherwise, a <see cref="CryptoCurrencyBasicDto"/></returns>
        public static CryptoCurrencyBasicDto FromEntity(CryptoCurrency from)
        {
            if (from is null)
            {
                return null;
            }

            return new CryptoCurrencyBasicDto()
            {
                Id = from.Id,
                Symbol = from.Symbol,
                Name = from.Name,
                PriceUsd = from.PriceUsd,
                Rank = from.Rank
            };
        }
    }
}
