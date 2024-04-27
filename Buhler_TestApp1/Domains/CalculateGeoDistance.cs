namespace Buhler_TestApp1.Domains
{
    /// <summary>
    /// Provides methods for calculating the great circle distance between two points on Earth.
    /// </summary>
    public class CalculateGeoDistance
    {
        #region public methods

        /// <summary>
        /// Calculates the great circle distance between two points on Earth using their longitude and latitude coordinates.
        /// Used the cartesian distance formula.
        /// </summary>
        /// <param name="lon1">The longitude of the first point.</param>
        /// <param name="lat1">The latitude of the first point.</param>
        /// <param name="lon2">The longitude of the second point.</param>
        /// <param name="lat2">The latitude of the second point.</param>
        /// <returns>The distance between the two points in meters.</returns>
        public static double GreatCircleDistance(double lon1, double lat1, double lon2, double lat2)
        {
            double R = 6371e3; // m

            double sLat1 = Math.Sin(Radians(lat1));
            double sLat2 = Math.Sin(Radians(lat2));
            double cLat1 = Math.Cos(Radians(lat1));
            double cLat2 = Math.Cos(Radians(lat2));
            double cLon = Math.Cos(Radians(lon1) - Radians(lon2));

            double cosD = sLat1 * sLat2 + cLat1 * cLat2 * cLon;

            double d = Math.Acos(cosD);

            double dist = R * d;

            return dist;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">The value in degrees.</param>
        /// <returns>The value in radians.</returns>
        public static double Radians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        #endregion public methods
    }
}
