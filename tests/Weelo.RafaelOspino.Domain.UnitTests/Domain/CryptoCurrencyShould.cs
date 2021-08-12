using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using NUnit.Framework;
using System;
using Weelo.RafaelOspino.Domain;

namespace Weelo.RafaelOspino.UnitTests.Domain
{
    [TestFixture]
    public class CryptoCurrencyShould
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Test(Description = "Tests that conversion to USD is equal to multiply PriceUSD by the amount")]
        public void ConvertToUsd_AnyAmount_ReturnAmountMultiplyByPriceUsd()
        {
            // Arrange
            var sut = fixture.Create<CryptoCurrency>();
            var amount = fixture.Create<decimal>();

            // Act
            var amountResult = sut.ConvertToUsd(amount);

            // Assert
            amountResult.Should().Be(sut.PriceUsd * amount);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-1)]
        public void ConvertToUsd_InvalidPriceUsd_ThrowsException(decimal? priceUsd)
        {
            // Arrange
            var sut = fixture.Build<CryptoCurrency>()
                .With(x => x.PriceUsd, priceUsd)
                .Create();

            var amount = fixture.Create<decimal>();

            // Act
            Action act = () => sut.ConvertToUsd(amount);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Test(Description = "Tests that converting to Cryptocurrency the result of a converted to USD amount return the original amount.")]
        public void ConvertToUsd_ConvertFromUsd_ReturnOriginalAmount()
        {
            //Arrange
            var sut = fixture.Create<CryptoCurrency>();
            var amount = fixture.Create<decimal>();

            //Act
            var amountResult = sut.ConvertToUsd(amount);
            var originalAmount = sut.ConvertFromUsd(amountResult);

            //Assert
            originalAmount.Should().Be(amount);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-1)]
        public void ConvertFromUsd_InvalidPriceUsd_ThrowsException(decimal? priceUsd)
        {
            //Arrange
            var sut = fixture.Build<CryptoCurrency>()
                .With(x => x.PriceUsd, priceUsd)
                .Create();

            var amount = fixture.Create<decimal>();

            //Act
            Action act = () => sut.ConvertFromUsd(amount);

            //Assert
            act.Should().Throw<DomainException>();
        }
    }
}