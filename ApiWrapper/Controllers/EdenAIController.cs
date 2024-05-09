using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApiWrapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EdenAIController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public EdenAIController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("text/generation")]
        public async Task<IActionResult> GenerateText()
        {
            try
            {
                string apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiMmNhZGM2NjQtMjcyNi00MGVkLWJlZjctNjk0MjU5MjFjYzNtrhIiwidHlwZSI6ImFwaV90b2tlbiJ9.SqxuDVRowPI9pLkPUaqb3SrnTItrP0xWxjkJyg92cDc";
                string apiUrl = "https://api.edenai.run/v2/text/generation";

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    return Ok(responseData);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Failed to call API.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
