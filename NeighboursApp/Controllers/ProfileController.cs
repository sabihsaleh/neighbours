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
      return View();
    }

    [Route("/edit-details")]
    [HttpPost]
    public RedirectResult UpdateDetails() {
      NeighboursDbContext dbContext = new NeighboursDbContext();
      string address = HttpContext.Request.Form["address"].ToString();
      string phonenumber = HttpContext.Request.Form["phonenumber"].ToString();
      int current_user_id = HttpContext.Session.GetInt32("user_id").Value;
      User currentUser = dbContext.Users.First(user => user.Id == current_user_id);
      currentUser.Address = address;
      currentUser.PhoneNumber = phonenumber;
      dbContext.SaveChanges();
      // Console.WriteLine(address);
      // Console.WriteLine(phonenumber);
      return new RedirectResult("/my-profile");

    }

    [Route("/edit-details/addresses")]
    [HttpGet]
    public IActionResult SelectAddress() {
      NeighboursDbContext dbContext = new NeighboursDbContext();
      if(HttpContext.Session.GetString("user_id") != null)
      {
          int current_user_id = HttpContext.Session.GetInt32("user_id").Value;
          List<User> currentuser = dbContext.Users.Where(user => user.Id == current_user_id).ToList();
          ViewBag.Name = currentuser.FirstOrDefault().Name;
      }
      string postcode = HttpContext.Request.Query["postcode"];
      List<string> addresses = PostcodeLookup.Lookup(postcode).Result;
      ViewBag.Addresses = addresses; 
      // foreach(string address in addresses)
      // {
      //   Console.WriteLine(address);
      // }
      return View();
    }    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
