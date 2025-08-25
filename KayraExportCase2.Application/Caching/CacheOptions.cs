using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportCase2.Application.Caching
{
    public sealed class CacheOptions
    {
        public string RedisConnectionString { get; set; } = "localhost:6379";
        public string InstanceName { get; set; } = "app:";
        public int DefaultAbsoluteSeconds { get; set; } = 3600;
        public int DefaultSlidingSeconds { get; set; } = 0;
    }
}
