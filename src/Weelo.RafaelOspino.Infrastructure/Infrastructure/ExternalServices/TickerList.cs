using System.Collections.Generic;
using System.Linq;
using Weelo.RafaelOspino.Domain;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.Infrastructure.ExternalServices
{

#pragma warning disable CS1591 // No documentation because it's a helper class for process CoinLore service responses.

    public class TickerList
    {
        public IList<TickerDto> Data { get; set; }
        public TickerListInfo Info { get; set; }

#pragma warning restore CS1591 // No documentation because it's a helper class for process CoinLore service responses.

        /// <summary>
        /// Transform <see cref="TickerList"/> to a <see cref="IPagedList{CryptoCurrency}"/>
        /// </summary>
        /// <param name="page">Pagination parameters</param>
        /// <returns>Returns a <see cref="IPagedList{CryptoCurrency}"/> whose type 
        /// argument is <see cref="CryptoCurrency"/></returns>
        public IPagedList<CryptoCurrency> ToCriptoCurrencyPagedList(IPagingOptions page)
        {
            var list = Data.Select(x => x.ToCryptoCurrency());
            var pagedList = new PagedList<CryptoCurrency>(list, Info.Coins_Num, page);
            return pagedList;
        }
    }
}
