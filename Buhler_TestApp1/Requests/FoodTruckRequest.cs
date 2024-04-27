namespace Buhler_TestApp1.Requests
{
    /// <summary>
    /// Represents a food truck request.
    /// </summary>
    public class FoodTruckRequest
    {
        /// <summary>
        /// Gets or sets the latitude of the food truck location.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the food truck location.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the food item requested.
        /// </summary>
        public string FoodItem { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of results to return.
        /// </summary>
        public int ResultsCount { get; set; }
    }
}
