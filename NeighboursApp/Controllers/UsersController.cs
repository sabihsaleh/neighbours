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

    // [Route("/signup")]
    // [HttpGet]
    // public IActionResult New()
    // {
    //     return View();
    // }

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
        return new RedirectResult("/?error=existinguser");
      }
      else if(PasswordValidator.ValidatePassword(new_user.Password) == false)
        {
          return new RedirectResult("/?error=password");
        }
      else if(new_user.Password != confirm_password)
        {
          return new RedirectResult("/?error=nonmatchingpassword");
        }
      else
      {
        dbContext.Users.Add(new_user);
        dbContext.SaveChanges();
        return new RedirectResult("/signin");
      }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
