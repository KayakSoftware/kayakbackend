using System.Net.Http;
using System.Threading.Tasks;
using KayakBackend.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace KayakBackend.Controllers
{
    [Route("sensor")]
    public class SensorController
    {
        [HttpGet]
        [Route("predictor")]
        public async Task<IActionResult> PredictResult([FromBody]PredictRequestModel requestModel)
        {
            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync("http://localhost:1500/hello/Ulrik");

            return new OkResult();
        }
    }
}