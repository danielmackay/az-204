using Cosmos.Template.Cli.Common;
using Cosmos.Template.Cli.Config;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Cosmos.Template.Cli.Commands;

public class DeleteDbCommand : AsyncCommand
{
    private readonly IOptions<CosmosConfig> _options;

    public DeleteDbCommand(IOptions<CosmosConfig> options)
    {
        _options = options;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var client = CosmosClientFactory.Create(_options);
        var db = client.GetDatabase(Constants.DbName);
        var response = await db.DeleteAsync();

        AnsiConsole.Console.WriteJson(response);

        return 0;
    }
}
