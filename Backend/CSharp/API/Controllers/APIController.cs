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

        [HttpPost("mode2")]
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

        [HttpPost("mode3")]
        [Consumes("application/json")]
        public ActionResult<ResponseModelForMode3AMore> Mode3Json([FromBody] RequestModelForMode3 request)
        {
            logger.LogInformation("Request Started");
            var (TotalDistance, BestRoutes, TotalValue, IncludedItems) = Mode3.Start(request.Distances, request.Capacities, request.Weights, request.Values);
            Truck[] trucks = new Truck[request.Capacities.Length];
            for (int i = 0; i < trucks.Length; i++)
            {
                trucks[i] = new Truck(i, request.Capacities[i], IncludedItems[i], BestRoutes[i], request.Values, request.Distances);
            }
            return Ok(new ResponseModelForMode3AMore
            {
                TotalValue = TotalValue,
                TotalDistance = TotalDistance,
                Trucks = trucks
            });
        }

        [HttpPost("mode3")]
        [Consumes("multipart/form-data")]
        public ActionResult<ResponseModel> Mode3Form([FromForm] RequestModel request)
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


    public class RequestModelForMode3
    {
        [JsonPropertyName("distances")]
        public int[][] Distances { get; set; }

        [JsonPropertyName("capacities")]
        public int[] Capacities { get; set; }

        [JsonPropertyName("weights")]
        public int[] Weights { get; set; }

        [JsonPropertyName("values")]
        public int[] Values { get; set; }
    }

    public class ResponseModelForMode3AMore
    {
        public int TotalValue { get; set; }
        public int TotalDistance { get; set; }
        public Truck[] Trucks { get; set; } = [];
    }

    public class Truck
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public int Value { get; set; }
        public int Distance { get; set; }
        public int[] IncludedItems { get; set; } = [];
        public int[] Route { get; set; } = [];

        public Truck(int id, int capacity, int[] includedItems, int[] route, int[] Values, int[][] distances)
        {
            Id = id;
            Capacity = capacity;
            IncludedItems = includedItems;
            Route = route;
            for (int i = 0; i < includedItems.Length; i++)
            {
                Value += Values[includedItems[i]];
            }

            for (int i = 0; i < route.Length - 1; i++)
            {
                Distance += distances[route[i]][route[i + 1]];
            }
        }
    }
}
