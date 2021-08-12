using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures;
using Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.Conversion;
using Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetById;
using Weelo.RafaelOspino.Api.Features.CryptocurrencyFeatures.GetPagedList;
using Weelo.RafaelOspino.Commons.Mediatr;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.UnitTests.Api.Features.CryptoCurrencyFeatures
{
    [TestFixture]
    public class CryptoCurrenciesControllerShould
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            //fixture.Customize<CryptoCurrenciesController>(c => c.FromFactory(
            //    new MethodInvoker(new GreedyConstructorQuery())));
        }

        [Test]
        public void Constructor_NullParams_ThrowsArgumentNullException()
        {
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(typeof(CryptoCurrenciesController).GetConstructors());
        }

        [Test]
        public async Task GetPage_OnRequest_InvokesMediator()
        {
            // Arrange
            IRequest<IRequestResult<IPagedList<CryptoCurrencyBasicDto>>> param = null;
            var query = fixture.Create<GetCryptoCurrencyPagedListQuery>();
            var result = fixture.Create<Mock<IRequestResult<IPagedList<CryptoCurrencyBasicDto>>>>();

            var mediator = fixture.Freeze<Mock<IMediator>>();
            mediator.Setup(x => x.Send(It.IsAny<GetCryptoCurrencyPagedListQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(result.Object))
                .Callback<IRequest<IRequestResult<IPagedList<CryptoCurrencyBasicDto>>>, CancellationToken>((x, y) => param = x);

            var sut = fixture.Build<CryptoCurrenciesController>().OmitAutoProperties().Create();

            // Act
            var response = await sut.Get(query);

            // Assert
            mediator.Verify(x => x.Send(It.IsAny<GetCryptoCurrencyPagedListQuery>(), CancellationToken.None), Times.AtLeastOnce());
            param.Should().Be(query);
        }

        [TestCase(true, HttpStatusCode.OK)]
        [TestCase(false, HttpStatusCode.BadRequest)]
        public async Task GetPage_OnIsSuccess_ReturnsStatusCode(bool isSuccess, HttpStatusCode statusCode)
        {
            // Arrange
            var query = fixture.Create<GetCryptoCurrencyPagedListQuery>();
            var result = fixture.Create<Mock<IRequestResult<IPagedList<CryptoCurrencyBasicDto>>>>();
            result.SetupGet(x => x.IsSuccess).Returns(isSuccess);

            var mediator = fixture.Freeze<Mock<IMediator>>();
            mediator.Setup(x => x.Send(It.IsAny<GetCryptoCurrencyPagedListQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(result.Object));

            var sut = fixture.Build<CryptoCurrenciesController>().OmitAutoProperties().Create();

            // Act
            var response = await sut.Get(query);

            // Assert
            (response.Result as ObjectResult).StatusCode.Should().Be((int)statusCode);
        }

        [Test]
        public async Task GetPage_OnException_ReturnsInternalServerError()
        {
            // Arrange
            var query = fixture.Create<GetCryptoCurrencyPagedListQuery>();

            var mediator = fixture.Freeze<Mock<IMediator>>();
            mediator.Setup(x => x.Send(It.IsAny<GetCryptoCurrencyPagedListQuery>(), CancellationToken.None))
                .Throws<Exception>();

            var sut = fixture.Build<CryptoCurrenciesController>().OmitAutoProperties().Create();

            // Act
            var response = await sut.Get(query);

            // Assert
            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }



        [Test]
        public async Task GetById_OnRequest_InvokesMediator()
        {
            // Arrange
            IRequest<IRequestResult<CryptoCurrencyDetailDto>> param = null;
            var query = fixture.Create<GetCryptoCurrencyByIdQuery>();
            var result = fixture.Create<Mock<IRequestResult<CryptoCurrencyDetailDto>>>();

            var mediator = fixture.Freeze<Mock<IMediator>>();
            mediator.Setup(x => x.Send(It.IsAny<GetCryptoCurrencyByIdQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(result.Object))
                .Callback<IRequest<IRequestResult<CryptoCurrencyDetailDto>>, CancellationToken>((x, y) => param = x);

            var sut = fixture.Build<CryptoCurrenciesController>().OmitAutoProperties().Create();

            // Act
            var response = await sut.Get(query.Id);

            // Assert
            mediator.Verify(x => x.Send(It.IsAny<GetCryptoCurrencyByIdQuery>(), CancellationToken.None), Times.AtLeastOnce());
            param.Should().Be(query);
        }

        [TestCase(true, HttpStatusCode.OK)]
        [TestCase(false, HttpStatusCode.BadRequest)]
        public async Task GetById_OnIsSuccess_ReturnsStatusCode(bool isSuccess, HttpStatusCode statusCode)
        {
            // Arrange
            var query = fixture.Create<GetCryptoCurrencyByIdQuery>();
            var result = fixture.Create<Mock<IRequestResult<CryptoCurrencyDetailDto>>>();
            result.SetupGet(x => x.IsSuccess).Returns(isSuccess);

            var mediator = fixture.Freeze<Mock<IMediator>>();
            mediator.Setup(x => x.Send(It.IsAny<GetCryptoCurrencyByIdQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(result.Object));

            var sut = fixture.Build<CryptoCurrenciesController>().OmitAutoProperties().Create();

            // Act
            var response = await sut.Get(query.Id);

            // Assert
            (response.Result as ObjectResult).StatusCode.Should().Be((int)statusCode);
        }

        [Test]
        public async Task GetById_OnSuccessWithNullPayload_ReturnsNotFound()
        {
            // Arrange
            var query = fixture.Create<GetCryptoCurrencyByIdQuery>();
            var result = fixture.Create<Mock<IRequestResult<CryptoCurrencyDetailDto>>>();
            result.SetupGet(x => x.IsSuccess).Returns(true);
            result.SetupGet(x => x.Payload).Returns((CryptoCurrencyDetailDto)null);

            var mediator = fixture.Freeze<Mock<IMediator>>();
            mediator.Setup(x => x.Send(It.IsAny<GetCryptoCurrencyByIdQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(result.Object));

            var sut = fixture.Build<CryptoCurrenciesController>().OmitAutoProperties().Create();

            // Act
            var response = await sut.Get(query.Id);

            // Assert
            (response.Result as StatusCodeResult).StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetById_OnException_ReturnsInternalServerError()
        {
            // Arrange
            var query = fixture.Create<GetCryptoCurrencyByIdQuery>();

            var mediator = fixture.Freeze<Mock<IMediator>>();
            mediator.Setup(x => x.Send(It.IsAny<GetCryptoCurrencyByIdQuery>(), CancellationToken.None))
                .Throws<Exception>();

            var sut = fixture.Build<CryptoCurrenciesController>().OmitAutoProperties().Create();

            // Act
            var response = await sut.Get(query.Id);

            // Assert
            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task GetConversion_OnRequest_InvokesMediator()
        {
            // Arrange
            IRequest<IRequestResult<CryptoCurrencyConversionDto>> param = null;
            var query = fixture.Create<CryptoCurrencyConversionQuery>();
            var result = fixture.Create<Mock<IRequestResult< CryptoCurrencyConversionDto>>>();

            var mediator = fixture.Freeze<Mock<IMediator>>();
            mediator.Setup(x => x.Send(It.IsAny<CryptoCurrencyConversionQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(result.Object))
                .Callback<IRequest<IRequestResult<CryptoCurrencyConversionDto>>, CancellationToken>((x, y) => param = x);

            var sut = fixture.Build<CryptoCurrenciesController>().OmitAutoProperties().Create();

            // Act
            var response = await sut.GetConversion(query.Id, query.BaseCurrency, query.BaseAmount);

            // Assert
            mediator.Verify(x => x.Send(It.IsAny<CryptoCurrencyConversionQuery>(), CancellationToken.None), Times.AtLeastOnce());
            param.Should().Be(query);
        }

        [TestCase(true, HttpStatusCode.OK)]
        [TestCase(false, HttpStatusCode.BadRequest)]
        public async Task GetConversion_OnIsSuccess_ReturnsStatusCode(bool isSuccess, HttpStatusCode statusCode)
        {
            // Arrange
            var query = fixture.Create<CryptoCurrencyConversionQuery>();
            var result = fixture.Create<Mock<IRequestResult<CryptoCurrencyConversionDto>>>();
            result.SetupGet(x => x.IsSuccess).Returns(isSuccess);
            result.SetupGet(x => x.Payload).Returns(fixture.Create< CryptoCurrencyConversionDto>());

            var mediator = fixture.Freeze<Mock<IMediator>>();
            mediator.Setup(x => x.Send(It.IsAny<CryptoCurrencyConversionQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(result.Object));

            var sut = fixture.Build<CryptoCurrenciesController>().OmitAutoProperties().Create();

            // Act
            var response = await sut.GetConversion(query.Id, query.BaseCurrency, query.BaseAmount);

            // Assert
            (response.Result as ObjectResult).StatusCode.Should().Be((int)statusCode);
        }

        [Test]
        public async Task GetConversion_OnException_ReturnsInternalServerError()
        {
            // Arrange
            var query = fixture.Create<CryptoCurrencyConversionQuery>();

            var mediator = fixture.Freeze<Mock<IMediator>>();
            mediator.Setup(x => x.Send(It.IsAny<CryptoCurrencyConversionQuery>(), CancellationToken.None))
                .Throws<Exception>();

            var sut = fixture.Build<CryptoCurrenciesController>().OmitAutoProperties().Create();

            // Act
            var response = await sut.GetConversion(query.Id, query.BaseCurrency, query.BaseAmount);

            // Assert
            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
