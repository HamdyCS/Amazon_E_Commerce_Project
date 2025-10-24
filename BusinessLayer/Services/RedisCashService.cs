using BusinessLayer.Contracks;
using BusinessLayer.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BusinessLayer.Servicese
{
    public class RedisCashService : IRedisCashService
    {
        private readonly ILogger<RedisCashService> _logger;
        private readonly IDistributedCache _distributedCache;

        public RedisCashService(ILogger<RedisCashService> logger, IDistributedCache distributedCache)
        {
            _logger = logger;
            _distributedCache = distributedCache;
        }
        public async Task<T?> GetValueByKeyAsync<T>(string key)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(key, nameof(key));

            try
            {
                var stringValue = await _distributedCache.GetStringAsync(key);
                if (stringValue == null)
                {
                    return default;
                }

                return JsonSerializer.Deserialize<T>(stringValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetValueByKeyAsync with key: {Key}. {message}", key, ex.Message);
                throw;
            }
        }

        public async Task SetValueByKeyAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(key, nameof(key));
            ParamaterException.CheckIfObjectIfNotNull(value, nameof(value));

            try
            {
                var stringValue = JsonSerializer.Serialize(value);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiry
                };

                await _distributedCache.SetStringAsync(key, stringValue, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SetValueByKeyAsync with key: {Key}. {message}", key, ex.Message);
                throw;
            }
        }
    }
}
