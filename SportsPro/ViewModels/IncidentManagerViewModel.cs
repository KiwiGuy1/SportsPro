using SportsPro.Models;

namespace SportsPro.ViewModels
{
    public class IncidentManagerViewModel
    {
        public List<Incident> Incidents { get; set; } = new List<Incident>();
        public string Filter { get; set; } = "All"; // default to "All"
    }
}
