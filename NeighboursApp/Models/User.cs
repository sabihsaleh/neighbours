namespace neighbours.Models;
using System.ComponentModel.DataAnnotations;

public class User
{
  [Key]
  public int Id {get; set;}
  public string? Name {get; set;}
  public string? Email {get; set;}
  public string? Password {get; set;}
  // public byte[]? ProfileImage {get; set;}  
  public ICollection<Listing>? Listings {get; set;}
}
