using Buhler_TestApp1.Domains;
using Buhler_TestApp1.Requests;
using Buhler_TestApp1.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Buhler_TestApp1.Controllers
{
    /// <summary>
    /// Controller for managing food tracks.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FoodTracksController : ControllerBase
    {
        private readonly ILogger<FoodTracksController> _logger;
        private readonly FoodTruckDomain _foodTruckDomain;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoodTracksController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="foodTruckDomain">The food truck domain.</param>
        public FoodTracksController(ILogger<FoodTracksController> logger)
        {
            _logger = logger;
            _foodTruckDomain =  new(logger);
        }

        /// <summary>
        /// Gets the list of food items.
        /// </summary>
        /// <returns>The list of food items.</returns>
        [HttpGet("fooditems")]
        public IEnumerable<string> GetFoodItems()
        {
            return _foodTruckDomain.GetFoodItems();
        }

        /// <summary>
        /// Saves the default results count.
        /// </summary>
        /// <param name="value">The default results count value.</param>
        /// <returns>The API response.</returns>
        [HttpPost("defaultresultscount/{value}")]
        public ApiResponse SaveDefaultResultsCount(int value)
        {
            return _foodTruckDomain.SaveDefaultResultsCount(value);
        }

        /// <summary>
        /// Gets the default results count.
        /// </summary>
        /// <returns>The default results count.</returns>
        [HttpGet("defaultresultscount")]
        public int GetDefaultResultsCount()
        {
            return _foodTruckDomain.GetDefaultResultsCount();
        }

        /// <summary>
        /// Finds the nearest food tracks.
        /// </summary>
        /// <param name="foodTruckRequest">The food truck request.</param>
        /// <returns>The API response.</returns>
        [HttpPost("nearestfoodtracks")]
        public ApiResponse FindNearestFoodTracks(FoodTruckRequest foodTruckRequest)
        {
            return _foodTruckDomain.FindNearestFoodTracks(foodTruckRequest);
        }
    }
}
