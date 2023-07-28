using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace multifactor_test
{
    // Бесполезный класс
    public static class Helpers
    {
        public static async Task<T?> GetRequestObjectAsync<T>(HttpContext context)
            where T : new()
        {
            var data = await new StreamReader(context.Request.Body).ReadToEndAsync();

            try
            {
                // нет смысла затаскивать эту либу, если есть System.Text.Json. ты же не пишешь никаких кастомных сериализаторов, конвертеров и тп. И даже в этом случае хватает стандартной либы. Лишняя зависимость
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch
            {
                // default - можно и так. Не забывай обращать внимание на рекомендации вижака
                return default(T);
            }
        }

        // Наверно, я желал бы видеть это где-то в биндере, да еще чтобы он возвращал 400 автоматически. Не уверен, но как-то в этом духе.
        // Другое решение - использовать полноценные контроллеры вместо минимал апи. Тогда атрибуты валидации будут работать и ответ 400 будет возвращаться автоматически - см. PingController
        public static bool ValidateRequest<T>(T? request)
        {
            try
            {
                // request не проверяется null, хотя тип T?
                // В конструкторе ValidationContext даже сказано, что возможен выстрел ArgumentNullException
                // И даже студия подчеркивает)
                var validContext = new ValidationContext(request);
                var validResult = new List<ValidationResult>();
                Validator.TryValidateObject(request, validContext, validResult, true);
                // Разве validResult может быть null? Ты ж его сам создаешь
                return !validResult?.Any() ?? true;
            }
            catch
            {
                return false;
            }
        }
    }
}