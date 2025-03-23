using Microsoft.AspNetCore.Mvc.Rendering;
using SportsPro.Models;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.ViewModels
{
    public class TechnicianIncidentViewModel
    {
        [Required(ErrorMessage = "Please select a technician.")]
        public int? TechnicianId { get; set; }

        public string? TechnicianName { get; set; }
        public List<SelectListItem>? Technicians { get; set; }
        public List<Incident>? Incidents { get; set; }
    }
}
