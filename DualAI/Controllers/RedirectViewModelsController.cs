using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DualAI.Data;
using DualAI.Models;
using Newtonsoft.Json;

namespace DualAI.Controllers
{
    public class RedirectViewModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RedirectViewModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RedirectViewModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.RedirectViewModel.ToListAsync());
        }

        // GET: RedirectViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var redirectViewModel = await _context.RedirectViewModel
                .FirstOrDefaultAsync(m => m.id == id);
            if (redirectViewModel == null)
            {
                return NotFound();
            }

            return View(redirectViewModel);
        }

        // GET: RedirectViewModels/Redirect
        public IActionResult Redirect()
        {
            return View();
        }

        // POST: RedirectViewModels/Redirect
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Redirect([Bind("id,GeneratedText,AudioUrl")] RedirectViewModel redirectViewModel)
        {
            if (ModelState.IsValid)
            {
                // Aggiungi qui la chiamata API
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.textcortex.com/v1/texts/translations");
                request.Headers.Add("Authorization", "Bearer gAAAAABmO0_fvdioGMZOZGwYvCnECRGJrt9Qlg1rBqUM_HOxvqaWEKLo_ZSAp9KkpWkEZWJvGstz_a7sVwzmhUdeo7F5a4eO63vRpg7FFuAI8hg2q8fzcZSpRDtsPdcW5FSmU-xc7Llp");
                var content = new StringContent("{\r\n  \"formality\": \"default\",\r\n  \"source_lang\": \"it\",\r\n  \"target_lang\": \"en\",\r\n  \"text\": \"Ciao\"\r\n}", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var apiResponse = await response.Content.ReadAsStringAsync();

                // Deserializza la risposta JSON
                var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);

                // Inserisci il campo 'text' nel campo 'GeneratedText' del tuo modello
                if (jsonResponse.ContainsKey("text"))
                {
                    redirectViewModel.GeneratedText = jsonResponse["text"];
                }

                // Continua con il codice originale
                _context.Add(redirectViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(redirectViewModel);
        }


        // GET: RedirectViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var redirectViewModel = await _context.RedirectViewModel.FindAsync(id);
            if (redirectViewModel == null)
            {
                return NotFound();
            }
            return View(redirectViewModel);
        }

        // POST: RedirectViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,GeneratedText,AudioUrl")] RedirectViewModel redirectViewModel)
        {
            if (id != redirectViewModel.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(redirectViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RedirectViewModelExists(redirectViewModel.id))
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
            return View(redirectViewModel);
        }

        // GET: RedirectViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var redirectViewModel = await _context.RedirectViewModel
                .FirstOrDefaultAsync(m => m.id == id);
            if (redirectViewModel == null)
            {
                return NotFound();
            }

            return View(redirectViewModel);
        }

        // POST: RedirectViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var redirectViewModel = await _context.RedirectViewModel.FindAsync(id);
            if (redirectViewModel != null)
            {
                _context.RedirectViewModel.Remove(redirectViewModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RedirectViewModelExists(int id)
        {
            return _context.RedirectViewModel.Any(e => e.id == id);
        }
    }
}
