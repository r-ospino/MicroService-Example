using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Weelo.RafaelOspino.Commons.Mediatr
{
    /// <summary>
    /// Adds pre-execution validation behavior to Commands/Queries
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type. Must implement <see cref="IRequestResult"/> </typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
        where TResponse : class, IRequestResult
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="validators">Validators to be applied</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        /// <summary>
        /// Validates the request through all validators.
        /// </summary>
        /// <remarks>
        /// If validation passes, call next; otherwise, returns.
        /// </remarks>
        /// <param name="request">Incoming request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="next">Awaitable delegate for the next action in the pipeline. Eventually this delegate represents the handler.</param>
        /// <returns>Awaitable task returning the <typeparamref name="TResponse"/>.</returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // Validate request through all validators and retrieve all validation messages
            var context = new ValidationContext<TRequest>(request);
            var failures = validators
                .SelectMany(x => x.Validate(context).Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            
            if (failures.Any())
            {
                IRequestResult invalidResponse;
                var responseType = typeof(TResponse);

                if (responseType.IsGenericType)
                {                    
                    var resultType = responseType.GetGenericArguments()[0];

                    // It's necesary to use reflection to create generic instance
                    // nameof is used instead of string for compile-time error 
                    invalidResponse = typeof(RequestResult<>)
                        .GetMethod(nameof(RequestResult<object>.Fail))
                        .MakeGenericMethod(resultType)
                        .Invoke(null, new object[] { failures }) as TResponse; ;
                }
                else
                {
                    invalidResponse = RequestResult.Fail(failures);
                }

                // Break the pipeline propagation and return a failed result.
                return invalidResponse as TResponse;
            }

            var response = await next();

            return response;
        }
    }
}
