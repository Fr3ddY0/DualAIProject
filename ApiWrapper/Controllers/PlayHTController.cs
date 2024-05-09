using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlayHTController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayHTController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlayHTController
            (IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("prova")]
        public async Task <IActionResult> prova()
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
