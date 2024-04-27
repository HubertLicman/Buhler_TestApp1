using Buhler_TestApp1.Controllers;
using Buhler_TestApp1.Data;
using Buhler_TestApp1.Helpers;
using Buhler_TestApp1.Requests;
using Buhler_TestApp1.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace Buhler_TestApp1.Domains
{
    /// <summary>
    /// Represents the domain logic for food trucks.
    /// </summary>
    public class FoodTruckDomain
    {
        private readonly ILogger<FoodTracksController> _logger;
        private readonly SettingsHelpers sh = new();
        private List<FoodTruckData> _foodTruckData = [];

        private const string dataPath = @"Data\Source\";
        private const string fileName = "Mobile_Food_Facility_Permit.csv";
        private const string countSettingKey = "AppSettings:DefaultResultsCount";

        #region ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="FoodTruckDomain"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FoodTruckDomain(ILogger<FoodTracksController> logger)
        {
            _logger = logger;
            // Load data
            loadData();
        }

        #endregion ctor

        #region public methods

        /// <summary>
        /// Gets the distinct food items from the food truck data.
        /// </summary>
        /// <returns>An enumerable of distinct food items.</returns>
        public IEnumerable<string> GetFoodItems() => _foodTruckData
                .Where(x => !string.IsNullOrEmpty(x.FoodItems))
                .Select(x => x.FoodItems)
                .Distinct();

        /// <summary>
        /// Finds the nearest food trucks based on the specified request.
        /// </summary>
        /// <param name="foodTruckRequest">The food truck request.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the nearest food trucks.</returns>
        public ApiResponse FindNearestFoodTracks(FoodTruckRequest foodTruckRequest)
        {
            ApiResponse apiResponse = new();

            try
            {
                // Validate request
                if (foodTruckRequest == null)
                {
                    apiResponse.ExceptionMessage = Properties.Resources.foodTruckRequest;
                    return apiResponse;
                }

                // Validate parameters
                if (foodTruckRequest.Latitude < -90 || foodTruckRequest.Latitude > 90)
                {
                    apiResponse.ExceptionMessage = Properties.Resources.LatitudeIsOutOfRange;
                    return apiResponse;
                }
                if (foodTruckRequest.Longitude < -180 || foodTruckRequest.Longitude > 180)
                {
                    apiResponse.ExceptionMessage = Properties.Resources.LongitudeIsOutOfRange;
                    return apiResponse;
                }
                if (string.IsNullOrEmpty(foodTruckRequest.FoodItem))
                {
                    apiResponse.ExceptionMessage = Properties.Resources.FoodItemIsNullOrEmpty;
                    return apiResponse;
                }
                var foodItems = GetFoodItems();
                if (!foodItems.Contains(foodTruckRequest.FoodItem))
                {
                    apiResponse.ExceptionMessage = Properties.Resources.FoodItemIsNotValid;
                    return apiResponse;
                }
                if (foodTruckRequest.ResultsCount <= 0)
                {
                    apiResponse.ExceptionMessage = Properties.Resources.ResultsCountIsOutOfRange;
                    return apiResponse;
                }

                // OK, data are valid
                var trucksInCategory = _foodTruckData
                    .Where(x => !string.IsNullOrEmpty(x.FoodItems)
                        && x.FoodItems.Contains(foodTruckRequest.FoodItem))
                    .ToList();
                if (trucksInCategory.Count == 0)
                {
                    apiResponse.ExceptionMessage = Properties.Resources.NoTrucksForDefinedCategoryFound;
                    return apiResponse;
                }

                // Calculate distances
                trucksInCategory.ForEach(x =>
                    x.SetDistance(foodTruckRequest.Latitude, foodTruckRequest.Longitude));

                // Order by distance
                trucksInCategory = trucksInCategory
                    .OrderBy(x => x.DistanceInKilometers)
                    .Take(foodTruckRequest.ResultsCount)
                    .ToList();

                // Set success and return data
                apiResponse.Success = true;
                // Here is possible to limit the data to return only the necessary fields
                // Strong or anonymous type can be used
                // Object is sent as serialized JSON
                apiResponse.ResultObject = JsonConvert.SerializeObject(trucksInCategory);
            }
            catch (Exception ex)
            {
                apiResponse.Message = Properties.Resources.ErrorInLookingForNearestTrucks;
                apiResponse.ExceptionMessage = ex.Message;
                throw;
            }

            return apiResponse;
        }

        /// <summary>
        /// Saves the default results count to the application settings.
        /// </summary>
        /// <param name="value">The value to save.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        public ApiResponse SaveDefaultResultsCount(int value)
        {
            return sh.AddOrUpdateAppSetting(countSettingKey, value.ToString());
        }

        /// <summary>
        /// Gets the default results count from the application settings.
        /// </summary>
        /// <returns>The default results count.</returns>
        public int GetDefaultResultsCount()
        {
            try
            {
                var value = sh.GetAppSettingValue(countSettingKey);
                return string.IsNullOrEmpty(value) ? 0 : Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Properties.Resources.ErrorReadingAppSettings);
                throw;
            }
        }

        #endregion public methods

        #region private methods

        /// <summary>
        /// Loads the food truck data from the CSV file.
        /// </summary>
        private void loadData()
        {
            try
            {
                string path = Path.Combine(Environment.CurrentDirectory, dataPath, fileName);
                if (!File.Exists(path))
                {
                    _logger.LogError(Properties.Resources.FileNotFound);
                    return;
                }
                _foodTruckData = File.ReadAllLines(path)
                    .Skip(1)
                    .Select(v => FoodTruckData.FromCsv(v))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, Properties.Resources.ErrorLoadingData);
                throw;
            }
        }

        /// <summary>
        /// Sets the food truck data.
        /// </summary>
        /// <param name="foodTruckData">The food truck data.</param>
        public void SetFoodTruckData(List<FoodTruckData> foodTruckData)
        {
            _foodTruckData = foodTruckData;
        }

        #endregion private methods

    }
}
