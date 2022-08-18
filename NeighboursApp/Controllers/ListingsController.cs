using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using neighbours.Models;
using neighbours.ActionFilters;

namespace neighbours.Controllers;

[ServiceFilter(typeof(AuthenticationFilter))]
public class ListingsController : Controller
{
    private readonly ILogger<ListingsController> _logger;

    public ListingsController(ILogger<ListingsController> logger)
    {
        _logger = logger;
    }

    [Route("/search")]
    [HttpGet]
    public IActionResult Search()

    {
      if(HttpContext.Session.GetString("user_id") != null)
        {
            int user_id = HttpContext.Session.GetInt32("user_id").Value;
            NeighboursDbContext dbContext = new NeighboursDbContext();
            List<User> users = dbContext.Users.Where(user => user.Id == user_id).ToList();
            ViewBag.Name = users.FirstOrDefault().Name;
        }
        return View();
    }

    [Route("/results")]
    [HttpGet]
    public IActionResult Results()

    {
      NeighboursDbContext dbContext = new NeighboursDbContext();
      if(HttpContext.Session.GetString("user_id") != null)
      {
          int user_id = HttpContext.Session.GetInt32("user_id").Value;
          List<User> users = dbContext.Users.Where(user => user.Id == user_id).ToList();
          ViewBag.Name = users.FirstOrDefault().Name;
      }
      string location = HttpContext.Request.Query["location"];
      string requirements = HttpContext.Request.Query["requirements"]; 
      ViewBag.Location = location;
      ViewBag.Requirements = requirements;
      List<Listing> listings = dbContext.Listings.Where(listing => listing.Location == location && listing.Item_Service == requirements).ToList();      
      ViewBag.Listings = listings;
      return View();
    }

    // [Route("/listings")]
    // [HttpGet]
    // public IActionResult Index() {
    //   NeighboursDbContext dbContext = new NeighboursDbContext();
    //   List<Listing> listings = dbContext.Listings.OrderByDescending(listing => listing.DateTimePosted).ToList();
    //   ViewBag.Listings = listings;
    //   // listings.Reverse();
    //   return View();
    // }

    [Route("/my-profile")]
    [HttpGet]
    public IActionResult MyProfile() {
      NeighboursDbContext dbContext = new NeighboursDbContext();
      int user_id = HttpContext.Session.GetInt32("user_id").Value;
      List<Listing> listings = dbContext.Listings.Where(listing => listing.UserId == user_id).ToList();
      List<User> users = dbContext.Users.Where(user => user.Id == user_id).ToList();
      ViewBag.Listings = listings;
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

    // [Route("/listings")]
    // [HttpPost]
    // public RedirectResult Create(Listing listing) {
    //   int user_id = HttpContext.Session.GetInt32("user_id").Value;
    //   listing.UserId = user_id;
    //   listing.DateTimePosted = DateTime.UtcNow;
    //   // listing.DateTimePosted.ToString("dddd, dd MMMM yyyy hh:mm tt");
    //   NeighboursDbContext dbContext = new NeighboursDbContext();
    //   dbContext.Listings.Add(listing);
    //   dbContext.SaveChanges();
    //   return new RedirectResult("/listings");
    // }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
