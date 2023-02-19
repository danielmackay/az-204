using Cosmos.Template.Cli.Common;
using Cosmos.Template.Cli.Config;
using Cosmos.Template.Data.Entities;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Cosmos.Template.Cli.Commands;

public class CreateItemsCommand : AsyncCommand
{
    private readonly IOptions<CosmosConfig> _options;

    public CreateItemsCommand(IOptions<CosmosConfig> options)
    {
        _options = options;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var client = CosmosClientFactory.Create(_options);
        var db = client.GetDatabase(Constants.DbName);
        var container = db.GetContainer(Constants.ContainerName); 

        foreach (var post in GetPosts())
        {
            await container.CreateItemAsync(post);
        }

        AnsiConsole.Console.WriteJson(container);

        return 0;
    }

    private IEnumerable<Post> GetPosts()
    {

        var architectureTag = new Tag { Name = "Architecture" };
        var powershellTag = new Tag { Name = "Powershell" };
        var bicepTag = new Tag { Name = "Bicep" };
        var blazorTag = new Tag { Name = "Blazor" };
        var webTag = new Tag { Name = "Web" };
        var consoleTag = new Tag { Name = "Console" };
        var dotnetTag = new Tag { Name = ".NET" };

        int postId = 1;

        yield return new Post
        {
            postId = postId++,
            Title = "Software Diagrams - Plant UML vs Mermaid",
            Content = "Before jumping into any complex software development, it’s often a good idea to spend some time designing the system or feature you will be working on. A design is easy and quick to change. A software implementation on the other hand, is not",
            Tags = new List<Tag> { architectureTag }
        };

        yield return new Post
        {
            postId = postId++,
            Title = "Introduction to PowerShell Scripting",
            Content = "Reference article covering: installation and setup, variables, parameters, inputs/outputs, arrays, hashtables, flow control, loops, functions, debugging, error handling, filtering, sorting, projecting, formatting, and the help system.",
            Tags = new List<Tag> { powershellTag }
        };
        yield return new Post
        {
            postId = postId++,
            Title = "Bicep - Part 2: Advanced Concepts and Features",
            Content = "Deep dive into Bicep templates including expressions, template logic, and ARM template decompilation, modules and best practices.\r\n\r\n",
            Tags = new List<Tag> { bicepTag }
        };
        yield return new Post
        {
            postId = postId++,
            Title = "Clean Architecture with .NET Core: Getting Started",
            Content = "This post provides an overview of Clean Architecture and introduces the new Clean Architecture Solution Template, a .NET Core Project template for building applications based on Angular, ASP.NET Core 3.1, and Clean Architecture.",
            Tags = new List<Tag> { architectureTag, webTag, dotnetTag }
        };
        yield return new Post
        {
            postId = postId++,
            Title = "Console App Project Template for .NET 7",
            Content = "In a previous blog post Developing Console Apps with .NET, I demonstrated how to develop command-line programs from scratch including support for built-in help, arguments, configuration, logging, dependency injection, and …",
            Tags = new List<Tag> { consoleTag, dotnetTag }
        };
        yield return new Post
        {
            postId = postId++,
            Title = "Clean Architecture Solution Template for Blazor WebAssembly",
            Content = "This week I released a new solution template to support creating Blazor WebAssembly applications hosted on ASP.NET Core and following the principles of Clean Architecture. With this new template, my …",
            Tags = new List<Tag> { architectureTag, blazorTag, dotnetTag }
        };
    }
}




