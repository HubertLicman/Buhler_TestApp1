using Buhler_TestApp1.Controllers;
using Buhler_TestApp1.Data;
using Buhler_TestApp1.Domains;
using Buhler_TestApp1.Helpers;
using Buhler_TestApp1.Requests;
using Buhler_TestApp1.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Buhler_TestApp1.Test
{
    [TestFixture]
    public class FoodTruckDomainTests
    {
        private FoodTruckDomain _foodTruckDomain;
        private Mock<ILogger<FoodTracksController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<FoodTracksController>>();
            _foodTruckDomain = new FoodTruckDomain(_loggerMock.Object);
        }

      
        [Test]
        public void FindNearestFoodTracks_WithValidRequest_ShouldReturnApiResponseWithNearestFoodTrucks()
        {
            // Arrange
            var foodTruckData = new List<FoodTruckData>
            {
                new FoodTruckData { FoodItems = "Burger", Latitude = 37.7749, Longitude = -122.4194 },
                new FoodTruckData { FoodItems = "Pizza", Latitude = 37.7749, Longitude = -122.4194 },
                new FoodTruckData { FoodItems = "Taco", Latitude = 37.7749, Longitude = -122.4194 },
                new FoodTruckData { FoodItems = "Burger", Latitude = 37.7749, Longitude = -122.4194 },
                new FoodTruckData { FoodItems = "Pizza", Latitude = 37.7749, Longitude = -122.4194 },
                new FoodTruckData { FoodItems = "Taco", Latitude = 37.7749, Longitude = -122.4194 }
            };
            _foodTruckDomain.SetFoodTruckData(foodTruckData);

            var request = new FoodTruckRequest
            {
                Latitude = 37.7749,
                Longitude = -122.4194,
                FoodItem = "Burger",
                ResultsCount = 2
            };

            // Act
            var apiResponse = _foodTruckDomain.FindNearestFoodTracks(request);

            // Assert
            var foodTruckDataList = JsonConvert.DeserializeObject<List<FoodTruckData>>(apiResponse.ResultObject);
            Assert.IsTrue(apiResponse.Success);
            if (foodTruckDataList != null && foodTruckDataList.Any())
            {
                Assert.AreEqual(2, foodTruckDataList.Count());
                Assert.IsTrue(foodTruckDataList.All(x => x.FoodItems.Contains("Burger")));
            }
        }

        [Test]
        public void FindNearestFoodTracks_WithNullRequest_ShouldReturnApiResponseWithExceptionMessage()
        {
            // Arrange
            var request = (FoodTruckRequest)null;

            // Act
            var apiResponse = _foodTruckDomain.FindNearestFoodTracks(request);

            // Assert
            Assert.IsFalse(apiResponse.Success);
        }

        [Test]
        public void FindNearestFoodTracks_WithInvalidLatitude_ShouldReturnApiResponseWithExceptionMessage()
        {
            // Arrange
            var request = new FoodTruckRequest
            {
                Latitude = 100,
                Longitude = -122.4194,
                FoodItem = "Burger",
                ResultsCount = 2
            };

            // Act
            var apiResponse = _foodTruckDomain.FindNearestFoodTracks(request);

            // Assert
            Assert.IsFalse(apiResponse.Success);
        }

        [Test]
        public void FindNearestFoodTracks_WithInvalidLongitude_ShouldReturnApiResponseWithExceptionMessage()
        {
            // Arrange
            var request = new FoodTruckRequest
            {
                Latitude = 37.7749,
                Longitude = -200,
                FoodItem = "Burger",
                ResultsCount = 2
            };

            // Act
            var apiResponse = _foodTruckDomain.FindNearestFoodTracks(request);

            // Assert
            Assert.IsFalse(apiResponse.Success);
        }

        [Test]
        public void FindNearestFoodTracks_WithEmptyFoodItem_ShouldReturnApiResponseWithExceptionMessage()
        {
            // Arrange
            var request = new FoodTruckRequest
            {
                Latitude = 37.7749,
                Longitude = -122.4194,
                FoodItem = "",
                ResultsCount = 2
            };

            // Act
            var apiResponse = _foodTruckDomain.FindNearestFoodTracks(request);

            // Assert
            Assert.IsFalse(apiResponse.Success);
        }

        [Test]
        public void FindNearestFoodTracks_WithInvalidFoodItem_ShouldReturnApiResponseWithExceptionMessage()
        {
            // Arrange
            var foodTruckData = new List<FoodTruckData>
            {
                new FoodTruckData { FoodItems = "Burger" },
                new FoodTruckData { FoodItems = "Pizza" },
                new FoodTruckData { FoodItems = "Taco" }
            };
            _foodTruckDomain.SetFoodTruckData(foodTruckData);

            var request = new FoodTruckRequest
            {
                Latitude = 37.7749,
                Longitude = -122.4194,
                FoodItem = "Hotdog",
                ResultsCount = 2
            };

            // Act
            var apiResponse = _foodTruckDomain.FindNearestFoodTracks(request);

            // Assert
            Assert.IsFalse(apiResponse.Success);
        }

        [Test]
        public void FindNearestFoodTracks_WithZeroResultsCount_ShouldReturnApiResponseWithExceptionMessage()
        {
            // Arrange
            var request = new FoodTruckRequest
            {
                Latitude = 37.7749,
                Longitude = -122.4194,
                FoodItem = "Burger",
                ResultsCount = 0
            };

            // Act
            var apiResponse = _foodTruckDomain.FindNearestFoodTracks(request);

            // Assert
            Assert.IsFalse(apiResponse.Success);
        }

        [Test]
        public void FindNearestFoodTracks_WithNoTrucksInCategory_ShouldReturnApiResponseWithExceptionMessage()
        {
            // Arrange
            var foodTruckData = new List<FoodTruckData>
            {
                new FoodTruckData { FoodItems = "Pizza" },
                new FoodTruckData { FoodItems = "Taco" }
            };
            _foodTruckDomain.SetFoodTruckData(foodTruckData);

            var request = new FoodTruckRequest
            {
                Latitude = 37.7749,
                Longitude = -122.4194,
                FoodItem = "Burger",
                ResultsCount = 2
            };

            // Act
            var apiResponse = _foodTruckDomain.FindNearestFoodTracks(request);

            // Assert
            Assert.IsFalse(apiResponse.Success);
        }

       
    }
}
