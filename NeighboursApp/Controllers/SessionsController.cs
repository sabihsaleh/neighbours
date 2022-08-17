using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using neighbours.Models;

namespace neighbours.Controllers;

public class SessionsController : Controller
{
    private readonly ILogger<SessionsController> _logger;

    public SessionsController(ILogger<SessionsController> logger)
    {
        _logger = logger;
    }

    [Route("/signin")]
    [HttpGet]
    public IActionResult New()
    {
        return View();
    }

    [Route("/signin")]
    [HttpPost]
    public RedirectResult Create(string email, string password) {
      NeighboursDbContext dbContext = new NeighboursDbContext();
      User? user = dbContext.Users.Where(user => user.Email == email).SingleOrDefault();
      if(user != null && user.Password == password)
      {
        HttpContext.Session.SetInt32("user_id", user.Id);
        return new RedirectResult("/search");
      }
      else
      {
        return new RedirectResult("/signin-error");
      }
    }

    [Route("/signout")]
    [HttpGet]
    public IActionResult Clear() {
      HttpContext.Session.Clear();
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}