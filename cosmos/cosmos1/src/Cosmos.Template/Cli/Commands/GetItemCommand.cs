using Cosmos.Template.Cli.Common;
using Cosmos.Template.Cli.Config;
using Cosmos.Template.Data.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Cosmos.Template.Cli.Commands;

public class GetItemCommand : AsyncCommand
{
    private readonly IOptions<CosmosConfig> _options;

    public GetItemCommand(IOptions<CosmosConfig> options)
    {
        _options = options;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var client = CosmosClientFactory.Create(_options);
        var db = client.GetDatabase(Constants.DbName);
        var container = db.GetContainer(Constants.ContainerName);

        var id = "1";
        var partitionKey = 1;

        var item = await container.ReadItemAsync<Post>(id, new Microsoft.Azure.Cosmos.PartitionKey(partitionKey));

        AnsiConsole.Console.WriteJson(item);

        return 0;
    }
}

public class QueryItemsCommand : AsyncCommand
{
    private readonly IOptions<CosmosConfig> _options;

    public QueryItemsCommand(IOptions<CosmosConfig> options)
    {
        _options = options;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var client = CosmosClientFactory.Create(_options);
        var db = client.GetDatabase(Constants.DbName);
        var container = db.GetContainer(Constants.ContainerName);

        var query = new QueryDefinition("select * from posts p where p.postId = @postId ")
            .WithParameter("@postId", 1);

        var results = container.GetItemQueryIterator<Post>(
            query,
            requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(1),
                MaxItemCount = 1
            });


        foreach(var result in await results.ReadNextAsync())
            AnsiConsole.Console.WriteJson(result);

        return 0;
    }
}




