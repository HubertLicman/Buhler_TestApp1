using NUnit.Framework;
using Buhler_TestApp1.Domains;

namespace Buhler_TestApp1.Test
{
    public class CalculateGeoDistanceTests
    {
        [Test]
        public void GreatCircleDistance_ReturnsCorrectDistance()
        {
            // Arrange
            double lon1 = 0;
            double lat1 = 0;
            double lon2 = 1;
            double lat2 = 1;
            double expectedDistance = 157249.38127194397;

            // Act
            double actualDistance = CalculateGeoDistance.GreatCircleDistance(lon1, lat1, lon2, lat2);

            // Assert
            Assert.AreEqual(expectedDistance, actualDistance, 0.0001);
        }

        [Test]
        public void Radians_ConvertsDegreesToRadians()
        {
            // Arrange
            double degrees = 180;
            double expectedRadians = 3.141592653589793;

            // Act
            double actualRadians = CalculateGeoDistance.Radians(degrees);

            // Assert
            Assert.AreEqual(expectedRadians, actualRadians, 0.0001);
        }
    }
}
