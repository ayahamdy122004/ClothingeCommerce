using System.ComponentModel.DataAnnotations;

public class ForgotPassword
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}