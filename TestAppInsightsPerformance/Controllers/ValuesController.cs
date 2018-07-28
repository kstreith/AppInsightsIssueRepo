using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestAppInsightsPerformance.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ValuesController : ControllerBase
    {
        private DataClient _dataClient;

        public ValuesController(DataClient dataClient)
        {
            _dataClient = dataClient;
        }

        // GET api/values
        [HttpGet]
        public async Task<string> Get()
        {
            var firstData = await _dataClient.GetTestData();
            var r = new Random();
            var firstValue = firstData[r.Next(firstData.Count)];
            return $"{firstValue.Name}-{firstValue.Rating}-{Guid.NewGuid()}";
        }
    }
}
