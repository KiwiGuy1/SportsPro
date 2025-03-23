using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using SportsPro.ViewModels;

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
        public async Task<IActionResult> List(String filter)
        {
            var incidents = _context.Incidents.Include(i => i.Customer)
                                              .Include(i => i.Product)
                                              .Include(i => i.Technician)
                                              .OrderBy(i => i.DateOpened)
                                              .ToList();

            var viewModel = new IncidentManagerViewModel
            {
                Incidents = incidents,
                Filter = filter ?? "All"
            };

            return View(viewModel);
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
            var viewModel = new IncidentEditViewModel
            {
                Customers = _context.Customers.ToList(),
                Products = _context.Products.ToList(),
                Technicians = _context.Technicians.Where(t => t.TechnicianID != -1).ToList(),
                Operation = "Add"
            };
            return View(viewModel);
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

            var viewModel = new IncidentEditViewModel
            {
                Customers = _context.Customers.ToList(),
                Products = _context.Products.ToList(),
                Technicians = _context.Technicians.Where(t => t.TechnicianID != -1).ToList(),
                Incident = incident,
                Operation = "Add"
            };

            return View(viewModel);
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

            var viewModel = new IncidentEditViewModel
            {
                Customers = _context.Customers.ToList(),
                Products = _context.Products.ToList(),
                Technicians = _context.Technicians.Where(t => t.TechnicianID != -1).ToList(),
                Incident = incident,
                Operation = "Edit"
            };

            return View(viewModel);
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

            var viewModel = new IncidentEditViewModel
            {
                Customers = _context.Customers.ToList(),
                Products = _context.Products.ToList(),
                Technicians = _context.Technicians.Where(t => t.TechnicianID != -1).ToList(),
                Incident = incident,
                Operation = "Edit"
            };
            return View(viewModel);
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

        // GET: /techincident
        [HttpGet("techincident")]
        public async Task<IActionResult> GetTechnician()
        {
            var technicians = await _context.Technicians
                .Select(t => new SelectListItem
                {
                    Value = t.TechnicianID.ToString(),
                    Text = t.Name
                }).ToListAsync();

            var model = new TechnicianIncidentViewModel
            {
                Technicians = technicians
            };

            return View("Select", model);
        }

        // POST: /techincident/list
        [HttpPost("techincident/list")]
        public async Task<IActionResult> ListIncidents(TechnicianIncidentViewModel model)
        {
            if (!ModelState.IsValid || model.TechnicianId == null)
            {
                model.Technicians = await _context.Technicians
                    .Select(t => new SelectListItem
                    {
                        Value = t.TechnicianID.ToString(),
                        Text = t.Name
                    }).ToListAsync();

                return View("Select", model);
            }

            var technician = await _context.Technicians.FindAsync(model.TechnicianId);
            var incidents = await _context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Where(i => i.TechnicianID == model.TechnicianId && i.DateClosed == null)
                .ToListAsync();

            var viewModel = new TechnicianIncidentViewModel
            {
                TechnicianId = model.TechnicianId,
                TechnicianName = technician?.Name,
                Incidents = incidents
            };

            return View("IncidentList", viewModel);
        }


        private bool IncidentExists(int id)
        {
            return (_context.Incidents?.Any(e => e.IncidentID == id)).GetValueOrDefault();
        }
    }
}