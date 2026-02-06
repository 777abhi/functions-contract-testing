# Functions Contract Testing: Seamless API Integration

This project demonstrates a robust pattern for testing Azure Functions with external API dependencies. It ensures seamless integration by utilizing a contract testing approach where a Mock Server simulates upstream services in CI/CD environments, while allowing connection to real public APIs (like JSONPlaceholder) in production or integration environments.

## Existing Features

The project currently implements the following features across its two main components:

### 1. Azure Function (`SimpleAzureFunction`)
*   **HttpTrigger Function**: Implements `GetPublicApiData`, an HTTP-triggered function that acts as a gateway/proxy.
*   **Environment-Aware Configuration**: Uses `API_URL` environment variable to switch between the Mock Server and the real API without code changes.
*   **Error Handling**: Robust error handling for upstream API failures (Bad Gateway) and internal exceptions (Internal Server Error).
*   **Dependency Injection**: Uses `IHttpClientFactory` for efficient HTTP connection pooling and management.
*   **Logging**: Integrated logging using `ILogger` for tracing request processing and errors.

### 2. Mock Server (`mock-server`)
*   **Express.js & TypeScript**: Built with a modern Node.js stack for type safety and performance.
*   **Health Check Endpoint**: `/health` endpoint providing server status, timestamp, and uptime.
*   **Comprehensive Data Mocking**:
    *   **Todos**: `/api/todos` serving a list of sample todo items.
    *   **Users**: Full CRUD support (`GET`, `POST`, `PUT`, `DELETE`) at `/api/users`.
    *   **Products**: `/api/products` serving e-commerce product data.
    *   **Orders**: `/api/orders` serving order transaction history.
*   **Webhook Simulation**: `/api/webhook` endpoint to receive and log POST webhooks.
*   **CORS Support**: Enabled Cross-Origin Resource Sharing for broader client compatibility.

## Future Roadmap: 20 Incremental Features

We plan to iteratively enhance the project with the following 20 features:

1.  **Get Todo by ID**: Enhance Azure Function to support fetching a single todo item by ID.
2.  **Filter Todos**: Add query parameter support to filter todos in the Azure Function proxy.
3.  **Create Todo Function**: Add a new Azure Function to create a todo item (POST).
4.  **Update Todo Function**: Add a new Azure Function to update a todo item (PUT).
5.  **Delete Todo Function**: Add a new Azure Function to delete a todo item (DELETE).
6.  **User Proxy Function**: Create an Azure Function to retrieve users from the upstream API.
7.  **Product Proxy Function**: Create an Azure Function to retrieve products.
8.  **Order Proxy Function**: Create an Azure Function to retrieve orders.
9.  **Global Exception Handler**: Implement a centralized exception handling middleware in the Azure Functions.
10. **Structured Logging**: Upgrade logging to use structured JSON format for better observability in tools like Application Insights.
11. **Resilience Policies**: Implement retry and circuit breaker patterns using Polly in the Azure Function.
12. **Response Caching**: Add in-memory caching to the Azure Function to reduce upstream calls for static data.
13. **Mock Server Latency Simulation**: Add a feature to the mock server to simulate API delays via headers or config.
14. **Mock Server Error Simulation**: Add a feature to the mock server to randomly trigger 500 errors for robustness testing.
15. **Request Validation**: Implement FluentValidation in the Azure Function to strictly validate incoming requests.
16. **OpenAPI Integration**: Integrate Swagger/OpenAPI to automatically document the Azure Functions.
17. **Key Vault Integration**: Securely store and retrieve secrets (like API keys) using Azure Key Vault.
18. **Automated Unit Tests**: Add a unit test project using xUnit and Moq to test function logic in isolation.
19. **Integration Test Suite**: Add a suite of integration tests that verify the end-to-end flow against the mock server.
20. **Containerization**: Create Dockerfiles for both the Azure Function and the Mock Server to facilitate container-based deployment.

## Setup Instructions

### Mock Server

1. Navigate to the mock server directory:
   ```bash
   cd mock-server
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Build the project:
   ```bash
   npm run build
   ```
4. Start the server:
   ```bash
   npm start
   ```
   The mock server will run at `http://localhost:3000`.

### Azure Function

1. Navigate to the function directory:
   ```bash
   cd SimpleAzureFunction
   ```
2. Configure `local.settings.json` (copy from example if needed) and set `API_URL` to `http://localhost:3000/api/todos` (or your target API).
3. Run the function app:
   ```bash
   func start
   ```

## Contribution Guidelines

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Commit your changes with clear and concise messages.
4. Submit a pull request for review.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
