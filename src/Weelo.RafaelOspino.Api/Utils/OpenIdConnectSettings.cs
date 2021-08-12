namespace Weelo.RafaelOspino.Api.Utils
{
    /// <summary>
    /// Settings for the OpenId Connect Authentication
    /// </summary>
    public record OpenIdConnectSettings
    {
        /// <summary>
        /// Gets or init the Authority base URL
        /// </summary>
        public string Authority { get; init; }
    }
}