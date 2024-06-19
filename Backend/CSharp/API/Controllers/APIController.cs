using Algorithms;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/")]
    [RequestTimeout(milliseconds: 180000)]
    public class APIController(ILogger<APIController> logger) : ControllerBase
    {
        [HttpPost("mode1")]
        [Consumes("application/json")]
        public ActionResult<ResponseModel> Mode1Json([FromBody] RequestModel request)
        {
            logger.LogInformation("Mode1 Json Request Started");
            var watch = new Stopwatch();
            watch.Start();
            var result = GetMode1(request);
            watch.Stop();
            if (result is null)
            {
                logger.LogError("An exception was thrown for this request: \n" + request.ToString());
                return BadRequest("Something went wrong");
            }
            logger.LogInformation("Server responded with: \n" + result.ToString());
            logger.LogInformation($"The Request Took: {watch.ElapsedMilliseconds} ms");
            return Ok(result);
        }

        [HttpPost("mode1")]
        [Consumes("multipart/form-data")]
        public ActionResult<ResponseModel> Mode1Form([FromForm] RequestModel request)
        {
            logger.LogInformation("Mode1 Form Request Started");
            var watch = new Stopwatch();
            watch.Start();
            var result = GetMode1(request);
            watch.Stop();
            if (result is null)
            {
                logger.LogError("An exception was thrown for this request: \n" + request.ToString());
                return BadRequest("Something went wrong");
            }
            logger.LogInformation("Server responded with: \n" + result.ToString());
            logger.LogInformation($"The Request Took: {watch.ElapsedMilliseconds} ms");
            return Ok(result);
        }

        [HttpPost("mode2")]
        [Consumes("application/json")]
        public ActionResult<ResponseModel> Mode2Json([FromBody] RequestModel request)
        {
            logger.LogInformation("Mode2 Json Request Started");
            var watch = new Stopwatch();
            watch.Start();
            var result = GetMode2(request);
            watch.Stop();
            if (result is null)
            {
                logger.LogError("An exception was thrown for this request: \n" + request.ToString());
                return BadRequest("Something went wrong");
            }
            logger.LogInformation("Server responded with: \n" + result.ToString());
            logger.LogInformation($"The Request Took: {watch.ElapsedMilliseconds} ms");
            return Ok(result);
        }

        [HttpPost("mode2")]
        [Consumes("multipart/form-data")]
        public ActionResult<ResponseModel> Mode2Form([FromForm] RequestModel request)
        {
            logger.LogInformation("Mode2 Form Request Started");
            var watch = new Stopwatch();
            watch.Start();
            var result = GetMode2(request);
            watch.Stop();
            if (result is null)
            {
                logger.LogError("An exception was thrown for this request: \n" + request.ToString());
                return BadRequest("Something went wrong");
            }
            logger.LogInformation("Server responded with: \n" + result.ToString());
            logger.LogInformation($"The Request Took: {watch.ElapsedMilliseconds} ms");
            return Ok(result);
        }

        [HttpPost("mode3")]
        [Consumes("application/json")]
        public ActionResult<ResponseModelForMode3AMore> Mode3Json([FromBody] RequestModelForMode3 request)
        {
            logger.LogInformation("Mode3 Json Request Started");
            var watch = new Stopwatch();
            watch.Start();
            var result = GetMode3(request);
            watch.Stop();
            if (result is null)
            {
                logger.LogError("An exception was thrown for this request: \n" + request.ToString());
                return BadRequest("Something went wrong");
            }
            logger.LogInformation("Server responded with: \n" + result.ToString());
            logger.LogInformation($"The Request Took: {watch.ElapsedMilliseconds} ms");
            return Ok(result);
        }

        [HttpPost("mode3")]
        [Consumes("multipart/form-data")]
        public ActionResult<ResponseModelForMode3AMore> Mode3Form([FromForm] RequestModelForMode3 request)
        {
            logger.LogInformation("Mode3 Form Request Started");
            var watch = new Stopwatch();
            watch.Start();
            var result = GetMode3(request);
            watch.Stop();
            if (result is null)
            {
                logger.LogError("An exception was thrown for this request: \n" + request.ToString());
                return BadRequest("Something went wrong");
            }
            logger.LogInformation("Server responded with: \n" + result.ToString());
            logger.LogInformation($"The Request Took: {watch.ElapsedMilliseconds} ms");
            return Ok(result);
        }

        [HttpPost("mode4")]
        [Consumes("application/json")]
        public ActionResult<ResponseModelForMode3AMore> Mode4Json([FromBody] RequestModelForMode4 request)
        {
            logger.LogInformation("Mode4 Json Request Started");
            var watch = new Stopwatch();
            watch.Start();
            var result = GetMode4(request);
            watch.Stop();
            if (result is null)
            {
                logger.LogError("An exception was thrown for this request: \n" + request.ToString());
                return BadRequest("Something went wrong");
            }
            logger.LogInformation("Server responded with: \n" + result.ToString());
            logger.LogInformation($"The Request Took: {watch.ElapsedMilliseconds} ms");
            return Ok(result);
        }

        [HttpPost("mode4")]
        [Consumes("multipart/form-data")]
        public ActionResult<ResponseModelForMode3AMore> Mode4Form([FromForm] RequestModelForMode4 request)
        {
            logger.LogInformation("Mode4 Form Request Started");
            var watch = new Stopwatch();
            watch.Start();
            var result = GetMode4(request);
            watch.Stop();
            if (result is null)
            {
                logger.LogError("An exception was thrown for this request: \n" + request.ToString());
                return BadRequest("Something went wrong");
            }
            logger.LogInformation("Server responded with: \n" + result.ToString());
            logger.LogInformation($"The Request Took: {watch.ElapsedMilliseconds} ms");
            return Ok(result);
        }

        [HttpPost("mode5")]
        [Consumes("application/json")]
        public ActionResult<ResponseModelForMode3AMore> Mode5Json([FromBody] RequestModelForMode5 request)
        {
            logger.LogInformation("Mode5 Json Request Started");
            var watch = new Stopwatch();
            watch.Start();
            var result = GetMode5(request);
            watch.Stop();
            if (result is null)
            {
                logger.LogError("An exception was thrown for this request: \n" + request.ToString());
                return BadRequest("Something went wrong");
            }
            logger.LogInformation("Server responded with: \n" + result.ToString());
            logger.LogInformation($"The Request Took: {watch.ElapsedMilliseconds} ms");
            return Ok(result);
        }

        [HttpPost("mode5")]
        [Consumes("multipart/form-data")]
        public ActionResult<ResponseModelForMode3AMore> Mode5Form([FromForm] RequestModelForMode5 request)
        {
            logger.LogInformation("Mode5 Form Request Started");
            var watch = new Stopwatch();
            watch.Start();
            var result = GetMode5(request);
            watch.Stop();
            if (result is null)
            {
                logger.LogError("An exception was thrown for this request: \n" + request.ToString());
                return BadRequest("Something went wrong");
            }
            logger.LogInformation("Server responded with: \n" + result.ToString());
            logger.LogInformation($"The Request Took: {watch.ElapsedMilliseconds} ms");
            return Ok(result);
        }

        [HttpPost("mode6")]
        [Consumes("application/json")]
        public ActionResult<ResponseModelForMode3AMore> Mode6Json([FromBody] RequestModelForMode6 request)
        {
            logger.LogInformation("Mode6 Json Request Started");
            var watch = new Stopwatch();
            watch.Start();
            var result = GetMode6(request);
            watch.Stop();
            if (result is null)
            {
                logger.LogError("An exception was thrown for this request: \n" + request.ToString());
                return BadRequest("Something went wrong");
            }
            logger.LogInformation("Server responded with: \n" + result.ToString());
            logger.LogInformation($"The Request Took: {watch.ElapsedMilliseconds} ms");
            return Ok(result);
        }

        [HttpPost("mode6")]
        [Consumes("multipart/form-data")]
        public ActionResult<ResponseModelForMode3AMore> Mode6Form([FromForm] RequestModelForMode6 request)
        {
            logger.LogInformation("Mode6 Form Request Started");
            var watch = new Stopwatch();
            watch.Start();
            var result = GetMode6(request);
            watch.Stop();
            if (result is null)
            {
                logger.LogError("An exception was thrown for this request: \n" + request.ToString());
                return BadRequest("Something went wrong");
            }
            logger.LogInformation("Server responded with: \n" + result.ToString());
            logger.LogInformation($"The Request Took: {watch.ElapsedMilliseconds} ms");
            return Ok(result);
        }






        private ResponseModel? GetMode1(RequestModel request)
        {
            try
            {
                var (TotalDistance, BestRoute, TotalValue, IncludedItems) = Mode1.Start(request.Distances, request.Capacity, request.Weights, request.Values);
                return new ResponseModel
                {
                    BestRoute = BestRoute,
                    IncludedItems = IncludedItems,
                    TotalDistance = TotalDistance,
                    TotalValue = TotalValue,
                };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
            return null;
        }

        private ResponseModel? GetMode2(RequestModel request)
        {
            try
            {
                var (TotalDistance, BestRoute, TotalValue, IncludedItems) = Mode2.Start(request.Distances, request.Capacity, request.Weights, request.Values);
                return new ResponseModel
                {
                    BestRoute = BestRoute,
                    IncludedItems = IncludedItems,
                    TotalDistance = TotalDistance,
                    TotalValue = TotalValue,
                };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
            return null;
        }

        private ResponseModelForMode3AMore? GetMode3(RequestModelForMode3 request)
        {
            try
            {
                var (TotalDistance, BestRoutes, TotalValue, IncludedItems) = Mode3.Start(request.Distances, request.Capacities, request.Weights, request.Values);
                return ResponseModelForMode3AMore.Map(TotalDistance, BestRoutes, TotalValue, IncludedItems, request.Distances, request.Capacities, request.Values);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
            return null;
        }

        private ResponseModelForMode3AMore? GetMode4(RequestModelForMode4 request)
        {
            try
            {
                var (TotalDistance, BestRoutes, TotalValue, IncludedItems) = Mode4.Start(request.Distances, request.indicesOfStartingPoints, request.indicesOfEndingPoints, request.Capacities, request.Weights, request.Values, request.Settings);
                return ResponseModelForMode3AMore.Map(TotalDistance, BestRoutes, TotalValue, IncludedItems, request.Distances, request.Capacities, request.Values);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
            return null;
        }

        private ResponseModelForMode3AMore? GetMode5(RequestModelForMode5 request)
        {
            try
            {
                var (TotalDistance, BestRoutes, TotalValue, IncludedItems) = Mode5.Start(request.Distances, request.indicesOfStartingPoints, request.indicesOfEndingPoints, request.indicesOfPickingUpPoints, request.indicesOfDroppingOffPoints, request.Capacities, request.Weights, request.Values, request.Settings);
                return ResponseModelForMode3AMore.Map(TotalDistance, BestRoutes, TotalValue, IncludedItems, request.Distances, request.Capacities, request.Values);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
            return null;
        }

        private ResponseModelForMode3AMore? GetMode6(RequestModelForMode6 request)
        {
            try
            {
                var (TotalDistance, BestRoutes, TotalValue, IncludedItems) = Mode6.Start(request.Distances, request.indicesOfStartingPoints, request.indicesOfEndingPoints, request.indicesOfPickingUpPoints, request.indicesOfDroppingOffPoints, request.Capacities, request.Weights, request.Values, request.pickUpPenalties, request.dropOffPenalties, request.Settings);
                return ResponseModelForMode3AMore.Map(TotalDistance, BestRoutes, TotalValue, IncludedItems, request.Distances, request.Capacities, request.Values);
            }
            catch (Exception e) 
            {
                logger.LogError(e.Message);
            }
            return null;
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

        [JsonPropertyName("settings")]
        public Settings? Settings { get; set; } = null;

        public override string ToString()
        {
            string distancesStr = Distances != null ? string.Join("; ", Distances.Select(row => string.Join(",", row))) : "null";
            string weightsStr = Weights != null ? string.Join(", ", Weights) : "null";
            string valuesStr = Values != null ? string.Join(", ", Values) : "null";
            string settingsStr = Settings?.ToString() ?? "null";

            return $"Distances: [{distancesStr}]\n Capacity: {Capacity}\n Weights: [{weightsStr}]\n Values: [{valuesStr}]\n Settings: {settingsStr}";
        }
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

        [JsonPropertyName("settings")]
        public Settings? Settings { get; set; } = null;

        public override string ToString()
        {
            string distancesStr = Distances != null ? string.Join("; ", Distances.Select(row => string.Join(",", row))) : "null";
            string capacitiesStr = Capacities != null ? string.Join(", ", Capacities) : "null";
            string weightsStr = Weights != null ? string.Join(", ", Weights) : "null";
            string valuesStr = Values != null ? string.Join(", ", Values) : "null";
            string settingsStr = Settings?.ToString() ?? "null";

            return $"Distances: [{distancesStr}]\n Capacities: [{capacitiesStr}]\n Weights: [{weightsStr}]\n Values: [{valuesStr}]\n Settings: {settingsStr}";
        }
    }

    public class RequestModelForMode4
    {
        [JsonPropertyName("distances")]
        public int[][] Distances { get; set; }

        [JsonPropertyName("capacities")]
        public int[] Capacities { get; set; }

        [JsonPropertyName("weights")]
        public int[] Weights { get; set; }

        [JsonPropertyName("values")]
        public int[] Values { get; set; }

        [JsonPropertyName("indicesofstartingpoints")]
        public int[] indicesOfStartingPoints {  get; set; }

        [JsonPropertyName("indicesofendingpoints")]
        public int[] indicesOfEndingPoints { get; set; }

        [JsonPropertyName("settings")]
        public Settings? Settings { get; set; } = null;

        public override string ToString()
        {
            string distancesStr = Distances != null ? string.Join("; ", Distances.Select(row => string.Join(",", row))) : "null";
            string capacitiesStr = Capacities != null ? string.Join(", ", Capacities) : "null";
            string weightsStr = Weights != null ? string.Join(", ", Weights) : "null";
            string valuesStr = Values != null ? string.Join(", ", Values) : "null";
            string startingPointsStr = indicesOfStartingPoints != null ? string.Join(", ", indicesOfStartingPoints) : "null";
            string endingPointsStr = indicesOfEndingPoints != null ? string.Join(", ", indicesOfEndingPoints) : "null";
            string settingsStr = Settings?.ToString() ?? "null";

            return $"Distances: [{distancesStr}]\n Capacities: [{capacitiesStr}]\n Weights: [{weightsStr}]\n Values: [{valuesStr}]\n " +
                   $"IndicesOfStartingPoints: [{startingPointsStr}]\n IndicesOfEndingPoints: [{endingPointsStr}]\n Settings: {settingsStr}";
        }
    }

    public class RequestModelForMode5
    {
        [JsonPropertyName("distances")]
        public int[][] Distances { get; set; }

        [JsonPropertyName("capacities")]
        public int[] Capacities { get; set; }

        [JsonPropertyName("weights")]
        public int[] Weights { get; set; }

        [JsonPropertyName("values")]
        public int[] Values { get; set; }

        [JsonPropertyName("indicesofstartingpoints")]
        public int[] indicesOfStartingPoints { get; set; }

        [JsonPropertyName("indicesofendingpoints")]
        public int[] indicesOfEndingPoints { get; set; }

        [JsonPropertyName("indicesofpickinguppoints")]
        public int[] indicesOfPickingUpPoints { get; set; }

        [JsonPropertyName("indicesofdroppingoffpoints")]
        public int[] indicesOfDroppingOffPoints { get; set; }

        [JsonPropertyName("settings")]
        public Settings? Settings { get; set; } = null;

        public override string ToString()
        {
            string distancesStr = Distances != null ? string.Join("; ", Distances.Select(row => string.Join(",", row))) : "null";
            string capacitiesStr = Capacities != null ? string.Join(", ", Capacities) : "null";
            string weightsStr = Weights != null ? string.Join(", ", Weights) : "null";
            string valuesStr = Values != null ? string.Join(", ", Values) : "null";
            string startingPointsStr = indicesOfStartingPoints != null ? string.Join(", ", indicesOfStartingPoints) : "null";
            string endingPointsStr = indicesOfEndingPoints != null ? string.Join(", ", indicesOfEndingPoints) : "null";
            string pickingUpPointsStr = indicesOfPickingUpPoints != null ? string.Join(", ", indicesOfPickingUpPoints) : "null";
            string droppingOffPointsStr = indicesOfDroppingOffPoints != null ? string.Join(", ", indicesOfDroppingOffPoints) : "null";
            string settingsStr = Settings?.ToString() ?? "null";

            return $"Distances: [{distancesStr}]\n Capacities: [{capacitiesStr}]\n Weights: [{weightsStr}]\n Values: [{valuesStr}]\n " +
                   $"IndicesOfStartingPoints: [{startingPointsStr}]\n IndicesOfEndingPoints: [{endingPointsStr}]\n " +
                   $"IndicesOfPickingUpPoints: [{pickingUpPointsStr}]\n IndicesOfDroppingOffPoints: [{droppingOffPointsStr}]\n Settings: {settingsStr}";
        }
    }

    public class RequestModelForMode6
    {
        [JsonPropertyName("distances")]
        public int[][] Distances { get; set; }

        [JsonPropertyName("capacities")]
        public int[] Capacities { get; set; }

        [JsonPropertyName("weights")]
        public int[] Weights { get; set; }

        [JsonPropertyName("values")]
        public int[] Values { get; set; }

        [JsonPropertyName("indicesofstartingpoints")]
        public int[] indicesOfStartingPoints { get; set; }

        [JsonPropertyName("indicesofendingpoints")]
        public int[] indicesOfEndingPoints { get; set; }

        [JsonPropertyName("indicesofpickinguppoints")]
        public int[] indicesOfPickingUpPoints { get; set; }

        [JsonPropertyName("indicesofdroppingoffpoints")]
        public int[] indicesOfDroppingOffPoints { get; set; }

        [JsonPropertyName("pickuppenalties")]
        public int[] pickUpPenalties { get; set; }

        [JsonPropertyName("dropoffpenalties")]
        public int[] dropOffPenalties { get; set; }

        [JsonPropertyName("settings")]
        public Settings? Settings { get; set; } = null;

        public override string ToString()
        {
            string distancesStr = Distances != null ? string.Join("; ", Distances.Select(row => string.Join(",", row))) : "null";
            string capacitiesStr = Capacities != null ? string.Join(", ", Capacities) : "null";
            string weightsStr = Weights != null ? string.Join(", ", Weights) : "null";
            string valuesStr = Values != null ? string.Join(", ", Values) : "null";
            string startingPointsStr = indicesOfStartingPoints != null ? string.Join(", ", indicesOfStartingPoints) : "null";
            string endingPointsStr = indicesOfEndingPoints != null ? string.Join(", ", indicesOfEndingPoints) : "null";
            string pickingUpPointsStr = indicesOfPickingUpPoints != null ? string.Join(", ", indicesOfPickingUpPoints) : "null";
            string droppingOffPointsStr = indicesOfDroppingOffPoints != null ? string.Join(", ", indicesOfDroppingOffPoints) : "null";
            string pickUpPenaltiesStr = pickUpPenalties != null ? string.Join(", ", pickUpPenalties) : "null";
            string dropOffPenaltiesStr = dropOffPenalties != null ? string.Join(", ", dropOffPenalties) : "null";
            string settingsStr = Settings?.ToString() ?? "null";

            return $"Distances: [{distancesStr}]\n, Capacities: [{capacitiesStr}]\n Weights: [{weightsStr}]\n Values: [{valuesStr}]\n " +
                   $"IndicesOfStartingPoints: [{startingPointsStr}]\n IndicesOfEndingPoints: [{endingPointsStr}]\n " +
                   $"IndicesOfPickingUpPoints: [{pickingUpPointsStr}]\n IndicesOfDroppingOffPoints: [{droppingOffPointsStr}]\n " +
                   $"PickUpPenalties: [{pickUpPenaltiesStr}]\n DropOffPenalties: [{dropOffPenaltiesStr}]\n Settings: {settingsStr}";
        }
    }

    public class ResponseModel
    {
        public int TotalDistance { get; set; }
        public int[] BestRoute { get; set; }
        public int TotalValue { get; set; }
        public int[] IncludedItems { get; set; }

        public override string ToString()
        {
            string bestRouteStr = BestRoute != null ? string.Join(", ", BestRoute) : "null";
            string includedItemsStr = IncludedItems != null ? string.Join(", ", IncludedItems) : "null";

            return $"TotalDistance: {TotalDistance}, BestRoute: [{bestRouteStr}]\n TotalValue: {TotalValue}, IncludedItems: [{includedItemsStr}]";
        }
    }

    public class ResponseModelForMode3AMore
    {
        public int TotalValue { get; set; }
        public int TotalDistance { get; set; }
        public Truck[] Trucks { get; set; } = [];

        public static ResponseModelForMode3AMore Map(int totalDistance, int[][] bestRoutes, int totalValue, int[][] includedItems, int[][] distances, int[] capacities, int[] values)
        {
            Truck[] trucks = new Truck[capacities.Length];
            for (int i = 0; i < trucks.Length; i++)
            {
                trucks[i] = new Truck(i, capacities[i], includedItems[i], bestRoutes[i], values, distances);
            }
            return new ResponseModelForMode3AMore
            {
                TotalValue = totalValue,
                TotalDistance = totalDistance,
                Trucks = trucks
            };
        }

        public override string ToString()
        {
            string trucksStr = Trucks != null ? string.Join("; ", Trucks.Select(truck => truck.ToString())) : "null";

            return $"TotalValue: {TotalValue}, TotalDistance: {TotalDistance}\n Trucks: [{trucksStr}]";
        }
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

        public override string ToString()
        {
            string includedItemsStr = IncludedItems != null ? string.Join(", ", IncludedItems) : "null";
            string routeStr = Route != null ? string.Join(", ", Route) : "null";

            return $"\n Id: {Id}, Capacity: {Capacity}, Value: {Value}, Distance: {Distance}\n IncludedItems: [{includedItemsStr}]\n Route: [{routeStr}]\n";
        }
    }
}