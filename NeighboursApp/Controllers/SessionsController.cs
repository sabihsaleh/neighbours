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
        string message = HttpContext.Request.Query["message"];
        string errorInfo = HttpContext.Request.Query["error"];
        if(errorInfo == "invalidcredentials")
        {
          ViewBag.ErrorMessage = "The credentials you have entered do not match our records. Try again or sign up to create an account.";
        }  
        if(message == "newuser")
        {
          ViewBag.SuccessMessage = "You have signed up successfully. Please sign in.";
        }
        else if(message == "signout")
        {
          ViewBag.SuccessMessage = "You have been signed out.";
        }
        ViewBag.DisplaySignIn = false;
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
        if(user.Address == null || user.PhoneNumber == null)
        {
          return new RedirectResult("/edit-details?message=incomplete");
        }
        else 
        {
          return new RedirectResult("/search");
        }
        
        // return new RedirectResult("/");

      }
      else
      {
        return new RedirectResult("/signin?error=invalidcredentials");
      }
    }

    [Route("/signout")]
    [HttpGet]
    public RedirectResult Clear() {
      HttpContext.Session.Clear();
      return new RedirectResult("/signin?message=signout");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
