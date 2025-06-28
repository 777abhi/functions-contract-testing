using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace SimpleAzureFunction
{
    public class HttpTriggerFunction
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpTriggerFunction> _logger;

        public HttpTriggerFunction(IHttpClientFactory httpClientFactory, ILogger<HttpTriggerFunction> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        [Function("GetPublicApiData")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                // Hit the JSONPlaceholder public API
                var apiUrl = Environment.GetEnvironmentVariable("API_URL");
                if (string.IsNullOrEmpty(apiUrl))
                {
                    throw new InvalidOperationException("API_URL environment variable is not set.");
                }
                var apiResponse = await _httpClient.GetAsync(apiUrl);
                
                if (!apiResponse.IsSuccessStatusCode)
                {
                    var errorResponse = req.CreateResponse(HttpStatusCode.BadGateway);
                    await errorResponse.WriteStringAsync("Failed to fetch data from public API");
                    return errorResponse;
                }

                // Read the API response
                var content = await apiResponse.Content.ReadAsStringAsync();
                
                // Create successful response
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(content);
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing request");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("An error occurred while processing the request");
                return errorResponse;
            }
        }
    }
}