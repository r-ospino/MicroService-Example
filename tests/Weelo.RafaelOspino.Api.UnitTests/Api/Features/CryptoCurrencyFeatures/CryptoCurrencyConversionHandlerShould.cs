using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.Conversion;
using Weelo.RafaelOspino.Domain;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.UnitTests.Api.Features.CryptoCurrencyFeatures
{
    [TestFixture]
    public class CryptoCurrencyConversionHandlerShould
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Test]
        public void Constructor_NullParams_ThrowsArgumentNullException()
        {
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(typeof(CryptoCurrencyConversionHandler).GetConstructors());
        }

        [Test]
        public async Task Handle_ValidQuery_InvokeRepositoryGet()
        {
            //Arrange
            int id = 0;
            var query = fixture.Create<CryptoCurrencyConversionQuery>();

            var repository = fixture.Freeze<Mock<IReadableRepository<CryptoCurrency>>>();
            repository.Setup(x => x.Get(It.IsAny<int>())).Returns(Task.FromResult((CryptoCurrency)null))
                .Callback<int>(x => id = x);

            var sut = fixture.Create<CryptoCurrencyConversionHandler>();

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            repository.Verify(x => x.Get(It.IsAny<int>()), Times.AtLeastOnce());
            id.Should().Be(query.Id);
        }

        [Test]
        public async Task Handle_CryptoCurrencyNotFound_ReturnFailedRequestResult()
        {
            //Arrange
            var query = fixture.Create<CryptoCurrencyConversionQuery>();

            var repository = fixture.Freeze<Mock<IReadableRepository<CryptoCurrency>>>();
            repository.Setup(x => x.Get(It.IsAny<int>())).Returns(Task.FromResult((CryptoCurrency)null));

            var sut = fixture.Create<CryptoCurrencyConversionHandler>();            

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.FailureReasons.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Handle_ValidForConversionCryptoCurrency_ReturnSuccessfulRequestResult()
        {
            //Arrange
            var query = fixture.Create<CryptoCurrencyConversionQuery>();

            var currency = fixture.Create<CryptoCurrency>();

            var repository = fixture.Freeze<Mock<IReadableRepository<CryptoCurrency>>>();
            repository.Setup(x => x.Get(It.IsAny<int>())).Returns(Task.FromResult(currency));

            var sut = fixture.Create<CryptoCurrencyConversionHandler>();

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Payload.Should().NotBeNull();
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-1)]
        public async Task Handle_InvalidForConversionCryptoCurrency_ReturnFailedRequestResult(decimal? priceUsd)
        {
            //Arrange
            var query = fixture.Create<CryptoCurrencyConversionQuery>();

            var currency = fixture.Build<CryptoCurrency>()
                .With(x => x.PriceUsd, priceUsd)
                .Create();

            var repository = fixture.Freeze<Mock<IReadableRepository<CryptoCurrency>>>();
            repository.Setup(x => x.Get(It.IsAny<int>())).Returns(Task.FromResult(currency));

            var sut = fixture.Create<CryptoCurrencyConversionHandler>();

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.FailureReasons.Should().NotBeNullOrEmpty();
        }
    }
}
