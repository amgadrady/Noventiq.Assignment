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
                var language = languageHeader.ToString().Split(',').FirstOrDefault()?.Trim();

                
                if (!string.IsNullOrEmpty(language))
                {
                    var primaryLanguage = language.Split('-')[0].ToLowerInvariant();
                    return primaryLanguage;
                }
            }

            return defaultLanguage;
        }
    }
}
