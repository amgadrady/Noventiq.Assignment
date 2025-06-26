using Microsoft.AspNetCore.Http;

namespace NoventiqAssignment.Services
{
    public class MultiLanguageService : IMultiLanguageService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public MultiLanguageService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentLanguage()
        {
            const string defaultLanguage = "en";
            var request = httpContextAccessor.HttpContext?.Request;
            if (request == null) return defaultLanguage;

            if (request.Headers.TryGetValue("Accept-Language", out var languageHeader))
            {
                return languageHeader.ToString().Split(',').FirstOrDefault()?.Trim() ?? defaultLanguage;
            }

            return defaultLanguage;
        }
    }
}
