using Microsoft.Extensions.Caching.Memory;

namespace csca5028BlazorApp2.Data
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    }

    public class MemoryCachingService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCachingService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
           
            //check null and return
            if(_cache.TryGetValue(key, out T value))
            {
                return value;
            }
            else
            {
                return default(T);
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new MemoryCacheEntryOptions();
            if (expiry.HasValue)
            {
                options.SetAbsoluteExpiration(expiry.Value);
            }
            _cache.Set(key, value, options);
        }
    }
    
}
