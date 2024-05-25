using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recipes.Models;

namespace Recipes.Controllers
{
    public class TetstimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TetstimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Tetstimonials
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Tetstimonials.Include(t => t.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Tetstimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Tetstimonials == null)
            {
                return NotFound();
            }

            var tetstimonial = await _context.Tetstimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tetstimonial == null)
            {
                return NotFound();
            }

            return View(tetstimonial);
        }

        // GET: Tetstimonials/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Tetstimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,CreatedDate,TestimonialText,UserId")] Tetstimonial tetstimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tetstimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", tetstimonial.UserId);
            return View(tetstimonial);
        }

        // GET: Tetstimonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Tetstimonials == null)
            {
                return NotFound();
            }

            var tetstimonial = await _context.Tetstimonials.FindAsync(id);
            if (tetstimonial == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", tetstimonial.UserId);
            return View(tetstimonial);
        }

        // POST: Tetstimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Status,CreatedDate,TestimonialText,UserId")] Tetstimonial tetstimonial)
        {
            if (id != tetstimonial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tetstimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TetstimonialExists(tetstimonial.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", tetstimonial.UserId);
            return View(tetstimonial);
        }

        // GET: Tetstimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Tetstimonials == null)
            {
                return NotFound();
            }

            var tetstimonial = await _context.Tetstimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tetstimonial == null)
            {
                return NotFound();
            }

            return View(tetstimonial);
        }

        // POST: Tetstimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Tetstimonials == null)
            {
                return Problem("Entity set 'ModelContext.Tetstimonials'  is null.");
            }
            var tetstimonial = await _context.Tetstimonials.FindAsync(id);
            if (tetstimonial != null)
            {
                _context.Tetstimonials.Remove(tetstimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TetstimonialExists(decimal id)
        {
          return (_context.Tetstimonials?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
