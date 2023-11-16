using Microsoft.AspNetCore.Mvc;
using ServiceStack.Mvc;

namespace MyApp.Controllers;

public class DocsController(MarkdownPages markdown, IWebHostEnvironment env) : ServiceStackController
{
    [Route("/docs/{slug}")]
    public IActionResult Index(string slug)
    {
        var doc = markdown.GetBySlug(slug);
        if (doc == null)
            return NotFound();
        
        ViewData["Title"] = doc.Title;
        return View(doc);
    }
}
