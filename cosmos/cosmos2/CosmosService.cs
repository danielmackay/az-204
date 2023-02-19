using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

public class CosmosService
{
    // Replace <documentEndpoint> with the information created earlier
    private readonly string EndpointUri = "<documentEndpoint>";

    // Set variable to the Primary Key from earlier.
    private readonly string PrimaryKey = "<your primary key>";

    // The Cosmos client instance
    private CosmosClient cosmosClient;

    // The database we will create
    private Database database;

    // The container we will create.
    private Container container;

    // The names of the database and container we will create
    private string databaseId = "az204Database";
    private string containerId = "az204Container";

    public CosmosService(IOptions<CosmosConfig> config)
    {
        EndpointUri = config.Value.EndpointUri;
        ArgumentNullException.ThrowIfNullOrEmpty(EndpointUri, nameof(EndpointUri));

        PrimaryKey = config.Value.PrimaryKey;
        ArgumentNullException.ThrowIfNull(PrimaryKey, nameof(PrimaryKey));
    }

    public async Task DoCosmosAsync()
    {
        // Create a new instance of the Cosmos Client
        this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

        // Runs the CreateDatabaseAsync method
        await this.CreateDatabaseAsync();

        // Run the CreateContainerAsync method
        await this.CreateContainerAsync();
    }

    private async Task CreateDatabaseAsync()
    {
        // Create a new database using the cosmosClient
        this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        Console.WriteLine("Created Database: {0}\n", this.database.Id);
    }

    private async Task CreateContainerAsync()
    {
        // Create a new container
        this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/LastName");
        Console.WriteLine("Created Container: {0}\n", this.container.Id);
    }
}
