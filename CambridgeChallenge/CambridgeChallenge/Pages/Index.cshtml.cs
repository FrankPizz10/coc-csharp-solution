using CambridgeChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Octokit;
using System.Diagnostics;

namespace CambridgeChallenge.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private HttpClient _httpClient;

        [BindProperty]
        public List<EventModel> events { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task OnGetAsync()
        {
            await getEventsData();
        }

        public async Task getEventsData()
        {
            string path = "https://www.cambridgema.gov/GetEvents.ashx?format=json&view=homepage";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(path);

                var responseContent = await response.Content.ReadAsStringAsync();

                this.events = JsonConvert.DeserializeObject<List<EventModel>>(responseContent);

                Debug.WriteLine("Events: " + this.events[0].Title);
            }
            catch (HttpRequestException exception) 
            {
                Debug.WriteLine("An HTTP request exception occurred.", exception.Message);
            }
        }
    }
}