using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Customer
    {
		public int CustomerID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your {0}.")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your {0}.")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Street Address")]
        [Required(ErrorMessage = "Please enter your {0}.")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please enter your {0}.")]
        public string City { get; set; } = string.Empty;

        [Display(Name = "State/Province")]
        [Required(ErrorMessage = "Please enter your {0}.")]
        public string State { get; set; } = string.Empty;

        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "Please enter your {0}.")]
        public string PostalCode { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid {0}.")]
        public string? Email { get; set; }



        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please select a country.")]
        public string CountryID { get; set; } = string.Empty;         // foreign key property


        [ValidateNever]
        public Country? Country { get; set; } = null!;           // navigation property

        public string FullName => FirstName + " " + LastName;   // read-only property
	}
}