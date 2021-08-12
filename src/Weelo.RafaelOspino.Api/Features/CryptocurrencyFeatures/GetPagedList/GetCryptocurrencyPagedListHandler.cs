using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Weelo.RafaelOspino.Commons.Mediatr;
using Weelo.RafaelOspino.Domain;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetPagedList
{
    /// <summary>
    /// Handler for a <see cref="GetCryptoCurrencyPagedListQuery"/>
    /// </summary>
    public partial class GetCryptoCurrencyPagedListHandler : IRequestHandler<GetCryptoCurrencyPagedListQuery, IRequestResult<IPagedList<CryptoCurrencyBasicDto>>>
    {
        private readonly IReadableRepository<CryptoCurrency> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCryptoCurrencyPagedListHandler"/> class.
        /// </summary>
        /// <param name="repository">Instace of service for retrieve CryptoCurrencies</param>
        public GetCryptoCurrencyPagedListHandler(IReadableRepository<CryptoCurrency> repository)
        {
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Handles a <see cref="GetCryptoCurrencyPagedListQuery"/>
        /// </summary>
        /// <param name="request">The request containig page info</param>
        /// <param name="cancellationToken">Cancelation token</param>
        /// <returns>
        /// When execution completes successfully, <see cref="IRequestResult.IsSuccess"/> is true and 
        /// <see cref="IRequestResult{CryptoCurrencyDetailDto}.Payload" /> contains the <see cref="IPagedList{CryptoCurrencyBasicDto}"/>. 
        /// Otherwise, <see cref="IRequestResult.IsSuccess"/> is false, and <see cref="IRequestResult.FailureReasons"/>
        /// has a collection of rule violations.
        /// </returns>
        public async Task<IRequestResult<IPagedList<CryptoCurrencyBasicDto>>> Handle(GetCryptoCurrencyPagedListQuery request, CancellationToken cancellationToken)
        {
            var p = await repository.GetPageAsync(request);
            var i = new PagedList<CryptoCurrencyBasicDto>(
                p.Result.Select(x => CryptoCurrencyBasicDto.FromEntity(x)), p.Total, request);
            
            return RequestResult<IPagedList<CryptoCurrencyBasicDto>>.Success(i);
        }
    }
}
