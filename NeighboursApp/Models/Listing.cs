namespace neighbours.Models;
using System.ComponentModel.DataAnnotations;

public class Listing
{
  [Key]
  public int Id {get; set;}
  public string? Item_Service {get; set;}
  public string? Description {get; set;}
  public string? Location {get; set;}
  public bool Display {get; set;} = true;
  public int UserId {get; set;}
  public User? User {get; set;}
 
}
