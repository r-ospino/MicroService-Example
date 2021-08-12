using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.Conversion;
using Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetById;
using Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetPagedList;
using Weelo.RafaelOspino.Domain;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures
{
    /// <summary>
    /// Microservice for Cryptocurrencies.
    /// </summary>
    /// <response code="400">For invalid request params.</response>
    /// <response code="401">Invalid or unauthorized JWT token provided.</response>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<string>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [ApiController]
    public class CryptoCurrenciesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<CryptoCurrenciesController> logger;

        private const string messageError = "Ha ocurrido un error inesperado. Por favor contante al Administrador";

        /// <summary>
        /// Initializes a new instace of the <see cref="CryptoCurrenciesController"/> class.
        /// </summary>
        /// <param name="mediator">Instace of IMediatr for CQRS</param>
        /// <param name="logger">Log to write exceptions</param>
        public CryptoCurrenciesController(IMediator mediator, ILogger<CryptoCurrenciesController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Returns a paged list of cryptocurrencies basic info.
        /// </summary>
        /// <remarks>
        /// Due to a large number of existing Cryptocurrencies, this service does not return a whole list. 
        /// Instead, it returns pages of maximum of 100 Cryptocurrencies.
        /// </remarks>
        /// <param name="page">Page params.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested page.</returns>
        /// <response code="200">The requested page.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IPagedList<CryptoCurrencyBasicDto>>> Get(
            [FromQuery] GetCryptoCurrencyPagedListQuery page, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(page, cancellationToken);

                return result.IsSuccess
                    ? Ok(result.Payload)
                    : BadRequest(result.FailureReasons);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                var error = ex is DomainException || ex is InfrastructureException
                    ? ex.Message
                    : messageError;

                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }

        /// <summary>
        /// Returns detailed info for a CryptoCurrency.
        /// </summary>
        /// <param name="id">Requested CryptoCurrecy Id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the requested CryptoCurrency.</returns>
        /// <response code="200">Returns the requested CryptoCurrency.</response>
        /// <response code="404">If the CryptoCurrency does not exists.</response>  
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<ActionResult<CryptoCurrencyDetailDto>> Get(
            [FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetCryptoCurrencyByIdQuery(id), cancellationToken);
                
                return result.IsSuccess
                    ? (result.Payload is not null ?  Ok(result.Payload) : NotFound())
                    : BadRequest(result.FailureReasons);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                var error = ex is DomainException || ex is InfrastructureException
                    ? ex.Message
                    : messageError;

                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }

        /// <summary>
        /// Returns the amount of cryptocurrency equivalent to the specified conventional currency amount.
        /// </summary>
        /// <remarks>
        /// Currently only supports conversion from 'USD' (United States Dollar) currencyCode. 
        /// Any other value fallbacks to 'USD'.
        /// </remarks>
        /// <param name="id">CryptoCurrency Id.</param>
        /// <param name="currencyCode">Conventional currency code.</param>
        /// <param name="amount">Amount of conventional currency to be converted.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <response code="200">Returns the requested conversion.</response>
        [HttpGet("{id}/Conversion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CryptoCurrencyConversionDto>> GetConversion(
            [FromRoute] int id, 
            [FromQuery]CurrencyCode currencyCode, 
            [FromQuery] decimal amount,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new CryptoCurrencyConversionQuery(id, amount), cancellationToken);

                return result.IsSuccess 
                    ? Ok(result.Payload)
                    : BadRequest(result.FailureReasons);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                var error = ex is DomainException || ex is InfrastructureException
                    ? ex.Message
                    : messageError;

                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }
    }
}
