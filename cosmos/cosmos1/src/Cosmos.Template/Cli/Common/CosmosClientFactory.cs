using Cosmos.Template.Cli.Config;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Cosmos.Template.Cli.Common;

public static class CosmosClientFactory
{
    public static CosmosClient Create(IOptions<CosmosConfig> options)
    {
        var val = options.Value;
        var client = new CosmosClient(val.Endpoint, val.Key);
        return client;
    }
}