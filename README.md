# functions-contract-testing

Test functions orchestration that is accessing public APIs when run on CD and mocked APIs when running on CI.

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

3. Start the mock server:

   ```bash
   npm run start:mock
   ```

4. Run the application:

   ```bash
   npm start
   ```

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
