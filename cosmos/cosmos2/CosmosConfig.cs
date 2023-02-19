public class CosmosConfig
{
    public const string Section = "Cosmos";

    public required string EndpointUri { get; set; }
    public required string PrimaryKey { get; set; }
}