using System.ComponentModel.DataAnnotations;

namespace SBShared.Models;

public class PersonModel
{
    [Required]
    public string FirstName { get; set; }=null!;
    [Required]
    public string LastName { get; set;}=null!;
}