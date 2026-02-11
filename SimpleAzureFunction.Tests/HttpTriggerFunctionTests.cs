using System.Net;
using Moq;
using Moq.Protected;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SimpleAzureFunction;
using SimpleAzureFunction.Tests.Fakes;

namespace SimpleAzureFunction.Tests
{
    public class HttpTriggerFunctionTests
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<ILogger<HttpTriggerFunction>> _mockLogger;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public HttpTriggerFunctionTests()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockLogger = new Mock<ILogger<HttpTriggerFunction>>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);
        }

        [Fact]
        public async Task Run_ReturnsInternalServerError_WhenApiUrlIsMissing()
        {
            // Arrange
            Environment.SetEnvironmentVariable("API_URL", null);
            var function = new HttpTriggerFunction(_mockHttpClientFactory.Object, _mockLogger.Object);
            var context = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(context.Object, new Uri("http://localhost"));

            // Act
            var response = await function.Run(request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            // Read body to verify message
            response.Body.Position = 0;
            using var reader = new StreamReader(response.Body);
            var body = await reader.ReadToEndAsync();
            Assert.Equal("An error occurred while processing the request", body);
        }

        [Fact]
        public async Task Run_ReturnsOk_WhenApiUrlIsSetAndUpstreamReturnsSuccess()
        {
            // Arrange
            Environment.SetEnvironmentVariable("API_URL", "http://example.com");

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"id\": 1, \"title\": \"test\"}")
                });

            var function = new HttpTriggerFunction(_mockHttpClientFactory.Object, _mockLogger.Object);
            var context = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(context.Object, new Uri("http://localhost"));

            // Act
            var response = await function.Run(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            response.Body.Position = 0;
            using var reader = new StreamReader(response.Body);
            var body = await reader.ReadToEndAsync();
            Assert.Equal("{\"id\": 1, \"title\": \"test\"}", body);
        }

        [Fact]
        public async Task Run_ReturnsBadGateway_WhenUpstreamReturnsFailure()
        {
            // Arrange
            Environment.SetEnvironmentVariable("API_URL", "http://example.com");

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var function = new HttpTriggerFunction(_mockHttpClientFactory.Object, _mockLogger.Object);
            var context = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(context.Object, new Uri("http://localhost"));

            // Act
            var response = await function.Run(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
        }
    }
}
