using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using neighbours.Models;

namespace neighbours.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Route("/")]
    [HttpGet]
    public IActionResult Index()
    {
        string errorInfo = HttpContext.Request.Query["error"];
        switch(errorInfo)
        {
            case "existinguser":
            ViewBag.Message = "An account with that email address already exists. Please try again.";
            break;
            case "password":
            ViewBag.Message = "Your password must contain at least 7 characters, a special character, and a number.";
            break;
            case "nonmatchingpassword":
            ViewBag.Message = "The passwords do not match. Please try again.";
            break;
        }
        if(HttpContext.Session.GetString("user_id") != null)
        {
            int user_id = HttpContext.Session.GetInt32("user_id").Value;
            NeighboursDbContext dbContext = new NeighboursDbContext();
            List<User> users = dbContext.Users.Where(user => user.Id == user_id).ToList();
            ViewBag.Name = users.FirstOrDefault().Name;
        }
        return View();
    }

    [Route("/privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
