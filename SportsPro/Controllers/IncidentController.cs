using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using Microsoft.Extensions.Logging;


namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        private readonly SportsProContext _context;
        private readonly ILogger<IncidentController> _logger;


        public IncidentController(SportsProContext context, ILogger<IncidentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Incidents
        public async Task<IActionResult> List()
        {
            var sportsProContext = _context.Incidents.Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician);
            return View(await sportsProContext.ToListAsync());
        }

        // GET: Incidents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Incidents == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefaultAsync(m => m.IncidentID == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // GET: Incidents/Create
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name");
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Email");
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,DateOpened,DateClosed,CustomerID,ProductID,TechnicianID")] Incident incident)
        {
            const string logMessageTemplate = "Error in IncidentController.Create: {ErrorMessage}";

            // Log submitted values
            _logger.LogInformation("Submitted Data: Title={Title}, Description={Description}, DateOpened={DateOpened}, DateClosed={DateClosed}, CustomerID={CustomerID}, ProductID={ProductID}, TechnicianID={TechnicianID}",
                incident.Title, incident.Description, incident.DateOpened, incident.DateClosed, incident.CustomerID, incident.ProductID, incident.TechnicianID);

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Model state is valid. Attempting to save incident.");
                    _context.Add(incident);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Incident saved successfully.");
                    return RedirectToAction(nameof(List));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, logMessageTemplate, ex.Message);
                }
            }
            else
            {
                _logger.LogWarning("Model state is invalid.");
            }

            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    _logger.LogError("Model state error in key '{Key}': {ErrorMessage}", key, error.ErrorMessage);
                }
            }

            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", incident.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", incident.ProductID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name", incident.TechnicianID);
            return View(incident);
        }


        // GET: Incidents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Incidents == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", incident.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", incident.ProductID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Email", incident.TechnicianID);
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IncidentID,Title,Description,DateOpened,DateClosed,CustomerID,ProductID,TechnicianID")] Incident incident)
        {
            if (id != incident.IncidentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentExists(incident.IncidentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(List));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", incident.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", incident.ProductID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Email", incident.TechnicianID);
            return View(incident);
        }

        // GET: Incidents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Incidents == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefaultAsync(m => m.IncidentID == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Incidents == null)
            {
                return Problem("Entity set 'SportsProContext.Incidents'  is null.");
            }
            var incident = await _context.Incidents.FindAsync(id);
            if (incident != null)
            {
                _context.Incidents.Remove(incident);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        private bool IncidentExists(int id)
        {
          return (_context.Incidents?.Any(e => e.IncidentID == id)).GetValueOrDefault();
        }
    }
}
