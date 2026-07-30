using E_commerce.Application.Contracts;
using E_commerce.Domain.Contracts;

namespace E_commerce.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public Task<string?> GetAsync(string cacheKey, CancellationToken ct = default)
        {
            return _cacheRepository.GetAsync(cacheKey, ct);
        }

        public Task SetAsync(
            string cacheKey,
            string cacheValue,
            TimeSpan timeToLive,
            CancellationToken ct = default)
        {
            // cacheValue is already serialized JSON
            return _cacheRepository.SetAsync(cacheKey, cacheValue, timeToLive, ct);
        }
    }
}