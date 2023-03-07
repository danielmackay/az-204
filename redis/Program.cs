using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using StackExchange.Redis;

var host = Host.CreateDefaultBuilder().Build();

host.Start();

var config = host.Services.GetRequiredService<IConfiguration>();
var connectionString = config.GetConnectionString("Redis");
ArgumentNullException.ThrowIfNull(connectionString);

using var cache = ConnectionMultiplexer.Connect(connectionString);

IDatabase db = cache.GetDatabase();

// Snippet below executes a PING to test the server connection
var result = await db.ExecuteAsync("ping");
Console.WriteLine($"PING = {result.Type} : {result}");

// Call StringSetAsync on the IDatabase object to set the key "test:key" to the value "100"
var setValue = await db.StringSetAsync("test:key", "100");
Console.WriteLine($"SET: {setValue}");

// StringGetAsync takes the key to retrieve and return the value
var getValue = await db.StringGetAsync("test:key");
Console.WriteLine($"GET: {getValue}");
