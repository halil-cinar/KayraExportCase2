using KayraExportCase2.Application.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KayraExportCase2.Application.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly CacheOptions _opts;
        private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

        public RedisCacheService(IDistributedCache cache, IOptions<CacheOptions> opts)
        {
            _cache = cache;
            _opts = opts.Value;
        }

        private string K(string key) => $"{_opts.InstanceName}{key}";

        public async Task<T?> GetAsync<T>(string key)
        {
            var bytes = await _cache.GetAsync(K(key));
            if (bytes is null || bytes.Length == 0) return default;
            return JsonSerializer.Deserialize<T>(bytes, JsonOpts);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteTtl = null, TimeSpan? slidingTtl = null)
        {
            var options = new DistributedCacheEntryOptions();
            var abs = absoluteTtl ?? TimeSpan.FromSeconds(_opts.DefaultAbsoluteSeconds);
            if (abs > TimeSpan.Zero) options.AbsoluteExpirationRelativeToNow = abs;
            var slide = slidingTtl ?? (_opts.DefaultSlidingSeconds > 0 ? TimeSpan.FromSeconds(_opts.DefaultSlidingSeconds) : TimeSpan.Zero);
            if (slide > TimeSpan.Zero) options.SlidingExpiration = slide;
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonOpts);
            await _cache.SetAsync(K(key), bytes, options);
        }

        public async Task<T> GetOrSetAsync<T>(string key, T value, TimeSpan? absoluteTtl = null, TimeSpan? slidingTtl = null)
        {
            var cached = await GetAsync<T>(key);
            if (cached is not null && !Equals(cached, default(T))) return cached;
            await SetAsync(key, value, absoluteTtl, slidingTtl);
            return value;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            await _cache.RemoveAsync(K(key));
            return true;
        }
    }
}
