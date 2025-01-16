using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityService.Services
{
    public class ErrorFormater
    {
        public static IEnumerable<object> Create(ModelStateDictionary ms)
        {
            var errors = ms.Where(ms => ms.Value?.Errors.Any() ?? false)
                    .Select(ms => new
                    {
                        field = ms.Key,
                        code = ms.Value!.Errors.First().ErrorMessage,
                    });

            return errors;
        }
    }
}
