using System.Collections.Generic;

namespace Weelo.RafaelOspino.Commons.Mediatr
{
    /// <summary>
    /// Wrapper for Query/Command responses. Allows to include failure messages.
    /// </summary>
    /// <remarks>For empty response</remarks>
    public interface IRequestResult
    {
        /// <summary>
        /// Gets the failure reasons
        /// </summary>
        /// <remarks>
        /// Should be used to return validation messages or business rule violations
        /// </remarks>
        public IEnumerable<string> FailureReasons { get; }

        /// <summary>
        /// Gets a value indicating whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; }
    }
}
