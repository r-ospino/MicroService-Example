using MediatR;
using Weelo.RafaelOspino.Commons.Mediatr;

namespace Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetById
{

    /// <summary>
    /// Represents a query for get a CryptoCurrency by Id
    /// </summary>
    /// <param name="Id">CryptoCurrency Id</param>
    public record GetCryptoCurrencyByIdQuery(int Id) : IRequest<IRequestResult<CryptoCurrencyDetailDto>>;
}
