namespace neighbours.Models;
using Microsoft.EntityFrameworkCore;


public class NeighboursDbContext : DbContext
{
    public DbSet<Listing>? Listings { get; set; }
    public DbSet<User>? Users { get; set; }

    public string? DbPath { get; }

    public string? GetDatabaseName() {
      string? DatabaseNameArg = Environment.GetEnvironmentVariable("DATABASE_NAME");

      if( DatabaseNameArg == null)
      {
        System.Console.WriteLine(
          "DATABASE_NAME is null. Defaulting to test database."
        );
        return "neighbours_test";
      }
      else
      {
        System.Console.WriteLine(
          "Connecting to " + DatabaseNameArg
        );
        return DatabaseNameArg;
      }
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"postgres://cdogdfpmgygthu:fdd4fa092da3c54b4d7f6886aef44920ebfef27c61385c3d2117220d8da4df55@ec2-54-228-125-183.eu-west-1.compute.amazonaws.com:5432/d3etu" + GetDatabaseName());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Listing>()
        .Navigation(listing => listing.User)
        .AutoInclude();
    }
}
