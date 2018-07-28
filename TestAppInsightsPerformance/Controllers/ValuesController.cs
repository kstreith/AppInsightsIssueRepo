using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestAppInsightsPerformance.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    //[ApiController]
    public class ValuesController : ControllerBase
    {
        private DataClient _dataClient;
        //private CacheDataClient _cacheDataClient;

        public ValuesController(DataClient dataClient/*, CacheDataClient cacheDataClient*/)
        {
            _dataClient = dataClient;
            //_cacheDataClient = cacheDataClient;
        }

        // GET api/values
        [HttpGet]
        public async Task<string> Get()
        {
            var firstData = await _dataClient.GetTestData();
            //var secondData = await _cacheDataClient.GetAll();
            var r = new Random();
            var firstValue = firstData[r.Next(firstData.Count)];
            //var secondValue = secondData[r.Next(secondData.Count)];
            //return $"{firstValue.Name}-{firstValue.Rating}-{secondValue.Description}-{secondValue.Location}-{Guid.NewGuid()}";
            return $"{firstValue.Name}-{firstValue.Rating}-{Guid.NewGuid()}";
        }
    }
}
