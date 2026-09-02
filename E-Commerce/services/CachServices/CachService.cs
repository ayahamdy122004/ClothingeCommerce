
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace E_Commerce.services.CachServices
{
    public class CachService : ICachService
    {private readonly IMemoryCache memoryCache;
        public CachService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public Task CachResponseAsync(string cachkey, object Response, TimeSpan expiration)
        {
            if(Response==null)
                {
                return Task.CompletedTask;
            } 
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase
            };  
            var serializedResponse=JsonSerializer.Serialize(Response,options);
            memoryCache.Set(cachkey,serializedResponse,expiration);
            return Task.CompletedTask;  
        }

        public Task<string> GetCachResponseAsync(string cachkey)
        {
            if(memoryCache.TryGetValue(cachkey,out string cachedResponse))
            {
                return Task.FromResult(cachedResponse);
            }
            return Task.FromResult<string>(null);
        }
    }
}
