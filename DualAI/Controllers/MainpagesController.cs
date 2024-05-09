using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DualAI.Data;
using DualAI.Models;
using Microsoft.AspNetCore.Authorization;

namespace DualAI.Controllers
{
    [Authorize]
    public class MainpagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MainpagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Mainpages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Mainpage.ToListAsync());
        }

        // GET: Mainpages/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainpage = await _context.Mainpage
                .FirstOrDefaultAsync(m => m.Nickname == id);
            if (mainpage == null)
            {
                return NotFound();
            }

            return View(mainpage);
        }

        // GET: Mainpages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mainpages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nickname,Prompt,LinguaOriginale,LinguaDiTraduzione")] Mainpage mainpage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mainpage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mainpage);
        }

        // GET: Mainpages/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainpage = await _context.Mainpage.FindAsync(id);
            if (mainpage == null)
            {
                return NotFound();
            }
            return View(mainpage);
        }

        // POST: Mainpages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Nickname,Prompt,LinguaOriginale,LinguaDiTraduzione")] Mainpage mainpage)
        {
            if (id != mainpage.Nickname)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mainpage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MainpageExists(mainpage.Nickname))
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
            return View(mainpage);
        }

        // GET: Mainpages/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainpage = await _context.Mainpage
                .FirstOrDefaultAsync(m => m.Nickname == id);
            if (mainpage == null)
            {
                return NotFound();
            }

            return View(mainpage);
        }

        // POST: Mainpages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var mainpage = await _context.Mainpage.FindAsync(id);
            if (mainpage != null)
            {
                _context.Mainpage.Remove(mainpage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MainpageExists(string id)
        {
            return _context.Mainpage.Any(e => e.Nickname == id);
        }
    }
}
