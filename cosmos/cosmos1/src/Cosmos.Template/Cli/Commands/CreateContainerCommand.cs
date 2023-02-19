using Cosmos.Template.Cli.Common;
using Cosmos.Template.Cli.Config;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Cosmos.Template.Cli.Commands;

public class CreateContainerCommand : AsyncCommand
{
    private readonly IOptions<CosmosConfig> _options;

    public CreateContainerCommand(IOptions<CosmosConfig> options)
    {
        _options = options;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var client = CosmosClientFactory.Create(_options);
        var db = client.GetDatabase(Constants.DbName);
        var container = await db.CreateContainerIfNotExistsAsync(Constants.ContainerName, "/postId");

        AnsiConsole.Console.WriteJson(container);

        return 0;
    }
}




