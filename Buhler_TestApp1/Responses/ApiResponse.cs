using Newtonsoft.Json.Linq;

namespace Buhler_TestApp1.Responses
{
    /// <summary>
    /// Represents the response from an API.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the API response is successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the message associated with the API response.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the data returned by the API response.
        /// </summary>
        public string ResultObject { get; set; }

        /// <summary>
        /// Gets or sets the exception message if an exception occurred during the API call.
        /// </summary>
        public string ExceptionMessage { get; set; }
    }
}
