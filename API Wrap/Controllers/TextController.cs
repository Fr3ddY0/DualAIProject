using Microsoft.AspNetCore.Mvc;

namespace API_Wrap.Controllers
{
    public class TextController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("Testo")]
        public async Task<IActionResult> Testo()
        {
            // Eseguire chiamata di Swagger con questi parametri
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.textcortex.com/v1/texts/translations");
            request.Headers.Add("Authorization", "Bearer gAAAAABmO0_fvdioGMZOZGwYvCnECRGJrt9Qlg1rBqUM_HOxvqaWEKLo_ZSAp9KkpWkEZWJvGstz_a7sVwzmhUdeo7F5a4eO63vRpg7FFuAI8hg2q8fzcZSpRDtsPdcW5FSmU-xc7Llp");
            var content = new StringContent("{\r\n  \"formality\": \"default\",\r\n  \"source_lang\": \"it\",\r\n  \"target_lang\": \"en\",\r\n  \"text\": \"Ciao\"\r\n}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());



            return Ok();

        }

    }
}
