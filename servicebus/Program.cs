using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .Build();

host.Start();

var config = host.Services.GetRequiredService<IConfiguration>();

var connectionString = config.GetConnectionString("Default");

var queueName = "az204-queue";

var client = new ServiceBusClient(connectionString);

var sender = client.CreateSender(queueName);

using var messageBatch = await sender.CreateMessageBatchAsync();

for (int i = 1; i <= 3; i++)
{
    var msg = $"Message {i}";

    Console.WriteLine($"Adding message {msg} to the batch.");

    if (!messageBatch.TryAddMessage(new ServiceBusMessage(msg)))
        throw new Exception($"Exception {i} has occurred.");
}

await sender.SendMessagesAsync(messageBatch);
Console.WriteLine($"A batch of three messages has been published to the queue.");

Console.WriteLine("Follow the directions in the exercise to review the results in the Azure portal.");
Console.WriteLine("Press any key to continue");
Console.ReadKey();

var processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

try
{
    processor.ProcessMessageAsync += Processor_ProcessMessageAsync;

    processor.ProcessErrorAsync += Processor_ProcessErrorAsync;

    await processor.StartProcessingAsync();

    Console.WriteLine("Wait for a minute and then press any key to end the processing");
    Console.ReadKey();

    // stop processing 
    Console.WriteLine("\nStopping the receiver...");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
    await sender.DisposeAsync();
}

Task Processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
{
    Console.WriteLine(arg.Exception.ToString());
    return Task.CompletedTask;
}

async Task Processor_ProcessMessageAsync(ProcessMessageEventArgs arg)
{
    var body = arg.Message.Body.ToString();
    Console.WriteLine($"Received: {body}");

    await arg.CompleteMessageAsync(arg.Message);
}