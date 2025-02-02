using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsPro.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Yearly price must be greater than zero.")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal YearlyPrice { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
    }
}
