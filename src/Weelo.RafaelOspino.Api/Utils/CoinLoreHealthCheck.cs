using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;
using Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetPagedList;

namespace Weelo.RafaelOspino.Api.Utils
{
    /// <summary>
    /// Checks the status of the CoinLoreService
    /// </summary>
    /// <remarks>
    /// Because the CoinLoreService does not have a health-check endpoint, 
    /// the status is checking by retrieving a single item page.
    /// </remarks>
    public class CoinLoreHealthCheck : IHealthCheck
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoinLoreHealthCheck"/> class.
        /// </summary>
        /// <param name="mediator">Instace of IMediatr for CQRS</param>
        public CoinLoreHealthCheck(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /////// <summary>
        /////// 
        /////// </summary>
        /////// <param name="context"><inheritdoc  path="/param[@name='context']"/></param>
        /////// <param name="cancellationToken"  path="/param[@name='cancellationToken']"></param>
        /////// <returns><inheritdoc/></returns>

        /// <inheritdoc/>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var page = new GetCryptoCurrencyPagedListQuery() { PageNumber = 1, PageSize = 1 };
            var result = await mediator.Send(page, cancellationToken);

            return result.IsSuccess
                ? HealthCheckResult.Healthy("OK")
                : HealthCheckResult.Unhealthy(string.Join(',', result.FailureReasons));
        }
    }
}
