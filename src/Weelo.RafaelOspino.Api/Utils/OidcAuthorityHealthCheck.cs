using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Weelo.RafaelOspino.Api.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class OidcAuthorityHealthCheck : IHealthCheck
    {
        private readonly IFlurlClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="OidcAuthorityHealthCheck"/> class
        /// </summary>
        /// <param name="flurlClientFactory">FlurlClient factory</param>
        /// /// <param name="settings">OpenId Connect Authentication service settings</param>
        public OidcAuthorityHealthCheck(IFlurlClientFactory flurlClientFactory, OpenIdConnectSettings settings)
        {
            if (flurlClientFactory is null)
            {
                throw new ArgumentNullException(nameof(flurlClientFactory));
            }

            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            client = flurlClientFactory.Get(settings.Authority);
        }

        /// <inheritdoc/>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var response = await client.Request(".well-known", "openid-configuration")
                .AllowAnyHttpStatus()
                .GetAsync();

            return response.StatusCode switch
            {
                // Assumes that with any response with status code equals to 2xx or 304, the service is available.
                (>= 200 and < 300) or 304 => HealthCheckResult.Healthy("OK"),
                _ => HealthCheckResult.Healthy("Unavailable")
            };
        }
    }
}
