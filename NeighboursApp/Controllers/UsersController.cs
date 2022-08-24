using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using neighbours.Models;

namespace neighbours.Controllers;

public class UsersController : Controller
{
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
    }

    [Route("/signup")]
    [HttpGet]
    public IActionResult Signup()
    { string errorInfo = HttpContext.Request.Query["error"];
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

    [Route("/users")]
    [HttpPost]
    public RedirectResult Create(User new_user) 
    {
      // IFormFile file = HttpContext.Request.Form["ImageData"];  
      // ContentRepository service = new ContentRepository();

      NeighboursDbContext dbContext = new NeighboursDbContext();
      IQueryable<User> existing_user = dbContext.Users.Where(user => user.Email == new_user.Email);
      string confirm_password = HttpContext.Request.Form["confirm-password"].ToString();
      if(existing_user.Any())
      {
        return new RedirectResult("/signup?error=existinguser");
      }
      else if(PasswordValidator.ValidatePassword(new_user.Password) == false)
        {
          return new RedirectResult("/signup?error=password");
        }
      else if(new_user.Password != confirm_password)
        {
          return new RedirectResult("/signup?error=nonmatchingpassword");
        }
      else
      {
        dbContext.Users.Add(new_user);
        dbContext.SaveChanges();
        return new RedirectResult("/signin?message=newuser");
      }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
