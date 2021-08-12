using System;
using System.Collections.Generic;
using System.Linq;

namespace Weelo.RafaelOspino.Commons.Mediatr
{
    /// <inheritdoc cref="IRequestResult"/>>
    public class RequestResult : IRequestResult
    {
        private readonly bool isSuccess;

        private readonly string[] failureReasons;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResult"/> class. 
        /// </summary>
        private RequestResult()
        {
            isSuccess = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResult"/> class. 
        /// </summary>
        /// <param name="failureReasons">Failure messages</param>
        private RequestResult(IEnumerable<string> failureReasons)
        {
            // Check for null parameter and remove null or empty messages an
            this.failureReasons = (failureReasons ?? throw new ArgumentNullException(nameof(failureReasons)))
                ?.Where(x => !string.IsNullOrWhiteSpace(x))
                ?.ToArray();

            if (this.failureReasons.Length == 0)
            {
                throw new ArgumentException("Must provide at least a failure reason", nameof(failureReasons));
            }

            isSuccess = false;
        }

        IEnumerable<string> IRequestResult.FailureReasons => failureReasons;

        bool IRequestResult.IsSuccess => isSuccess;

        /// <summary>
        /// Creates an instance of <see cref="IRequestResult"/>.
        /// <see cref="IRequestResult.IsSuccess"/> is false.
        /// </summary>
        /// <param name="failureReasons">Failure messages</param>
        /// <returns></returns>
        public static RequestResult Fail(IEnumerable<string> failureReasons)
            => new(failureReasons);

        /// <summary>
        /// Creates an instance of <see cref="IRequestResult"/>./>.
        /// <see cref="IRequestResult.IsSuccess"/> is true.
        /// </summary>
        /// <returns></returns>
        public static IRequestResult Success()
            => new RequestResult();
    }
}
