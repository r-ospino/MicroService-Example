using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using FluentAssertions;
using Flurl.Http.Configuration;
using Flurl.Http.Testing;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;
using Weelo.RafaelOspino.Domain;
using Weelo.RafaelOspino.Infrastructure.ExternalServices;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.UnitTests.Infrastructure.ExternalServices
{
    [TestFixture]
    public class CoinLoreTickersServiceShould
    {
        private IFixture fixture;
        private HttpTest httpTest;
        //private IFlurlClientFactory flurlClientFactory;
        //private CryptoCurrencyServiceSettings settings;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Inject<IFlurlClientFactory>(new PerBaseUrlFlurlClientFactory());
            fixture.Inject(new CryptoCurrencyServiceSettings() { BaseUrl = "https://NonExistinUrl.fake" });
            httpTest = new HttpTest();
        }

        [TearDown]
        public void TearDown()
        {
            httpTest.Dispose();
            //flurlClientFactory.Dispose();
        }

        [Test]
        public void Constructor_NullParams_ThrowsArgumentNullException()
        {
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(typeof(CoinLoreTickersService).GetConstructors());
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.ServiceUnavailable)]
        public async Task GetPage_StatusCodeIsNot2xx_ThrowsException(HttpStatusCode httpStatusCode)
        {
            // Arrange
            httpTest.RespondWith(ReasonPhrases.GetReasonPhrase((int)httpStatusCode), (int)httpStatusCode);
            var paginOptions = fixture.Create<Mock<IPagingOptions>>();
            var sut = fixture.Create<CoinLoreTickersService>();
            
            // Act
            Func<Task<IPagedList<CryptoCurrency>>> act = async () => await sut.GetPageAsync(paginOptions.Object);

            // Assert
            act.Should().Throw<InfrastructureException>();
        }

        [Test]
        public async Task GetPage_NonExistingPage_ReturnEmptyPage()
        {
            // Arrange
            var total = fixture.Create<int>();
            httpTest.RespondWith("{'data':[],'info':{'coins_num':" + total + ",'time':1628746980}}");
            var paginOptions = fixture.Create<Mock<IPagingOptions>>();
            var sut = fixture.Create<CoinLoreTickersService>();

            // Act
            var page = await sut.GetPageAsync(paginOptions.Object);

            // Assert
            page.Result.Should().BeEmpty();
            page.Total.Should().Be(total);
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.ServiceUnavailable)]
        public async Task Get_StatusCodeIsNot2xx_ThrowsException(HttpStatusCode httpStatusCode)
        {
            // Arrange
            httpTest.RespondWith(ReasonPhrases.GetReasonPhrase((int)httpStatusCode), (int)httpStatusCode);
            var sut = fixture.Create<CoinLoreTickersService>();

            // Act
            Func<Task<CryptoCurrency>> act = async () => await sut.Get(0);

            // Assert
            act.Should().Throw<InfrastructureException>();
        }

        [TestCase("[]")]
        [TestCase("")]
        public async Task Get_NonExistingCryptoCurrency_ReturnNull(string body)
        {
            // Arrange
            httpTest.RespondWith(body);
            var sut = fixture.Create<CoinLoreTickersService>();

            // Act
            var entity = await sut.Get(0);

            // Assert
            entity.Should().BeNull();
        }


        [TestCase("[{'id':'0', 'symbol':'T2T', 'name':'TraceTo', 'nameid':'traceto', 'rank':5900, 'price_usd':'?', 'percent_change_24h':'?', 'percent_change_1h':'?', 'percent_change_7d':'?', 'market_cap_usd':'?', 'volume24':'?', 'volume24_native':'?', 'csupply':'?', 'price_btc':'?', 'tsupply':'?', 'msupply':'?'}]")]
        public async Task Get_NonNumericValues_ReturnEntitiesWithNullProperties(string body)
        {
            // Arrange
            httpTest.RespondWith(body);
            var sut = fixture.Create<CoinLoreTickersService>();

            // Act
            var entity = await sut.Get(0);

            // Assert
            entity.PriceUsd.Should().BeNull();
            entity.PriceBtc.Should().BeNull();
            entity.PercentChange24h.Should().Be(0);
            entity.PercentChange1h.Should().Be(0);
            entity.PercentChange7d.Should().Be(0);
            entity.MarketCapUsd.Should().BeNull();
            entity.Volume24.Should().BeNull();
            entity.Volume24Native.Should().BeNull();
            entity.CirculatingSupply.Should().BeNull();
            entity.TotalSupply.Should().BeNull();
            entity.MaxSupply.Should().BeNull();
        }
    }
}
