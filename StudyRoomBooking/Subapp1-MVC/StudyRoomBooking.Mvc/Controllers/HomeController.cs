using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    // Viser en generell feilside uten å mellomlagre svaret.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
{
    Response.StatusCode = 500;
    return View();
}
}
