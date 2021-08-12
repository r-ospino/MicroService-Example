namespace Weelo.RafaelOspino.SeedWork
{
    /// <summary>
    /// Allows defining parameters for pagination
    /// </summary>
    public interface IPagingOptions
    {
        /// <summary>
        /// Gets the page number
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Gets the number or records per page
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Gets the number of records that need to be skipped
        /// </summary>
        int Offset { get; }
    }
}