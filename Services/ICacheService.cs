namespace TomsNewsApi.Services;

public interface ICacheService
{
    public Task<T?> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> retrieveDataFunc, TimeSpan? slidingExpiration = null);
}
