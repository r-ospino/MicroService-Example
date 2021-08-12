using System;
using System.Collections.Generic;
using System.Linq;

namespace Weelo.RafaelOspino.Commons.Mediatr
{
    /// <inheritdoc cref="IRequestResult{TResult}"/>
    /// <typeparam name="TResult" />
    public class RequestResult<TResult> : IRequestResult<TResult>
    {
        private readonly TResult payload;

        private readonly bool isSuccess;

        private readonly string[] failureReasons;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResult{TResult}"/> class 
        /// whose type argument is <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="payload">Query/Command respose</param>
        private RequestResult(TResult payload) : base()
        {
            this.payload = payload;
            isSuccess = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResult"/> class
        /// whose type argument is <typeparamref name="TResult"/>.
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

        TResult IRequestResult<TResult>.Payload => payload;

        IEnumerable<string> IRequestResult.FailureReasons => failureReasons;

        bool IRequestResult.IsSuccess => isSuccess;

        /// <summary>
        /// Creates an instance of <see cref="IRequestResult{TResult}"/> whose type argument is <typeparamref name="TResult"/>.
        /// <see cref="IRequestResult.IsSuccess"/> is false.
        /// </summary>
        /// <param name="failureReasons">Failure messages</param>
        /// <returns></returns>
        public static IRequestResult<TResult> Fail(IEnumerable<string> failureReasons)
            => new RequestResult<TResult>(failureReasons);

        /// <summary>
        /// Creates an instance of <see cref="IRequestResult{TResult}"/> whose type argument is <typeparamref name="TResult"/>.
        /// <see cref="IRequestResult.IsSuccess"/> is true.
        /// </summary>
        /// <param name="payload">Query/Command respose</param>
        /// <returns></returns>
        public static IRequestResult<TResult> Success(TResult payload)
            => new RequestResult<TResult>(payload);
    }
}
