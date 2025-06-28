# Functions Contract Testing: Seamless API Integration Across CI/CD Environments

Test functions orchestration to ensure seamless integration with APIs. When running on CI, the mock server fulfills the responses, simulating the behavior of the actual service. When running on CD, the application connects to the actual public APIs.

## Mock Service URL

### Health Check

<http://localhost:3000/health>

### Mocked Endpoint for Todos

#### Public API Data URL

<https://jsonplaceholder.typicode.com/todos>

From mocked server: <http://localhost:3000/todos>

## Setup Instructions

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/functions-contract-testing.git
   cd functions-contract-testing
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Build the project:

   ```bash
   npm run build
   ```

   This command compiles the source code and prepares the project for execution.

4. Start the application:

   ```bash
   npm run start
   ```

   This command runs the application, connecting it to the appropriate APIs based on the environment (mocked APIs in CI or public APIs in CD).

## Testing

To run tests, use the following command:

```bash
npm test
```

Tests will verify the orchestration of functions using mocked APIs in CI and public APIs in CD environments.

## Contribution Guidelines

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Commit your changes with clear and concise messages.
4. Submit a pull request for review.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
