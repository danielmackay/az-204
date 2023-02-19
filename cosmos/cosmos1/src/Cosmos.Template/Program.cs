using Cosmos.Template.Cli.Commands;
using Cosmos.Template.Cli.Common;
using Cosmos.Template.Cli.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Reflection;

AnsiConsole.Write(new FigletText("EF Sandbox").Color(Color.Purple));
AnsiConsole.WriteLine($"Entity Framework Sandbox Command-line Tools {Assembly.GetExecutingAssembly().GetName().Version}");
AnsiConsole.WriteLine();

var builder = Host.CreateDefaultBuilder(args);

// Add services to the container
builder.ConfigureServices(services =>
{
    services.AddOptions<CosmosConfig>().BindConfiguration("Cosmos");
});

var registrar = new TypeRegistrar(builder);
var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.PropagateExceptions();

    config.AddCommand<CreateDbCommand>("create-db");
    config.AddCommand<ReadDbCommand>("read-db");
    config.AddCommand<DeleteDbCommand>("delete-db");

    config.AddCommand<CreateContainerCommand>("create-container");
    config.AddCommand<GetContainerCommand>("get-container");
    config.AddCommand<DeleteContainerCommand>("delete-container");
    
    config.AddCommand<CreateItemsCommand>("create-items");
    config.AddCommand<GetItemCommand>("get-item");
    config.AddCommand<QueryItemsCommand>("query-items");
});

try
{
    return app.Run(args);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    return -99;
}
