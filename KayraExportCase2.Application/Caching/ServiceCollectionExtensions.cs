using KayraExportCase2.Application.Abstract;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace KayraExportCase2.Application.Caching
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCacheService(this IServiceCollection services, IConfiguration cfg, string section = "Cache")
        {
            services.Configure<CacheOptions>(cfg.GetSection(section).Bind);
            var opts = cfg.GetSection(section).Get<CacheOptions>() ?? new CacheOptions();

            services.AddStackExchangeRedisCache(o =>
            {
                o.Configuration = opts.RedisConnectionString;
                o.InstanceName = opts.InstanceName;
            });

            services.AddSingleton<ICacheService, RedisCacheService>();
            return services;
        }
    }
}
