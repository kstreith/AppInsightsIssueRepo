using System;
using System.Threading.Tasks;
//using Hangfire;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;

namespace TestAppInsightsPerformance.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    //[ApiController]
    public class QueueController : ControllerBase
    {
        //private DataClient _dataClient;
        //private TelemetryClient _client;

        public QueueController(/*DataClient dataClient, TelemetryClient client*/)
        {
            /*_dataClient = dataClient;
            _client = client;*/
        }

        // GET api/values
        [HttpGet]
        public async Task<string> Get()
        {
            var val = Guid.NewGuid().ToString();
            //BackgroundJob.Enqueue(() => DoWork(val));
            return await Task.FromResult($"Queued-{val}");
        }

        /*
        public async Task DoWork(string guid)
        {
            using (var op = _client.StartOperation<RequestTelemetry>("Background: QueueController-DoWork"))
            {
                _client.TrackTrace("starting request for data");
                var data = await _dataClient.GetTestData();
                _client.TrackTrace($"got back results for data count is {data.Count}");

                op.Telemetry.ResponseCode = "200";
                _client.StopOperation(op);
            }
        }*/
    }
}
