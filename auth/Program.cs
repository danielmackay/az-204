using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;

var host = Host
    .CreateDefaultBuilder(args)
    .Build();

host.Start();

var config = host.Services.GetRequiredService<IConfiguration>();

string? _clientId = config.GetValue<string>("clientId");
string? _tenantId = config.GetValue<string>("tenantId");

ArgumentNullException.ThrowIfNullOrEmpty(_clientId);
ArgumentNullException.ThrowIfNullOrEmpty(_tenantId);

var app = PublicClientApplicationBuilder
    .Create(_clientId)
    .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
    .WithRedirectUri("http://localhost")
    .Build();

string[] scopes = { "user.read" };

AuthenticationResult result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();

Console.WriteLine($"Token:\t{result.AccessToken}");

Console.WriteLine("Press any key to exit");