using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weelo.RafaelOspino.Domain;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.Infrastructure.ExternalServices
{
    /// <summary>
    /// Represents a repository to retrieve entities of <see cref="CryptoCurrency"/> type.
    /// </summary>
    public class CoinLoreTickersService : IReadableRepository<CryptoCurrency>
    {
        private readonly IFlurlClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoinLoreTickersService"/> class.
        /// </summary>
        /// <param name="flurlClientFactory">FlurlClient factory</param>
        /// <param name="settings">CoinLore service settings</param>
        public CoinLoreTickersService(IFlurlClientFactory flurlClientFactory, CryptoCurrencyServiceSettings settings)
        {
            if (flurlClientFactory is null)
            {
                throw new ArgumentNullException(nameof(flurlClientFactory));
            }

            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            client = flurlClientFactory.Get(settings.BaseUrl);
        }

        // To get all cryptocurrencies, multiple iteratively requests could be made and concatenate the results.
        // However, to improve time response and reduce overhead were not implemented.

        /// <summary>
        /// Returns a <see cref="IPagedList{T}"/> whose type argument is <see cref="CryptoCurrency"/>
        /// </summary>
        /// <param name="pageOptions"><inheritdoc path="/param[@name='pageOptions']"/></param>
        /// <returns><inheritdoc/></returns>
        public async Task<IPagedList<CryptoCurrency>> GetPageAsync(IPagingOptions pageOptions)
        {
            var request = client.Request("tickers")
                .SetQueryParam("start", pageOptions.Offset)
                .SetQueryParam("limit", pageOptions.PageSize);

            TickerList content;

            try
            {
                content = await request.GetJsonAsync<TickerList>();
            }
            catch (FlurlHttpTimeoutException ex)
            {
                throw new InfrastructureException("CoinLore service did not complete the request before the timeout period. ()", ex);
            }
            catch (FlurlHttpException ex)
            {
                throw new InfrastructureException("CoinLore service is unavailable or is unable to process the request at this moment", ex);
            }

            var result = content.ToCriptoCurrencyPagedList(pageOptions);
            return result;
        }

        /// <summary>
        /// Returns an <see cref="CryptoCurrency"/> for the given id.
        /// </summary>
        /// <param name="id"><inheritdoc path="/param[@name='id']"/></param>
        /// <returns><inheritdoc/></returns>
        public async Task<CryptoCurrency> Get(int id)
        {
            var request = client.Request("ticker")
                .SetQueryParam("id", id);

            IEnumerable<TickerDto> content;

            try
            {
                content = await request.GetJsonAsync<IEnumerable<TickerDto>>();
            }
            catch (FlurlHttpTimeoutException ex)
            {
                throw new InfrastructureException("CoinLore service did not complete the request before the timeout period. ()", ex);
            }
            catch (FlurlHttpException ex)
            {
                throw new InfrastructureException("CoinLore service is unavailable or is unable to process the request at this moment", ex);
            }

            var result = content?.FirstOrDefault()?.ToCryptoCurrency();
            return result;
        }
    }
}
