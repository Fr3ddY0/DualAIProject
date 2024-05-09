using Microsoft.AspNetCore.Mvc;

namespace API_Wrap.Controllers
{
    public class AudioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("Audio")]
        public async Task<IActionResult> Audio()
        {
            // Eseguire chiamata di Swagger con questi parametri
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://play.ht/studio/files/44a86ab7-3a7b-4acc-ba9f-4cd8a186c953");
            request.Headers.Add("Authorization", "Basic QnFSRklwRkdEVWhrZmkyeTQxcXJIMUFpN2M5Mjo3OGQzMzA5M2QzNzY0ZjNkYmM3ODU3ZGMxMDk0MTE0YQ==");
            var content = new StringContent("Ciao", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            return Ok();

        }
    }
}
