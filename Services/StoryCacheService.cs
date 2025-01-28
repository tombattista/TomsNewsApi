using Microsoft.Extensions.Caching.Memory;

namespace TomsNewsApi.Services;

public class StoryCacheService(IMemoryCache memoryCache) : ICacheService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    
    public async Task<T?> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> retrieveDataFunc, TimeSpan? slidingExpiration = null)
    {
        if (!_memoryCache.TryGetValue(cacheKey, out T? cachedData))
        {
            cachedData = await retrieveDataFunc();
            var cacheEntryOptions = new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration ?? TimeSpan.FromMinutes(60) };
            _memoryCache.Set(cacheKey, cachedData, cacheEntryOptions);
        }

        return cachedData;
    }
}
