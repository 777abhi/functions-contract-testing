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

        [Function("GetTodos")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todos")] HttpRequestData req)
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

                var uriBuilder = new UriBuilder(apiUrl);
                var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                var reqQuery = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                foreach (string key in reqQuery)
                {
                    query[key] = reqQuery[key];
                }
                uriBuilder.Query = query.ToString();

                var apiResponse = await _httpClient.GetAsync(uriBuilder.ToString());
                
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

        [Function("GetPublicApiData")]
        public async Task<HttpResponseData> RunLegacy(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request (legacy endpoint).");
            return await Run(req);
        }

        [Function("CreateTodo")]
        public async Task<HttpResponseData> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "todos")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to create a todo.");

            try
            {
                var apiUrl = Environment.GetEnvironmentVariable("API_URL");
                if (string.IsNullOrEmpty(apiUrl))
                {
                    throw new InvalidOperationException("API_URL environment variable is not set.");
                }

                var requestBody = await req.ReadAsStringAsync();
                var content = new StringContent(requestBody ?? string.Empty, System.Text.Encoding.UTF8, "application/json");

                var apiResponse = await _httpClient.PostAsync(apiUrl, content);

                if (!apiResponse.IsSuccessStatusCode)
                {
                    var errorResponse = req.CreateResponse(apiResponse.StatusCode);
                    var errorContent = await apiResponse.Content.ReadAsStringAsync();
                    await errorResponse.WriteStringAsync(errorContent);
                    return errorResponse;
                }

                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var response = req.CreateResponse(apiResponse.StatusCode);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(responseContent);

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

        [Function("DeleteTodo")]
        public async Task<HttpResponseData> DeleteTodo(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "todos/{id}")] HttpRequestData req,
            string id)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to delete todo with id: {id}.");

            try
            {
                var apiUrl = Environment.GetEnvironmentVariable("API_URL");
                if (string.IsNullOrEmpty(apiUrl))
                {
                    throw new InvalidOperationException("API_URL environment variable is not set.");
                }

                var targetUrl = $"{apiUrl.TrimEnd('/')}/{id}";
                var apiResponse = await _httpClient.DeleteAsync(targetUrl);

                if (!apiResponse.IsSuccessStatusCode)
                {
                    var errorResponse = req.CreateResponse(apiResponse.StatusCode);
                    var errorContent = await apiResponse.Content.ReadAsStringAsync();
                    await errorResponse.WriteStringAsync(errorContent);
                    return errorResponse;
                }

                var response = req.CreateResponse(HttpStatusCode.OK);
                var content = await apiResponse.Content.ReadAsStringAsync();
                await response.WriteStringAsync(content);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");

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

        [Function("GetTodoById")]
        public async Task<HttpResponseData> RunById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todos/{id}")] HttpRequestData req,
            string id)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to get todo with id: {id}.");

            try
            {
                var apiUrl = Environment.GetEnvironmentVariable("API_URL");
                if (string.IsNullOrEmpty(apiUrl))
                {
                    throw new InvalidOperationException("API_URL environment variable is not set.");
                }

                var targetUrl = $"{apiUrl.TrimEnd('/')}/{id}";
                var apiResponse = await _httpClient.GetAsync(targetUrl);

                if (!apiResponse.IsSuccessStatusCode)
                {
                    var errorResponse = req.CreateResponse(apiResponse.StatusCode);
                    var errorContent = await apiResponse.Content.ReadAsStringAsync();
                    await errorResponse.WriteStringAsync(errorContent);
                    return errorResponse;
                }

                var content = await apiResponse.Content.ReadAsStringAsync();
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

        [Function("UpdateTodo")]
        public async Task<HttpResponseData> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "todos/{id}")] HttpRequestData req,
            string id)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to update todo with id: {id}.");

            try
            {
                var apiUrl = Environment.GetEnvironmentVariable("API_URL");
                if (string.IsNullOrEmpty(apiUrl))
                {
                    throw new InvalidOperationException("API_URL environment variable is not set.");
                }

                var targetUrl = $"{apiUrl.TrimEnd('/')}/{id}";
                var requestBody = await req.ReadAsStringAsync();
                var content = new StringContent(requestBody ?? string.Empty, System.Text.Encoding.UTF8, "application/json");

                var apiResponse = await _httpClient.PutAsync(targetUrl, content);

                if (!apiResponse.IsSuccessStatusCode)
                {
                    var errorResponse = req.CreateResponse(apiResponse.StatusCode);
                    var errorContent = await apiResponse.Content.ReadAsStringAsync();
                    await errorResponse.WriteStringAsync(errorContent);
                    return errorResponse;
                }

                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var response = req.CreateResponse(apiResponse.StatusCode);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(responseContent);

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