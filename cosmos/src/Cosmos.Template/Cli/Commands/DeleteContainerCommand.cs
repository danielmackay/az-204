using Cosmos.Template.Cli.Common;
using Cosmos.Template.Cli.Config;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Cosmos.Template.Cli.Commands;

public class DeleteContainerCommand : AsyncCommand
{
    private readonly IOptions<CosmosConfig> _options;

    public DeleteContainerCommand(IOptions<CosmosConfig> options)
    {
        _options = options;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var client = CosmosClientFactory.Create(_options);
        var db = client.GetDatabase(Constants.DbName);
        var container = db.GetContainer(Constants.ContainerName);
        var response = await container.DeleteContainerAsync();

        AnsiConsole.Console.WriteJson(response);

        return 0;
    }
}





