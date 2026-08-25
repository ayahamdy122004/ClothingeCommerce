using System.ComponentModel.DataAnnotations;

public class UpdateProfile
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }
}