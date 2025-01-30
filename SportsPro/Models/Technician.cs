using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SportsPro.Models
{
    public class Technician
    {
        public int TechnicianID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Phone number must be 12 characters long (including dashes)")]

        public string Phone { get; set; }

    }
}
