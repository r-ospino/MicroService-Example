using FluentValidation;

namespace Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.Conversion
{
    /// <summary>
    /// Validator for <see cref="CryptoCurrencyConversionQuery"/>
    /// </summary>
    public class CryptoCurrencyConversionValidator : AbstractValidator<CryptoCurrencyConversionQuery>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoCurrencyConversionValidator"/> class. 
        /// </summary>
        public CryptoCurrencyConversionValidator()
        {
            // Only 'USD' is supported
            RuleFor(x => x.BaseCurrency).Equal(Domain.CurrencyCode.USD);

            // Amount must be greater than 0.
            RuleFor(x => x.BaseAmount).GreaterThan(0);
        }
    }
}
