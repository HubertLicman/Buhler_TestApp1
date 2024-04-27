using NUnit.Framework;
using Buhler_TestApp1.Data;

namespace Buhler_TestApp1.Test
{
    public class FoodTruckDataTests
    {
        [Test]
        public void FromCsv_ValidCsvLine_ReturnsFoodTruckDataObject()
        {
            // Arrange
            string csvLine = "1,Applicant,Food Truck,CNN,Location Description,Address,Blocklot,Block,Lot,Permit,Status,Food Items,X,Y,37.1234,-122.5678,Schedule,Days and Hours,NOI Sent,Approved,Received,Prior Permit,Expiration Date";

            // Act
            var result = FoodTruckData.FromCsv(csvLine);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Locationid);
            Assert.AreEqual("Applicant", result.Applicant);
            Assert.AreEqual("Food Truck", result.FacilityType);
            Assert.AreEqual("CNN", result.Cnn);
            Assert.AreEqual("Location Description", result.LocationDescription);
            Assert.AreEqual("Address", result.Address);
            Assert.AreEqual("Blocklot", result.Blocklot);
            Assert.AreEqual("Block", result.Block);
            Assert.AreEqual("Lot", result.Lot);
            Assert.AreEqual("Permit", result.Permit);
            Assert.AreEqual("Status", result.Status);
            Assert.AreEqual("Food Items", result.FoodItems);
            Assert.AreEqual("X", result.X);
            Assert.AreEqual("Y", result.Y);
            Assert.AreEqual(37.1234, result.Latitude);
            Assert.AreEqual(-122.5678, result.Longitude);
            Assert.AreEqual("Schedule", result.Schedule);
            Assert.AreEqual("Days and Hours", result.Dayshours);
            Assert.AreEqual("NOI Sent", result.NOISent);
            Assert.AreEqual("Approved", result.Approved);
            Assert.AreEqual("Received", result.Received);
            Assert.AreEqual("Prior Permit", result.PriorPermit);
            Assert.AreEqual("Expiration Date", result.ExpirationDate);
        }

        [Test]
        public void SetDistance_ValidCoordinates_CalculatesDistanceInKilometers()
        {
            // Arrange
            var foodTruckData = new FoodTruckData
            {
                Latitude = 37.1234,
                Longitude = -122.5678
            };
            double targetLatitude = 37.4321;
            double targetLongitude = -122.8765;

            // Act
            foodTruckData.SetDistance(targetLatitude, targetLongitude);

            // Assert
            Assert.AreEqual(44, foodTruckData.DistanceInKilometers, 1.001);
        }
    }
}
