using System.Collections.Generic;

namespace Weelo.RafaelOspino.SeedWork
{
    /// <inheritdoc cref="IPagedList{T}"/>>
    public class PagedList<T> : IPagedList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="result">Records of the page</param>
        /// <param name="total">Total records Of the whole list</param>
        /// <param name="paging">Pagination parameters</param>
        public PagedList(IEnumerable<T> result, int total, IPagingOptions paging)
        {
            Result = result;
            Total = total;
            PageNumber = paging.PageNumber;
            PageSize = paging.PageSize;
            HasMorePages = paging.PageNumber * paging.PageSize < total;
        }

        /// <inheritdoc/>
        public IEnumerable<T> Result { get; }

        /// <inheritdoc/>
        public int PageNumber { get; }

        /// <inheritdoc/>
        public int PageSize { get; }

        /// <inheritdoc/>
        public bool HasMorePages { get; }

        /// <inheritdoc/>
        public int Total { get; }
    }

}
