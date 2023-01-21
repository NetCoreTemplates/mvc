#nullable enable
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Mvc;

namespace MyApp.Controllers;

public class BlogController : ServiceStackController
{
    private readonly BlogPosts _blogPosts;
    private readonly IWebHostEnvironment _env;
    public BlogController(BlogPosts blogPosts, IWebHostEnvironment env)
    {
        _blogPosts = blogPosts;
        _env = env;
    }

    public IActionResult Index(string? author = null, string? tag = null)
    {
        return View(_blogPosts.GetPosts(author, tag));
    }

    [Route("Blog/{slug}")]
    public IActionResult Post(string slug)
    {
        var doc = _blogPosts.FindPostBySlug(slug);
        if (doc != null && _env.IsDevelopment())
            doc = _blogPosts.Load(doc.Path);

        return View(doc);
    }
}
