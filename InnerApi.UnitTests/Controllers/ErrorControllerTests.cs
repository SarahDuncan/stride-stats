using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using StrideStats.InnerApi.Controllers;
using System.Net;

namespace InnerApi.UnitTests.Controllers
{
    public class ErrorControllerTests
    {
        private readonly ErrorController _controller;
        private readonly Mock<IHostEnvironment> _hostEnvironment;
        private readonly IFixture _fixture;

        public ErrorControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _hostEnvironment = _fixture.Freeze<Mock<IHostEnvironment>>();
            _controller = new ErrorController();
        }

        [Fact]
        public void HandlesError_ReturnsProblem()
        {
            var result = _controller.HandleError();

            var problemResult = result.Should().BeOfType<ObjectResult>().Subject;
            problemResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void ErrorInDevelopment_ReturnsNotFound_WhenNotInDevelopment()
        {
            _hostEnvironment.Setup(h => h.EnvironmentName).Returns("Production");

            var result = _controller.ErrorInDevelopment(_hostEnvironment.Object);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void ErrorInDevelopment_ReturnsProblem_WhenInDevelopment()
        {
            _hostEnvironment.Setup(h => h.EnvironmentName).Returns("Development");
            var exception = _fixture.Create<Exception>();
            var exceptionHandlerFeatureMock = _fixture.Freeze<Mock<IExceptionHandlerFeature>>();
            exceptionHandlerFeatureMock.Setup(x => x.Error).Returns(exception);
            var context = new DefaultHttpContext();
            context.Features.Set(exceptionHandlerFeatureMock.Object);
            _controller.ControllerContext.HttpContext = context;

            var result = _controller.ErrorInDevelopment(_hostEnvironment.Object);

            var problemResult = result.Should().BeOfType<ObjectResult>().Subject;
            var problemDetails = problemResult.Value.Should().BeOfType<ProblemDetails>();
            problemResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            problemDetails.Which.Title.Should().Be(exception.Message);
        }
    }
}
