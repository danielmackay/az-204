using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder();
builder.ConfigureServices((context, services) =>
{
    services.AddOptions<CosmosConfig>().Bind(context.Configuration.GetSection(CosmosConfig.Section));
    services.AddSingleton<CosmosService>();
});

var host = builder.Build();
host.Start();

try
{
    Console.WriteLine("Beginning operations...\n");
    var cosmosService = host.Services.GetRequiredService<CosmosService>();
    await cosmosService.DoCosmosAsync();
}
catch (CosmosException de)
{
    Exception baseException = de.GetBaseException();
    Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
}
catch (Exception e)
{
    Console.WriteLine("Error: {0}", e);
}
finally
{
    Console.WriteLine("End of program, press any key to exit.");
    Console.ReadKey();
}
