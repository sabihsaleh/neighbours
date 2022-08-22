using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using neighbours.Models;
using neighbours.ActionFilters;

namespace neighbours.Controllers;

[ServiceFilter(typeof(AuthenticationFilter))]
public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(ILogger<ProfileController> logger)
    {
        _logger = logger;
    }

    [Route("/my-profile")]
    [HttpGet]
    public IActionResult MyProfile() {
      NeighboursDbContext dbContext = new NeighboursDbContext();
      int user_id = HttpContext.Session.GetInt32("user_id").Value;
      List<Listing> listings = dbContext.Listings.Where(listing => listing.UserId == user_id).ToList();
      List<User> users = dbContext.Users.Where(user => user.Id == user_id).ToList();
      ViewBag.Listings = listings;
      ViewBag.ListingsBool = listings.Any().ToString();
      ViewBag.Name = users.FirstOrDefault().Name;
      ViewBag.User = users.FirstOrDefault();
      return View();
    }

    [Route("/user-profile")]
    [HttpGet]
    public IActionResult UserProfile() {
      NeighboursDbContext dbContext = new NeighboursDbContext();
      if(HttpContext.Session.GetString("user_id") != null)
      {
          int current_user_id = HttpContext.Session.GetInt32("user_id").Value;
          List<User> currentuser = dbContext.Users.Where(user => user.Id == current_user_id).ToList();
          ViewBag.Name = currentuser.FirstOrDefault().Name;
      }
      int user_id = Convert.ToInt32(HttpContext.Request.Query["user-id"]);
      List<Listing> user_listings = dbContext.Listings.Where(listing => listing.UserId == user_id).ToList();
      List<User> user = dbContext.Users.Where(user => user.Id == user_id).ToList();
      ViewBag.Listings = user_listings;
      ViewBag.User = user.FirstOrDefault();
      return View();
    }

    [Route("/edit-details")]
    [HttpGet]
    public IActionResult EditDetails() {
      NeighboursDbContext dbContext = new NeighboursDbContext();
      if(HttpContext.Session.GetString("user_id") != null)
      {
          int current_user_id = HttpContext.Session.GetInt32("user_id").Value;
          List<User> currentuser = dbContext.Users.Where(user => user.Id == current_user_id).ToList();
          ViewBag.Name = currentuser.FirstOrDefault().Name;
      }
      string postcode = HttpContext.Request.Query["postcode"];
      Console.WriteLine(postcode);
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}