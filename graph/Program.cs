using Azure.Identity;

using Microsoft.Graph;
using Microsoft.Graph.Models;

var scopes = new[] { "User.Read" };

// Multi-tenant apps can use "common",
// single-tenant apps must use the tenant ID from the Azure portal
var tenantId = "common";

// Value from app registration
var clientId = "CLIENT_ID";

// using Azure.Identity;
var options = new TokenCredentialOptions
{
    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
};

// Callback function that receives the user prompt
// Prompt contains the generated device code that you must
// enter during the auth process in the browser
Func<DeviceCodeInfo, CancellationToken, Task> callback = (code, cancellation) => {
    Console.WriteLine(code.Message);
    return Task.FromResult(0);
};

// https://learn.microsoft.com/dotnet/api/azure.identity.devicecodecredential
var deviceCodeCredential = new DeviceCodeCredential(
    callback, tenantId, clientId, options);

var graphClient = new GraphServiceClient(deviceCodeCredential, scopes);

// GET https://graph.microsoft.com/v1.0/me

var user = await graphClient.Me
    .GetAsync();

var messages = await graphClient.Me.Messages
    .GetAsync();
    //.Select(m => new {
    //    m.Subject,
    //    m.Sender
    //})
    //.Filter("<filter condition>")
    //.OrderBy("receivedDateTime")
    //.GetAsync();

string messageId = "AQMkAGUy...";
var message = graphClient.Me.Messages[messageId].DeleteAsync();

// POST https://graph.microsoft.com/v1.0/me/calendars

var calendar = new Calendar
{
    Name = "Volunteer"
};

var newCalendar = await graphClient.Me.Calendars.PostAsync(calendar);
