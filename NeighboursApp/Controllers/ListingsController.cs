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

    [Route("/listings")]
    [HttpGet]
    public IActionResult Index() {
      NeighboursDbContext dbContext = new NeighboursDbContext();
      List<Listing> listings = dbContext.Listings.OrderByDescending(listing => listing.DateTimePosted).ToList();
      ViewBag.Listings = listings;
      // listings.Reverse();
      return View();
    }

    // [Route("/my-profile")]
    // [HttpGet]
    // public IActionResult MyProfile() {
    //   NeighboursDbContext dbContext = new NeighboursDbContext();
    //   int user_id = HttpContext.Session.GetInt32("user_id").Value;
    //   // IQueryable<Listing> my_listing = dbContext.Posts.Where(lsiting => listing.UserId == user_id);
    //   List<Listing> listings = dbContext.Listings.Where(listing => listing.UserId == user_id).ToList();
    //   List<User> users = dbContext.Users.Where(user => user.Id == user_id).ToList();
    //   ViewBag.Listings = listings;
    //   ViewBag.Name = users.FirstOrDefault().Name;
    //   listings.Reverse();
    //   return View();
    // }

    [Route("/listings")]
    [HttpPost]
    public RedirectResult Create(Listing post) {
      int user_id = HttpContext.Session.GetInt32("user_id").Value;
      listing.UserId = user_id;
      listing.DateTimePosted = DateTime.UtcNow;
      // listing.DateTimePosted.ToString("dddd, dd MMMM yyyy hh:mm tt");
      NeighboursDbContext dbContext = new NeighboursDbContext();
      dbContext.Listings.Add(post);
      dbContext.SaveChanges();
      return new RedirectResult("/listings");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
