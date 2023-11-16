using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using Microsoft.AspNetCore.Authorization;
using MyApp.ServiceInterface;
using ServiceStack.Mvc;

namespace MyApp.Controllers;

public class HomeController : ServiceStackController
{
    public IActionResult Index()
    {
        return View(SessionAs<CustomUserSession>());
    }

    [Authorize]
    public IActionResult Contacts()
    {
        return View();
    }

    [Authorize]
    public IActionResult Bookings()
    {
        return View();
    }

    public IActionResult AuthExamples()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize]
    public IActionResult RequiresAuth()
    {
        return View();
    }

    [Authorize(Roles = "Manager")]
    public IActionResult RequiresRole()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public IActionResult RequiresAdmin()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}