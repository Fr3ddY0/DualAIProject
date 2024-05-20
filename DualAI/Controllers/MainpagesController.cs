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
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;


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

        // Metodo rifatto richiesto dal prof
        public async Task<IActionResult> Push(string id)
        {
            var mainpage = await _context.Mainpage.FindAsync(id);
            if (mainpage == null)
            {
                return NotFound();
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.textcortex.com/v1/texts/translations"),
                Headers =
        {
            { "Authorization", "Bearer gAAAAABmO0_fvdioGMZOZGwYvCnECRGJrt9Qlg1rBqUM_HOxvqaWEKLo_ZSAp9KkpWkEZWJvGstz_a7sVwzmhUdeo7F5a4eO63vRpg7FFuAI8hg2q8fzcZSpRDtsPdcW5FSmU-xc7Llp" },
        },
                Content = new StringContent("{\n  \"formality\": \"default\",\n  \"source_lang\": \"" + mainpage.LinguaOriginale + "\",\n  \"target_lang\": \"" + mainpage.LinguaDiTraduzione + "\",\n  \"text\": \"" + mainpage.Prompt + "\"\n}")
                {
                    Headers =
            {
                ContentType = new MediaTypeHeaderValue("application/json")
            }
                }
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserializza la risposta JSON per estrarre il campo "text"
                var jsonResponse = JObject.Parse(responseBody);
                var generatedText = jsonResponse["data"]?["outputs"]?.FirstOrDefault()?["text"]?.ToString();

                if (generatedText != null)
                {
                    // Salva il testo generato nel campo GeneratedText del mainpage
                    mainpage.GeneratedText = generatedText;
                    _context.Update(mainpage);
                    await _context.SaveChangesAsync();
                }
            }

            // Esegui la seconda chiamata API
            var secondClient = new HttpClient();
            var secondRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.play.ht/api/v2/tts/stream"),
                Headers =
        {
            { "accept", "application/json" },
            { "AUTHORIZATION", "df888c2ce26e469faec722f7e8426a5d" },
            { "X-USER-ID", "x6dI5kAeNUMoS39JrFCld5H4VQP2" },
        },
                Content = new StringContent("{\"text\":\"" + mainpage.GeneratedText + "\",\"voice\":\"s3://voice-cloning-zero-shot/d9ff78ba-d016-47f6-b0ef-dd630f59414e/female-cs/manifest.json\",\"output_format\":\"mp3\",\"quality\":\"medium\"}")
                {
                    Headers =
            {
                ContentType = new MediaTypeHeaderValue("application/json")
            }
                }
            };

            using (var secondResponse = await secondClient.SendAsync(secondRequest))
            {
                secondResponse.EnsureSuccessStatusCode();
                var body = await secondResponse.Content.ReadAsStringAsync();

                // Deserializza la risposta JSON per estrarre il campo "href"
                var secondJsonResponse = JObject.Parse(body);
                var audioUrl = secondJsonResponse["href"]?.ToString();

                if (audioUrl != null)
                {
                    // Salva l'URL dell'audio nel campo audiourl del mainpage
                    mainpage.AudioUrl = audioUrl;
                    _context.Update(mainpage);
                    await _context.SaveChangesAsync();
                }
            }

            return View(mainpage); // Passa il modello alla vista
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
