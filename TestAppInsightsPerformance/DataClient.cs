using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestAppInsightsPerformance
{
    public class DataClient
    {
        private HttpClient _httpClient;

        public DataClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<NameRating>> GetTestData()
        {
            var resp = await _httpClient.GetAsync("https://appinsightstestdata.blob.core.windows.net/testdata/testlist1.json");
            resp.EnsureSuccessStatusCode();
            var stringContent = await resp.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<List<NameRating>>(stringContent);
            return content;
        }
    }
}
