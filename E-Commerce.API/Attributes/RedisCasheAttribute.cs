using E_commerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce.API.Attributes
{
    public class RedisCasheAttribute : ActionFilterAttribute
    {
        private readonly int _durationSec;

        public RedisCasheAttribute(int durationSec = 90)
        {
            _durationSec = durationSec;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //get cache service from Container [Not Injection Direct Into Constructor]
            var casheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheKey = CreateCasheKey(context.HttpContext.Request);
            var cashed=await casheService.GetAsync(cacheKey);
            // if data exist InCahe => return data from cache and skip endpoint
            if(!string.IsNullOrEmpty(cashed))
            {
                context.Result = new ContentResult
                {
                    Content = cashed,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            //if not exist => execute endpoint and store data in cache if Result is Ok
            var executed = await next.Invoke();
            if (executed.Result is OkObjectResult { Value: not null } ok)
            {
                var serializedValue = System.Text.Json.JsonSerializer.Serialize(ok.Value);
                await casheService.SetAsync(cacheKey, serializedValue, TimeSpan.FromSeconds(_durationSec));
            }



        }

        private string CreateCasheKey(HttpRequest request)
        {
            //api/products?page=1&size=10/
            //api/products?page=1&size=10/ user 01
            //api/products?size=10&page=1/ user 02
            var key =new StringBuilder();
            key.Append(request.Path).Append('?');
            foreach (var (k, v) in request.Query.OrderBy(q => q.Key))
            {
                key.Append(k).Append('=').Append(v).Append('&');
            }
            return key.ToString();
        }
    }
}
