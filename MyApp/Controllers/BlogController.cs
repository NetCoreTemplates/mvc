using Microsoft.AspNetCore.Mvc;
using ServiceStack.Mvc;

namespace MyApp.Controllers;

public class BlogController(MarkdownBlog blog, IWebHostEnvironment env) : ServiceStackController
{
    [HttpGet]
    [Route("/posts")]
    public IActionResult Index(string? author = null, string? tag = null)
    {
        return View(blog.GetPosts(author, tag));
    }

    [HttpGet]
    [Route("/posts/{slug}")]
    public IActionResult Post(string slug)
    {
        var doc = blog.FindPostBySlug(slug);
        if (doc == null)
            return NotFound();
        if (env.IsDevelopment())
            doc = blog.Load(doc.Path);

        return View(doc);
    }
}
