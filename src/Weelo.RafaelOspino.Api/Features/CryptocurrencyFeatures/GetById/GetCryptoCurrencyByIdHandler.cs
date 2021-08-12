using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Weelo.RafaelOspino.Commons.Mediatr;
using Weelo.RafaelOspino.Domain;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetById
{
    /// <summary>
    /// Handler for a <see cref="GetCryptoCurrencyByIdQuery"/>
    /// </summary>
    public partial class GetCryptoCurrencyByIdHandler : IRequestHandler<GetCryptoCurrencyByIdQuery, IRequestResult<CryptoCurrencyDetailDto>>
    {
        private readonly IReadableRepository<CryptoCurrency> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCryptoCurrencyByIdHandler"/> class.
        /// </summary>
        /// <param name="repository">Instace of service for retrieve CryptoCurrencies</param>
        public GetCryptoCurrencyByIdHandler(IReadableRepository<CryptoCurrency> repository)
        {
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Handles a <see cref="GetCryptoCurrencyByIdQuery"/>
        /// </summary>
        /// <param name="request">The request containig the id</param>
        /// <param name="cancellationToken">Cancelation token</param>
        /// <returns>
        /// When execution completes successfully, <see cref="IRequestResult.IsSuccess"/> is true and 
        /// <see cref="IRequestResult{CryptoCurrencyDetailDto}.Payload" /> contains the <see cref="CryptoCurrencyDetailDto"/>. 
        /// Otherwise, <see cref="IRequestResult.IsSuccess"/> is false, and <see cref="IRequestResult.FailureReasons"/>
        /// has a collection of rule violations.
        /// </returns>
        public async Task<IRequestResult<CryptoCurrencyDetailDto>> Handle(GetCryptoCurrencyByIdQuery request, CancellationToken cancellationToken)
        {
            var currency = await repository.Get(request.Id);
            var payload = CryptoCurrencyDetailDto.FromEntity(currency);

            return RequestResult<CryptoCurrencyDetailDto>.Success(payload);
        }
    }
}
