using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using Microsoft.AspNetCore.Authorization;
using ServiceStack.Mvc;

namespace MyApp.Controllers;

public class HomeController : ServiceStackController
{
    public IActionResult Index() => View();

    [Authorize]
    public IActionResult Bookings() => View();

    public IActionResult AuthExamples() => View();

    [Authorize]
    public IActionResult RequiresAuth() => View();

    [Authorize(Roles = "Manager")]
    public IActionResult RequiresRole() => View();

    [Authorize(Roles = "Admin")]
    public IActionResult RequiresAdmin() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}