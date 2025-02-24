using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    [Route("customers")] 
  public class CustomerController : Controller
    {
        private readonly SportsProContext _context;

        public CustomerController(SportsProContext context)
        {
            _context = context;
        }

        // GET: customers
        [HttpGet("")]
        public async Task<IActionResult> List()
        {
            var sportsProContext = _context.Customers.Include(c => c.Country)
                                                     .OrderBy(c => c.LastName); //Organized by last name of customer
            return View(await sportsProContext.ToListAsync());
        }

        // GET: customers/details/5
        [HttpGet("details/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: customers/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["CountryID"] = new SelectList(
                _context.Countries, "CountryID", "Name"
            );

            return View();
        }

        // POST: customers/create
        [HttpPost("create")]
        public IActionResult Create(Customer model)
        {
            if (ModelState.IsValid)
            {
                model.Country = _context.Countries.Find(model.CountryID);
                _context.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(List));
            }

            // Reload the countries list if the form reloads
            ViewBag.Countries = _context.Countries.ToList();
            return View(model);
        }

        // GET: customers/edit/5
        [HttpGet("edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["CountryID"] = new SelectList(
                              _context.Countries.Select(c => new { c.CountryID, c.Name }), "CountryID", "Name", customer.CountryID); return View(customer);
        }

        // POST: customers/edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,FirstName,LastName,Address,City,State,PostalCode,Phone,Email,CountryID")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
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
            ViewData["CountryID"] = new SelectList(
                   _context.Countries.Select(c => new { c.CountryID, c.Name }), "CountryID", "Name", customer.CountryID); return View(customer);
        }

        // GET: customers/delete/5
        [HttpGet("delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: customers/delete/5
        [HttpPost("delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'SportsProContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.CustomerID == id)).GetValueOrDefault();
        }
    }
}
