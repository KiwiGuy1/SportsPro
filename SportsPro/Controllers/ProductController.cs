using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    [Route("products")]
    public class ProductController : Controller
    {
        private readonly SportsProContext _context;

        public ProductController(SportsProContext context)
        {
            _context = context;
        }

        // GET: Product
        [HttpGet("")]
        public async Task<ViewResult> List()
        {
            return _context.Products != null ?
                        View(await _context.Products.ToListAsync()) :
                        View("Entity set 'SportsProContext.Products'  is null.");
        }

        // GET: Product/Details/5
        [HttpGet("details/{id?}")]
        public async Task<ViewResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return View("NotFound");
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return View("NotFound");
            }

            return View(product);
        }

        // GET: Product/Create
        [HttpGet("create")]
        public ViewResult Create()
        {
            var model = new Product(); // This ensures ReleaseDate is set to DateTime.Now
            return View(model);
        }
        // POST: Product/Create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Create([Bind("ProductID,ProductCode,Name,YearlyPrice,ReleaseDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"{product.Name} was successfully added.";  // Store success message
                return RedirectToAction(nameof(List));  // Redirect to the List page after successful creation
            }

            // Model state is invalid, redirect back to the Create page
            TempData["ErrorMessage"] = "There was an issue with the product data. Please check and try again."; // Optionally store error message
            return RedirectToAction(nameof(Create));  // Redirect back to the Create page
        }



        // GET: Product/Edit/5
        [HttpGet("edit/{id?}")]
        public async Task<ViewResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return View("NotFound");
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return View("NotFound");
            }
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Edit(int id, [Bind("ProductID,ProductCode,Name,YearlyPrice,ReleaseDate")] Product product)
        {
            if (id != product.ProductID)
            {
                return RedirectToAction(nameof(List)); // Redirect to list if IDs don't match
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"{product.Name} successfully updated.";  // Store success message
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return RedirectToAction(nameof(List));  // Redirect if product doesn't exist anymore
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(List)); // Redirect to the List page after successful edit
            }

            // If model state is invalid, redirect back to the Edit page
            TempData["ErrorMessage"] = "There was an issue with the product data. Please check and try again.";  // Optionally store error message
            return RedirectToAction(nameof(Edit), new { id = product.ProductID });  // Redirect back to the Edit page with the product ID
        }


        // GET: Product/Delete/5
        [HttpGet("delete/{id?}")]
        public async Task<ViewResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return View("NotFound");
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return View("NotFound");
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost("delete/{id}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return RedirectToAction(nameof(List)); // If context is null, redirect to the List page
            }

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"{product.Name} successfully deleted.";  // Store success message
            }

            return RedirectToAction(nameof(List)); // Redirect to the List page after successful delete
        }

        // Helper method to check if product exists
        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductID == id)).GetValueOrDefault();
        }
    }
}

