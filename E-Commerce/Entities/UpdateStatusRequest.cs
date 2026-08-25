using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities
{
    public class UpdateStatusRequest
    {
        [Required]
        public bool IsActive { get; set; }
    }
}
