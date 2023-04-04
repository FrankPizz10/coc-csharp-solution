using CambridgeChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CambridgeChallenge.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private HttpClient _httpClient;

        [BindProperty]
        public List<EventModel> events { get; set; }

        [BindProperty]
        public Dictionary<string, List<EventModel>> dateDictionary { get; set; }

        [BindProperty(SupportsGet = true)]
        public int cols { get; set; }

        [BindProperty(SupportsGet = true)]
        public int rows { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task OnGetAsync()
        {
            await getEventsData();
            setDateDictionary();
            if (this.cols < 2 || this.cols > 6)
            {
                this.cols = 3;
            }
            if (this.rows < 1 || this.rows > 8)
            {
                this.rows = 4;
            }
        }

        public async Task getEventsData()
        {
            var path = "https://www.cambridgema.gov/GetEvents.ashx?format=json&view=homepage";

            var httpRequestMessage = new HttpRequestMessage();

            try
            {
                var response = await _httpClient.GetAsync(path);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    Debug.WriteLine("No data received from server.");
                    this.events = new List<EventModel>();
                }
                else
                {
                    this.events = JsonConvert.DeserializeObject<List<EventModel>>(responseContent);
                }
            }
            catch (HttpRequestException exception) 
            {
                this.events = new List<EventModel>();
                Debug.WriteLine("An HTTP request exception occurred.", exception.Message);
            }
        }

        public void setDateDictionary()
        {   
            this.dateDictionary = new Dictionary<string, List<EventModel>>();
            foreach (var item in this.events)
            {
                var date = item.Start.Substring(0, 10);
                if (!this.dateDictionary.ContainsKey(date))
                {
                    this.dateDictionary[date] = new List<EventModel> { item };
                }
                else
                {
                    this.dateDictionary[date].Add(item);
                }
            }
        }
    }
}