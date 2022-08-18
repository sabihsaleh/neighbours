namespace neighbours.Models;
using System.ComponentModel.DataAnnotations;

public class Listing
{
  [Key]
  public int Id {get; set;}
  public string? Name {get; set;}
  public string? Description {get; set;}
  public int? Price {get; set;}
  public int UserId {get; set;}
  public User? User {get; set;}
 
}