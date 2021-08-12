using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using Weelo.RafaelOspino.Domain;

namespace Weelo.RafaelOspino.Infrastructure.ExternalServices
{
#pragma warning disable CS1591 // No documentation because it's a helper class for process CoinLore service responses.
    public class TickerDto
    {
        public string Id { get; set; }

        public string Symbol { get; set; }

        public string Name { get; set; }

        public string Nameid { get; set; }

        public int Rank { get; set; }

        public string Price_usd { get; set; }

        public string Percent_change_24h { get; set; }

        public string Percent_change_1h { get; set; }

        public string Percent_change_7d { get; set; }

        public string Market_cap_usd { get; set; }

        public string Volume24 { get; set; }

        public string Volume24_native { get; set; }


        [JsonProperty("volume24a")]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", 
            Justification = "It's necessary for resolving the gap between property names 'Volume24_native' and 'Volume24a' returned by ticker and tickers services respectively.")]
        private string Volume24a { set { Volume24_native = value; } }

        public string Csupply { get; set; }

        public string Price_btc { get; set; }

        public string Tsupply { get; set; }

        public string Msupply { get; set; }

#pragma warning restore CS1591 // No documentation because it's a helper class for process CoinLore service responses.

        /// <summary>
        /// Transform <see cref="TickerDto"/> to a <see cref="CryptoCurrency"/> entity.
        /// </summary>
        /// <returns>Returns a <see cref="CryptoCurrency"/></returns>
        public CryptoCurrency ToCryptoCurrency()
        {
            return new CryptoCurrency()
            {
                Id = int.Parse(Id),
                Symbol = Symbol,
                Name = Name,
                NameId = Nameid,
                PriceUsd = decimal.TryParse(Price_usd, out var priceUsd) ? priceUsd : null,
                PriceBtc = decimal.TryParse(Price_usd, out var priceBtc) ? priceBtc : null,
                Rank = Rank,
                PercentChange24h = float.TryParse(Percent_change_24h, out var percentChange24h) ? percentChange24h : 0,
                PercentChange1h = float.TryParse(Percent_change_1h, out var percentChange1h) ? percentChange1h : 0,
                PercentChange7d = float.TryParse(Percent_change_7d, out var percentChange7d) ? percentChange7d : 0,
                MarketCapUsd = decimal.TryParse(Market_cap_usd, out var marketCapUsd) ? marketCapUsd : null,
                Volume24 = decimal.TryParse(Volume24, out var volume24) ? volume24 : null,
                Volume24Native = decimal.TryParse(Volume24_native, out var volume24Native) ? volume24Native : null,
                CirculatingSupply = decimal.TryParse(Csupply, out var csupply) ? csupply : null,
                TotalSupply = decimal.TryParse(Tsupply, out var tsupply) ? tsupply : null,
                MaxSupply = decimal.TryParse(Msupply, out var msupply) ? msupply : null,
            };
        }
    }

}
