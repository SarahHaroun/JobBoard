using System.Text;
using JobBoard.Repositories.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JobBoard.API.Helpers
{
    public class CachedAttribute : Attribute , IAsyncActionFilter
    {
        private readonly int _expireTimeInSec;

        public int Duration { get; set; } = 60;
       
        public CachedAttribute(int ExpireTimeInSec)
        {
            _expireTimeInSec = ExpireTimeInSec;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IRedisService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedResponse = await cacheService.GetAsync<string>(cacheKey);
            if(!string.IsNullOrEmpty(cachedResponse))
            {
                context.Result = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                return;
            }
            // Proceed to the next action in the pipeline
            var executedContext = await next.Invoke();   //will execute the EndPoint
            if(executedContext.Result is OkObjectResult)
            {
                var okResult = executedContext.Result as OkObjectResult;
                if (okResult != null)
                {
                    await cacheService.SetAsync(cacheKey, okResult.Value, TimeSpan.FromSeconds(_expireTimeInSec));
                }
            } 
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path); //api/controller

            foreach(var (key , value) in request.Query.OrderBy(x=>x.Key))
            {

                keyBuilder.Append($"|{key}-{value}");

            }
            return keyBuilder.ToString();

        }
    }
    
}
