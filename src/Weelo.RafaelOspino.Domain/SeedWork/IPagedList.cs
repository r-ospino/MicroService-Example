using System.Collections.Generic;

namespace Weelo.RafaelOspino.SeedWork
{
    /// <summary>
    /// Represents a paged list whose type argument is <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Record type</typeparam>
    public interface IPagedList<T>
    {
        /// <summary>
        /// Gets a value indicating whether there are more pages after this page.
        /// </summary>
        bool HasMorePages { get; }

        /// <summary>
        /// Gets the page number
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Gets the number or records per page
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Gets the total records of the whole list
        /// </summary>
        int Total { get; }

        /// <summary>
        /// Gets the records of the page
        /// </summary>
        IEnumerable<T> Result { get; }
    }
}