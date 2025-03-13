using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using StrideStats.InnerApi.Controllers;

namespace InnerApi.UnitTests.Controllers
{
    public class ErrorControllerTests
    {
        private readonly ErrorController _controller;
        private readonly Mock<IHostEnvironment> _hostEnvironment;

        public ErrorControllerTests()
        {
            _hostEnvironment = new Mock<IHostEnvironment>();
            _controller = new ErrorController();
        }

        [Fact]
        public void HandlesError_ReturnsProblem()
        {
            var result = _controller.HandleError();

            var problemResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, problemResult.StatusCode);
        }

        [Fact]
        public void ErrorInDevelopment_ReturnsNotFound_WhenNotInDevelopment()
        {
            _hostEnvironment.Setup(h => h.EnvironmentName).Returns("Production");

            var result = _controller.ErrorInDevelopment(_hostEnvironment.Object);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ErrorInDevelopment_ReturnsProblem_WhenInDevelopment()
        {
            _hostEnvironment.Setup(h => h.EnvironmentName).Returns("Development");
            var exceptionHandlerFeatureMock = new Mock<IExceptionHandlerFeature>();
            exceptionHandlerFeatureMock.Setup(x => x.Error).Returns(new Exception("Test exception"));
            var context = new DefaultHttpContext();
            context.Features.Set(exceptionHandlerFeatureMock.Object);
            _controller.ControllerContext.HttpContext = context;

            var result = _controller.ErrorInDevelopment(_hostEnvironment.Object);

            var problemResult = Assert.IsType<ObjectResult>(result);
            var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
            Assert.Equal(500, problemResult.StatusCode);
            Assert.Equal("Test exception", problemDetails.Title);
        }
    }
}
