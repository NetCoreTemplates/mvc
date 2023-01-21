#nullable enable
namespace MyApp.Models.BlogViewModels;

public class PostViewModel
{
    public BlogPosts BlogPosts { get; }
    public bool Static { get; set; }
    public string? Slug { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Splash { get; set; }
    public string? AuthorProfileUrl { get; set; }
    public List<string> Tags { get; set; } = new();
    public DateTime? Date { get; set; }
    public int? WordCount { get; set; }
    public string? HtmlContent { get; set; }
}
