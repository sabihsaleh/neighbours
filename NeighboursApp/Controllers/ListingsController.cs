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
      string errorInfo = HttpContext.Request.Query["error"];
      if(errorInfo == "blanksearch")
      {
        ViewBag.Error = "You haven't entered anything to search for!";
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
      ViewBag.Listings = null;
      if(location == "" && requirements == "")
      {
        return new RedirectResult("/listings-all");
      }
      else if(requirements == "")
      {
        List<Listing> listings = dbContext.Listings.Where(listing => listing.Location.ToLower().Contains(location.ToLower()) && listing.Display == true).ToList();      
        ViewBag.Listings = listings;
        ViewBag.ListingsBool = listings.Any().ToString();
      }
      else if(location == "")
      {
        List<Listing> listings = dbContext.Listings.Where(listing => listing.Item_Service.ToLower().Contains(requirements.ToLower()) && listing.Display == true).ToList();      
        ViewBag.Listings = listings;
        ViewBag.ListingsBool = listings.Any().ToString();
      }
      else
      {
        List<Listing> listings = dbContext.Listings.Where(listing => listing.Location.ToLower().Contains(location.ToLower()) && listing.Item_Service.ToLower().Contains(requirements.ToLower()) && listing.Display == true).ToList();      
        ViewBag.Listings = listings;
        ViewBag.ListingsBool = listings.Any().ToString();
      }
      return View();
    }

    [Route("/listings-all")]
    [HttpGet]
    public IActionResult AllListings()

    {
      NeighboursDbContext dbContext = new NeighboursDbContext();
      if(HttpContext.Session.GetString("user_id") != null)
      {
          int user_id = HttpContext.Session.GetInt32("user_id").Value;
          List<User> users = dbContext.Users.Where(user => user.Id == user_id).ToList();
          ViewBag.Name = users.FirstOrDefault().Name;
      }
        List<Listing> listings = dbContext.Listings.Where(listing => listing.Display == true).ToList();      
        ViewBag.Listings = listings;
      return View();
    }


    // [Route("/my-profile")]
    // [HttpGet]
    // public IActionResult MyProfile() {
    //   NeighboursDbContext dbContext = new NeighboursDbContext();
    //   int user_id = HttpContext.Session.GetInt32("user_id").Value;
    //   List<Listing> listings = dbContext.Listings.Where(listing => listing.UserId == user_id).ToList();
    //   List<User> users = dbContext.Users.Where(user => user.Id == user_id).ToList();
    //   ViewBag.Listings = listings;
    //   ViewBag.ListingsBool = listings.Any().ToString();
    //   ViewBag.Name = users.FirstOrDefault().Name;
    //   ViewBag.User = users.FirstOrDefault();
    //   return View();
    // }

    // [Route("/user-profile")]
    // [HttpGet]
    // public IActionResult UserProfile() {
    //   NeighboursDbContext dbContext = new NeighboursDbContext();
    //   if(HttpContext.Session.GetString("user_id") != null)
    //   {
    //       int current_user_id = HttpContext.Session.GetInt32("user_id").Value;
    //       List<User> currentuser = dbContext.Users.Where(user => user.Id == current_user_id).ToList();
    //       ViewBag.Name = currentuser.FirstOrDefault().Name;
    //   }
    //   int user_id = Convert.ToInt32(HttpContext.Request.Query["user-id"]);
    //   List<Listing> user_listings = dbContext.Listings.Where(listing => listing.UserId == user_id).ToList();
    //   List<User> user = dbContext.Users.Where(user => user.Id == user_id).ToList();
    //   ViewBag.Listings = user_listings;
    //   ViewBag.User = user.FirstOrDefault();
    //   return View();
    // }

    [Route("/create-listing")]
    [HttpGet]
    public IActionResult CreateListing() {
      if(HttpContext.Session.GetString("user_id") != null)
      {
        int current_user_id = HttpContext.Session.GetInt32("user_id").Value;
        NeighboursDbContext dbContext = new NeighboursDbContext();
        List<User> currentuser = dbContext.Users.Where(user => user.Id == current_user_id).ToList();
        ViewBag.Name = currentuser.FirstOrDefault().Name;
      }
      return View();
    }

    [Route("/create-listing")]
    [HttpPost]
    public RedirectResult Create(Listing listing) {
      int user_id = HttpContext.Session.GetInt32("user_id").Value;
      listing.UserId = user_id;
      // listing.DateTimePosted = DateTime.UtcNow;
      // listing.DateTimePosted.ToString("dddd, dd MMMM yyyy hh:mm tt");
      NeighboursDbContext dbContext = new NeighboursDbContext();
      dbContext.Listings.Add(listing);
      dbContext.SaveChanges();
      return new RedirectResult("/my-profile");
    }

    [Route("/remove-listing")]
    [HttpPost]
    public RedirectResult Remove() {
      int listing_id = Convert.ToInt32(HttpContext.Request.Form["listing-id"]);
      NeighboursDbContext dbContext = new NeighboursDbContext();
      Listing currentListing = dbContext.Listings.First(listing => listing.Id == listing_id);
      currentListing.Display = false;
      dbContext.SaveChanges();
      return new RedirectResult("/my-profile");
    }

    [Route("/edit-listing")]
    [HttpGet]
    public IActionResult Edit() {
      int listing_id = Convert.ToInt32(HttpContext.Request.Query["listing-id"]);
      NeighboursDbContext dbContext = new NeighboursDbContext();
      Listing currentListing = dbContext.Listings.First(listing => listing.Id == listing_id);
      ViewBag.Listing = currentListing;
      return  View();
    }
     [Route("/edit-listing")]
    [HttpPost]
    public RedirectResult SaveEdit() {
      int listing_id = Convert.ToInt32(HttpContext.Request.Form["listing-id"]);
      NeighboursDbContext dbContext = new NeighboursDbContext();
      Listing currentListing = dbContext.Listings.First(listing => listing.Id == listing_id);
      string item_service = HttpContext.Request.Form["item_service"].ToString();
      string description = HttpContext.Request.Form["description"].ToString();
      string location = HttpContext.Request.Form["location"].ToString();
      currentListing.Item_Service = item_service;
      currentListing.Description = description;
      currentListing.Location = location;
      dbContext.SaveChanges();
      return new RedirectResult("/my-profile");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
