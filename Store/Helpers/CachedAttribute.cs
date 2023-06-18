using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Store.Core.Services;

namespace Store.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLiveInSeconds;
        public CachedAttribute(int timeToLiveInSeconds)
        {
            this.timeToLiveInSeconds = timeToLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            // Check if the response is already cached
            var cachedResponse = await cacheService.GetCachedResponse(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var response = JsonConvert.DeserializeObject(cachedResponse);
                var objectResult = new ObjectResult(response)
                {
                    StatusCode = 200
                };
                context.Result = objectResult;
                return;
            }

            // If the response is not cached, execute the action
            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                var response = okObjectResult.Value;
                var responseJson = JsonConvert.SerializeObject(response);

                await cacheService.CacheResponseAsync(cacheKey, responseJson, TimeSpan.FromSeconds(timeToLiveInSeconds));
            }
        }
        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}