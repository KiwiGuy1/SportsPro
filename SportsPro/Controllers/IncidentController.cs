using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    [Route("incidents")]
    public class IncidentController : Controller
    {
        private readonly SportsProContext _context;

        public IncidentController(SportsProContext context)
        {
            _context = context;
        }

        // GET: Incidents
        [HttpGet("")]
        public async Task<IActionResult> List()
        {
            var sportsProContext = _context.Incidents.Include(i => i.Customer)
                                                    .Include(i => i.Product)
                                                    .Include(i => i.Technician)
                                                    .OrderBy(i => i.DateOpened);
            return View(await sportsProContext.ToListAsync());
        }

        // GET: Incidents/Details/5
        [HttpGet("details/{id?}")]
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
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name");
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Email");
            return View();
        }

        // POST: Incidents/Create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,DateOpened,DateClosed,CustomerID,ProductID,TechnicianID")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incident);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }

            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", incident.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", incident.ProductID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Email", incident.TechnicianID);
            return View(incident);
        }

        // GET: Incidents/Edit/5
        [HttpGet("edit/{id?}")]
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
        [HttpPost("edit/{id}")]
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
        [HttpGet("delete/{id?}")]
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
        [HttpPost("delete/{id}")]
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