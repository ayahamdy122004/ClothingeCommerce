using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DTO.Idetity
{
 
        public class RegisterModel
        {
            [Required(ErrorMessage = "First Name is required")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last Name is required")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid Email format")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Phone Number is required")]
            [Phone(ErrorMessage = "Invalid Phone Number format")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; }

            [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
            public string ConfirmPassword { get; set; }
        }
    }

