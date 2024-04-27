using Buhler_TestApp1.Responses;
using Newtonsoft.Json;

namespace Buhler_TestApp1.Helpers
{
    /// <summary>
    /// Helper class for managing application settings.
    /// </summary>
    public class SettingsHelpers
    {
        /// <summary>
        /// Adds or updates an application setting.
        /// </summary>
        /// <typeparam name="T">The type of the setting value.</typeparam>
        /// <param name="sectionPathKey">The section path key of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        public ApiResponse AddOrUpdateAppSetting<T>(string sectionPathKey, T value)
        {
            ApiResponse apiResponse = new();

            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                string json = File.ReadAllText(filePath);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);

                SetValueRecursively(sectionPathKey, jsonObj, value);

                string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, output);

                apiResponse.Success = true;
                apiResponse.ResultObject = JsonConvert.SerializeObject(value);
            }
            catch (Exception ex)
            {
                apiResponse.Message = Properties.Resources.ErrorWritingAppSettings;
                apiResponse.ExceptionMessage = ex.Message;
                throw;
            }

            return apiResponse;
        }

        /// <summary>
        /// Gets the value of an application setting.
        /// </summary>
        /// <param name="key">The key of the setting.</param>
        /// <returns>The value of the setting.</returns>
        public string GetAppSettingValue(string key)
        {
            var appSettingsJsonFilePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "appsettings.json");
            var json = System.IO.File.ReadAllText(appSettingsJsonFilePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);
            var remainingSections = key.Split(":", 2);
            return jsonObj[remainingSections[0]][remainingSections[1]].ToString();
        }

        private void SetValueRecursively<T>(string sectionPathKey, dynamic jsonObj, T value)
        {
            // split the string at the first ':' character
            var remainingSections = sectionPathKey.Split(":", 2);

            var currentSection = remainingSections[0];
            if (remainingSections.Length > 1)
            {
                // continue with the procress, moving down the tree
                var nextSection = remainingSections[1];
                SetValueRecursively(nextSection, jsonObj[currentSection], value);
            }
            else
            {
                // we've got to the end of the tree, set the value
                jsonObj[currentSection] = value;
            }
        }
    }
}