using MediatR;
using Weelo.RafaelOspino.Commons.Mediatr;
using Weelo.RafaelOspino.Domain;

namespace Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.Conversion
{
    /// <summary>
    /// Represents a query for CrytoCurrency conversion
    /// </summary>
    /// <param name="Id">CryptoCurrency Id</param>
    /// <param name="BaseAmount">Amount to be converted</param>
    public record CryptoCurrencyConversionQuery(int Id, decimal BaseAmount) : IRequest<IRequestResult<CryptoCurrencyConversionDto>>
    {
        /// <summary>
        /// Source currency code.
        /// </summary>
        public CurrencyCode BaseCurrency { get; init; }
    }
}
