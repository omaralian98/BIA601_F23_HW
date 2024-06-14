using Algorithms;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class APIController(ILogger<APIController> logger) : ControllerBase
    {
        [HttpPost("mode1")]
        [Consumes("application/json")]
        public ActionResult<ResponseModel> Mode1Json([FromBody] RequestModel request)
        {
            logger.LogInformation("Request Started");
            var (TotalDistance, BestRoute, TotalValue, IncludedItems) = Mode1.Start(request.Distances, request.Capacity, request.Weights, request.Values);
            return Ok(new ResponseModel
            {
                BestRoute = BestRoute,
                IncludedItems = IncludedItems,
                TotalDistance = TotalDistance,    
                TotalValue = TotalValue,
            });
        }

        [HttpPost("mode1")]
        [Consumes("multipart/form-data")]
        public ActionResult<ResponseModel> Mode1Form([FromForm] RequestModel request)
        {
            logger.LogInformation("Request Started");
            var (TotalDistance, BestRoute, TotalValue, IncludedItems) = Mode1.Start(request.Distances, request.Capacity, request.Weights, request.Values);
            return Ok(new ResponseModel
            {
                BestRoute = BestRoute,
                IncludedItems = IncludedItems,
                TotalDistance = TotalDistance,
                TotalValue = TotalValue,
            });
        }

        [HttpPost("mode2")]
        [Consumes("application/json")]
        public ActionResult<ResponseModel> Mode2Json([FromBody] RequestModel request)
        {
            logger.LogInformation("Request Started");
            var (TotalDistance, BestRoute, TotalValue, IncludedItems) = Mode2.Start(request.Distances, request.Capacity, request.Weights, request.Values);
            return Ok(new ResponseModel
            {
                BestRoute = BestRoute,
                IncludedItems = IncludedItems,
                TotalDistance = TotalDistance,
                TotalValue = TotalValue,
            });
        }

        [HttpPost("mode1")]
        [Consumes("multipart/form-data")]
        public ActionResult<ResponseModel> Mode2Form([FromForm] RequestModel request)
        {
            logger.LogInformation("Request Started");
            var (TotalDistance, BestRoute, TotalValue, IncludedItems) = Mode2.Start(request.Distances, request.Capacity, request.Weights, request.Values);
            return Ok(new ResponseModel
            {
                BestRoute = BestRoute,
                IncludedItems = IncludedItems,
                TotalDistance = TotalDistance,
                TotalValue = TotalValue,
            });
        }
    }

    public class RequestModel
    {
        [JsonPropertyName("distances")]
        public int[][] Distances { get; set; }

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        [JsonPropertyName("weights")]
        public int[] Weights { get; set; }

        [JsonPropertyName("values")]
        public int[] Values { get; set; }
    }

    public class ResponseModel
    {
        public int TotalDistance { get; set; }
        public int[] BestRoute { get; set; }
        public int TotalValue { get; set; }
        public int[] IncludedItems { get; set; }
    }
}
