using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetPagedList;
using Weelo.RafaelOspino.Domain;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.UnitTests.Api.Features.CryptoCurrencyFeatures
{
    [TestFixture]
    public class GetCryptocurrencyPagedListHandlerShould
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

            assertion.Verify(typeof(GetCryptoCurrencyPagedListHandler).GetConstructors());
        }

        [Test]
        public async Task Handle_ValidQuery_InvokeRepositoryGetPage()
        {
            //Arrange
            IPagingOptions pagingOptions = null;
            var query = fixture.Create<GetCryptoCurrencyPagedListQuery>();

            var pagedList = fixture.Create<Mock<IPagedList<CryptoCurrency>>>();

            var repository = fixture.Freeze<Mock<IReadableRepository<CryptoCurrency>>>();
            repository.Setup(x => x.GetPageAsync(It.IsAny<IPagingOptions>())).Returns(Task.FromResult(pagedList.Object))
                .Callback<IPagingOptions>(x => pagingOptions = x);

            var sut = fixture.Create<GetCryptoCurrencyPagedListHandler>();

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            repository.Verify(x => x.GetPageAsync(It.IsAny<IPagingOptions>()), Times.AtLeastOnce());
            pagingOptions.Should().Be(query);
        }

    }
}
