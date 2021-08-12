using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetById;
using Weelo.RafaelOspino.Domain;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.UnitTests.Api.Features.CryptoCurrencyFeatures
{
    [TestFixture]
    public class GetCryptoCurrencyByIdHandlerShould
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

            assertion.Verify(typeof(GetCryptoCurrencyByIdHandler).GetConstructors());
        }

        [Test]
        public async Task Handle_ValidQuery_InvokeRepositoryGet()
        {
            //Arrange
            int id = 0;
            var query = fixture.Create<GetCryptoCurrencyByIdQuery>();

            var repository = fixture.Freeze<Mock<IReadableRepository<CryptoCurrency>>>();
            repository.Setup(x => x.Get(It.IsAny<int>())).Returns(Task.FromResult((CryptoCurrency)null))
                .Callback<int>(x => id = x);

            var sut = fixture.Create<GetCryptoCurrencyByIdHandler>();

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            repository.Verify(x => x.Get(It.IsAny<int>()), Times.AtLeastOnce());
            id.Should().Be(query.Id);
        }

        [Test]
        public async Task Handle_CryptoCurrencyNotFound_ReturnSuccessfulRequestResultWithNullPayload()
        {
            //Arrange
            var query = fixture.Create<GetCryptoCurrencyByIdQuery>();

            var repository = fixture.Freeze<Mock<IReadableRepository<CryptoCurrency>>>();
            repository.Setup(x => x.Get(It.IsAny<int>())).Returns(Task.FromResult((CryptoCurrency)null));

            var sut = fixture.Create<GetCryptoCurrencyByIdHandler>();

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Payload.Should().BeNull();
        }

        [Test]
        public async Task Handle_CryptoCurrencyFound_ReturnSuccessfulRequestResultWithNotNullPayload()
        {
            //Arrange
            var query = fixture.Create<GetCryptoCurrencyByIdQuery>();

            var currency = fixture.Create<CryptoCurrency>();

            var repository = fixture.Freeze<Mock<IReadableRepository<CryptoCurrency>>>();
            repository.Setup(x => x.Get(It.IsAny<int>())).Returns(Task.FromResult(currency));

            var sut = fixture.Create<GetCryptoCurrencyByIdHandler>();

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Payload.Should().NotBeNull()
                .And.BeEquivalentTo(currency, config => config.Excluding(x => x.HasValidConversionRate));

        }

    }
}
