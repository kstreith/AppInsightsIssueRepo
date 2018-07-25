using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestAppInsightsPerformance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private DataClient _dataClient;

        public ValuesController(DataClient dataClient)
        {
            _dataClient = dataClient;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var firstData = await _dataClient.GetTestData();
            var secondData = await _dataClient.GetTestData2();
            var r = new Random();
            var firstValue = firstData[r.Next(firstData.Count)];
            var secondValue = secondData[r.Next(secondData.Count)];
            return $"{firstValue.Name}-{firstValue.Rating}-{secondValue.Description}-{secondValue.Location}-{Guid.NewGuid()}";
        }
    }
}
