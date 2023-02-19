using System.Text;
using System.Text.Json.Serialization;

namespace Cosmos.Template.Data.Entities;

public class Post
{
    //public string id => Guid.NewGuid().ToString();
    public string id => postId.ToString();

    // Partiion Key
    public int postId { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
    public ICollection<Tag> Tags { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"PostId:{postId}");
        sb.AppendLine($"BlogId:{BlogId}");
        sb.AppendLine($"Title:{Title}");
        sb.AppendLine($"Content:{Content}");
        return sb.ToString();
    }
}
