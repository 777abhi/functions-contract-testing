using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.Extensions; // Ensure this namespace is included

try
{
    var host = new HostBuilder()
        .ConfigureFunctionsWorkerDefaults() // Updated method for Functions Worker
        .ConfigureServices(services =>
        {
            services.AddHttpClient(); // Register HttpClient for dependency injection
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Host terminated unexpectedly: {ex.Message}");
    Environment.Exit(1);
}