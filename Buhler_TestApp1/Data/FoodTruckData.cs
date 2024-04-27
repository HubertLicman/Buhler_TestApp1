using System.Globalization;
using Buhler_TestApp1.Domains;
using Microsoft.Spatial;



namespace Buhler_TestApp1.Data
{
    /// <summary>
    /// Represents the data of a food truck.
    /// </summary>
    public class FoodTruckData
    {
        /// <summary>
        /// Gets or sets the location ID.
        /// </summary>
        public int Locationid { get; set; }

        /// <summary>
        /// Gets or sets the applicant name.
        /// </summary>
        public string? Applicant { get; set; }

        /// <summary>
        /// Gets or sets the facility type.
        /// </summary>
        public string? FacilityType { get; set; }

        /// <summary>
        /// Gets or sets the CNN.
        /// </summary>
        public string? Cnn { get; set; }

        /// <summary>
        /// Gets or sets the location description.
        /// </summary>
        public string? LocationDescription { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the blocklot.
        /// </summary>
        public string? Blocklot { get; set; }

        /// <summary>
        /// Gets or sets the block.
        /// </summary>
        public string? Block { get; set; }

        /// <summary>
        /// Gets or sets the lot.
        /// </summary>
        public string? Lot { get; set; }

        /// <summary>
        /// Gets or sets the permit.
        /// </summary>
        public string? Permit { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the food items.
        /// </summary>
        public string? FoodItems { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        public string? X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        public string? Y { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the schedule.
        /// </summary>
        public string? Schedule { get; set; }

        /// <summary>
        /// Gets or sets the days and hours.
        /// </summary>
        public string? Dayshours { get; set; }

        /// <summary>
        /// Gets or sets the NOI sent.
        /// </summary>
        public string? NOISent { get; set; }

        /// <summary>
        /// Gets or sets the approved status.
        /// </summary>
        public string? Approved { get; set; }

        /// <summary>
        /// Gets or sets the received date.
        /// </summary>
        public string? Received { get; set; }

        /// <summary>
        /// Gets or sets the prior permit.
        /// </summary>
        public string? PriorPermit { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        public string? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public Geography? Location { get; set; }

        /// <summary>
        /// Gets or sets the distance.
        /// </summary>
        public double DistanceInKilometers { get; set; }

        /// <summary>
        /// Converts a CSV line to a FoodTruckData object.
        /// </summary>
        /// <param name="csvLine">The CSV line.</param>
        /// <returns>The FoodTruckData object.</returns>
        public static FoodTruckData FromCsv(string csvLine)
        {
            FoodTruckData foodTruckData = new();

            try
            {
                if (!string.IsNullOrEmpty(csvLine))
                {
                    string[] values = csvLine.Split(',');

                    //Process only when location data are defined
                    if (values != null
                        && !string.IsNullOrEmpty(values[14])
                        && !string.IsNullOrEmpty(values[15]))
                    {
                        // Prepare for parsing
                        NumberFormatInfo provider = new();
                        provider.NumberDecimalSeparator = ".";

                        foodTruckData = new()
                        {
                            Locationid = Convert.ToInt32(values[0]),
                            Applicant = values[1],
                            FacilityType = values[2],
                            Cnn = values[3],
                            LocationDescription = values[4],
                            Address = values[5],
                            Blocklot = values[6],
                            Block = values[7],
                            Lot = values[8],
                            Permit = values[9],
                            Status = values[10],
                            FoodItems = values[11],
                            X = values[12],
                            Y = values[13],
                            Latitude = Convert.ToDouble(values[14], provider),
                            Longitude = Convert.ToDouble(values[15], provider),
                            Schedule = values[16],
                            Dayshours = values[17],
                            NOISent = values[18],
                            Approved = values[19],
                            Received = values[20],
                            PriorPermit = values[21],
                            ExpirationDate = values[22],
                            Location = GeographyPoint.Create(Convert.ToDouble(values[14], provider), Convert.ToDouble(values[15], provider))
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Any logger should be applied here.
                // I'm using Azure App insight normally
                throw;
            }

            return foodTruckData;
        }

        /// <summary>
        /// Sets the distance between the food truck and a given latitude and longitude.
        /// Returned value is in kilometers
        /// </summary>
        /// <param name="latitude">The latitude of the target location.</param>
        /// <param name="longitude">The longitude of the target location.</param>
        public void SetDistance(double latitude, double longitude)
        {
            DistanceInKilometers = 
                CalculateGeoDistance.GreatCircleDistance(Longitude, Latitude, longitude, latitude) / 1000;
        }
    }
}
