using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<BlobService>();
    });

var app = builder.Build();

app.Start();

Console.WriteLine("Azure Blob Storage exercise\n");

// Run the examples asynchronously, wait for the results before proceeding
var blobService = app.Services.GetRequiredService<BlobService>();
var container = await blobService.CreateContainer();
var (blobClient, localFilePath) = await blobService.UploadBlob(container);
await blobService.ListBlobs(container);
var downloadFilePath = await blobService.DownloadBlobs(blobClient, localFilePath);
await blobService.DeleteContainer(container, localFilePath, downloadFilePath);

Console.WriteLine("Press enter to exit the sample application.");
Console.ReadLine();


public class BlobService
{
    readonly BlobServiceClient _blobServiceClient;

    public BlobService(IConfiguration config)
    {
        var storageConnectionString = config.GetConnectionString("Blob");
        ArgumentNullException.ThrowIfNull(storageConnectionString, nameof(storageConnectionString));
        _blobServiceClient = new BlobServiceClient(storageConnectionString);
    }

    public async Task<BlobContainerClient> CreateContainer()
    {
        string containerName = "wtblob" + Guid.NewGuid().ToString(); //.Replace("-", "");

        // Create the container and return a container client object
        var clientContainer = await _blobServiceClient.CreateBlobContainerAsync(containerName);
        Console.WriteLine("A container named '" + containerName + "' has been created. " +
    "\nTake a minute and verify in the portal." +
    "\nNext a file will be created and uploaded to the container.");

        Console.WriteLine("Press 'Enter' to continue.");
        Console.ReadLine();

        return clientContainer.Value;
    }

    public async Task<(BlobClient blobClient, string localFilePath)> UploadBlob(BlobContainerClient containerClient)
    {
        // Create a local file in the ./data/ directory for uploading and downloading
        string localPath = "./";
        string fileName = "wtfile" + Guid.NewGuid().ToString() + ".txt";
        string localFilePath = Path.Combine(localPath, fileName);

        // Write text to the file
        await File.WriteAllTextAsync(localFilePath, "Hello, World!");

        // Get a reference to the blob
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

        // Open the file and upload its data
        using (FileStream uploadFileStream = File.OpenRead(localFilePath))
        {
            await blobClient.UploadAsync(uploadFileStream);
            uploadFileStream.Close();
        }

        Console.WriteLine("\nThe file was uploaded. We'll verify by listing" +
                " the blobs next.");
        Console.WriteLine("Press 'Enter' to continue.");
        Console.ReadLine();

        return (blobClient, localFilePath);
    }

    public async Task ListBlobs(BlobContainerClient containerClient)
    {
        // List blobs in the container
        Console.WriteLine("Listing blobs...");
        await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
        {
            Console.WriteLine("\t" + blobItem.Name);
        }

        Console.WriteLine("\nYou can also verify by looking inside the " +
                "container in the portal." +
                "\nNext the blob will be downloaded with an altered file name.");
        Console.WriteLine("Press 'Enter' to continue.");
        Console.ReadLine();
    }

    public async Task<string> DownloadBlobs(BlobClient blobClient, string localFilePath)
    {
        // Download the blob to a local file
        // Append the string "DOWNLOADED" before the .txt extension 
        string downloadFilePath = localFilePath.Replace(".txt", "DOWNLOADED.txt");

        Console.WriteLine("\nDownloading blob to\n\t{0}\n", downloadFilePath);

        // Download the blob's contents and save it to a file
        BlobDownloadInfo download = await blobClient.DownloadAsync();

        using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
        {
            await download.Content.CopyToAsync(downloadFileStream);
        }
        Console.WriteLine("\nLocate the local file in the data directory created earlier to verify it was downloaded.");
        Console.WriteLine("The next step is to delete the container and local files.");
        Console.WriteLine("Press 'Enter' to continue.");
        Console.ReadLine();

        return downloadFilePath;
    }

    public async Task DeleteContainer(BlobContainerClient containerClient, string localFilePath, string downloadFilePath)
    {
        // Delete the container and clean up local files created
        Console.WriteLine("\n\nDeleting blob container...");
        await containerClient.DeleteAsync();

        Console.WriteLine("Deleting the local source and downloaded files...");
        File.Delete(localFilePath);
        File.Delete(downloadFilePath);

        Console.WriteLine("Finished cleaning up.");
    }
}
