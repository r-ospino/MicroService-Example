using System.Threading.Tasks;

namespace Weelo.RafaelOspino.SeedWork
{
    /// <summary>
    /// Represent a repository to retrieve entities of <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IReadableRepository<T>
    {
        /// <summary>
        /// Returns a <see cref="IPagedList{T}"/> whose type argument is <typeparamref name="T"/>
        /// </summary>
        /// <param name="pageOptions">Pagination parameters</param>
        /// <returns>
        /// If the page does not exist, return an empty one; 
        /// otherwise, return the requested page.
        /// </returns>
        public Task<IPagedList<T>> GetPageAsync(IPagingOptions pageOptions);

        /// <summary>
        /// Returns a <typeparamref name="T"/> for the given id.
        /// </summary>
        /// <param name="id">The id of the entity</param>
        /// <returns>
        /// If the entity does not exist, return null; otherwise, return the matching entity.
        /// </returns>
        public Task<T> Get(int id);
    }
}
