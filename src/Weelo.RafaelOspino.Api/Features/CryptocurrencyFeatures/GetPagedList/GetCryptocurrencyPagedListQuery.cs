using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Weelo.RafaelOspino.Commons.Mediatr;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetPagedList
{
    /// <summary>
    /// Represents a query for get a CryptoCurrency Paged List
    /// </summary>
    public record GetCryptoCurrencyPagedListQuery : IRequest<IRequestResult<IPagedList<CryptoCurrencyBasicDto>>>, IPagingOptions
    {
        private const int minPageNumber = 1;
        private int pageNumber = 1;

        private const int minPageSize = 1;
        private const int maxPageSize = 100;
        private int pageSize = 100;

        /// <summary>
        /// Gets or inits the page number
        /// </summary>
        /// <remarks>
        /// The default is 1. On init If value is less than 1, fallbacks to 1.
        /// </remarks>
        public int PageNumber
        {
            get
            {
                return pageNumber;
            }
            init
            {
                pageNumber = value < minPageNumber ? minPageNumber : value;
            }
        }

        /// <summary>
        /// Gets or inits the number or records per page
        /// </summary>
        /// <remarks>
        /// The default is 100. 
        /// On init If value is less than 1, fallbacks to 1. If the value is greater than 100, fallbacks to 100
        /// </remarks>
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            init
            {
                pageSize = value switch
                {
                    _ when value < minPageSize => minPageSize,
                    _ when value > maxPageSize => maxPageSize,
                    _ => value
                };
            }
        }
    
        /// <summary>
        /// Gets the number of records that need to be skipped
        /// </summary>
        [BindNever]
        public int Offset => (pageNumber - 1) * pageSize;
    }
}
