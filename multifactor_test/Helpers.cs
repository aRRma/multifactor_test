using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace multifactor_test
{
    public static class Helpers
    {
        public static async Task<T?> GetRequestObjectAsync<T>(HttpContext context)
            where T : new()
        {
            var data = await new StreamReader(context.Request.Body).ReadToEndAsync();

            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch
            {
                return default(T);
            }
        }

        public static bool ValidateRequest<T>(T? request)
        {
            try
            {
                var validContext = new ValidationContext(request);
                var validResult = new List<ValidationResult>();
                Validator.TryValidateObject(request, validContext, validResult, true);
                return !validResult?.Any() ?? true;
            }
            catch
            {
                return false;
            }
        }
    }
}