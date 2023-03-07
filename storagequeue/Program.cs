using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .Build();

host.Start();

var config = host.Services.GetRequiredService<IConfiguration>();
var connectionString = config.GetConnectionString("Storage");
var queueName = "az204-queue";

var queueClient = new QueueClient(connectionString, queueName);

await queueClient.CreateIfNotExistsAsync();

if (queueClient.Exists())
{
    Console.WriteLine("Adding message 1");
    await queueClient.SendMessageAsync("my queue message 1");

    Console.WriteLine("Adding message 2");
    await queueClient.SendMessageAsync("my queue message 2");

    var props = await queueClient.GetPropertiesAsync();
    Console.WriteLine($"queue length: {props.Value.ApproximateMessagesCount}");

    //var peekedMessage = await queueClient.PeekMessageAsync();

    Console.WriteLine("Getting messages");

    var response = await queueClient.ReceiveMessagesAsync(10);

    var message = response.Value[0];

    Console.WriteLine("Updating message 1");
    queueClient.UpdateMessage(message.MessageId, message.PopReceipt, "my updated message", TimeSpan.FromSeconds(1));

    await Task.Delay(2000);

     var deleteMessage = response.Value[1];

    Console.WriteLine("Deleting message 2");
    await queueClient.DeleteMessageAsync(deleteMessage.MessageId, deleteMessage.PopReceipt);
}

