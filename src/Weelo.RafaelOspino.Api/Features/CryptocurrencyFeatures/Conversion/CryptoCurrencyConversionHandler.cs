using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Weelo.RafaelOspino.Commons.Mediatr;
using Weelo.RafaelOspino.Domain;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.Conversion
{
    /// <summary>
    /// Handler for a <see cref="CryptoCurrencyConversionQuery"/>
    /// </summary>
    public class CryptoCurrencyConversionHandler : IRequestHandler<CryptoCurrencyConversionQuery, IRequestResult<CryptoCurrencyConversionDto>>
    {
        private readonly IReadableRepository<CryptoCurrency> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoCurrencyConversionHandler"/> class.
        /// </summary>
        /// <param name="repository">Instace of service for retrieve CryptoCurrencies</param>
        public CryptoCurrencyConversionHandler(IReadableRepository<CryptoCurrency> repository)
        {
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Handles a <see cref="CryptoCurrencyConversionQuery"/>
        /// </summary>
        /// <param name="request">The currency conversion request</param>
        /// <param name="cancellationToken">Cancelation token</param>
        /// <returns>
        /// When execution completes successfully, <see cref="IRequestResult.IsSuccess"/> is true and 
        /// <see cref="IRequestResult{CryptoCurrencyConversionDto}.Payload" /> contains the <see cref="CryptoCurrencyConversionDto"/>. 
        /// Otherwise, <see cref="IRequestResult.IsSuccess"/> is false, and <see cref="IRequestResult.FailureReasons"/>
        /// has a collection of rule violations.
        /// </returns>
        public async Task<IRequestResult<CryptoCurrencyConversionDto>> Handle(CryptoCurrencyConversionQuery request, CancellationToken cancellationToken)
        {            
            var toCurrency = await repository.Get(request.Id);

            // If CryptoCurrency does not exists return a failed status result.
            if (toCurrency == null)
            {
                return RequestResult<CryptoCurrencyConversionDto>.Fail(new string[] { $" Does not exists a currency with the id {request.Id}." });
            }

            // If CryptoCurrency does not have a valid conversion rate return a failed status result.
            // It´s better than call CryptoCurrrency.ConvertFromUsd directly and catch the exception
            if (!toCurrency.HasValidConversionRate)
            {
                return RequestResult<CryptoCurrencyConversionDto>.Fail(new string[] { $"CrpytoCurrency with Id: {request.Id} does not have a valid conversion rate ({toCurrency.PriceUsd})" });
            }

            var toAmount = toCurrency.ConvertFromUsd(request.BaseAmount);
            var result = new CryptoCurrencyConversionDto(
                new CurrencyDto(request.BaseCurrency.ToString(), request.BaseAmount),
                new CurrencyDto(toCurrency.Symbol, toAmount));
//                request.BaseCurrency.ToString(), request.BaseAmount, toCurrency.Symbol, toAmount);

            return RequestResult<CryptoCurrencyConversionDto>.Success(result);
        }
    }
}
