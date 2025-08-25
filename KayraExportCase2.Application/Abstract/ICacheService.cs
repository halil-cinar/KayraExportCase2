using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportCase2.Application.Abstract
{
    public interface ICacheService
    {
        public Task<T?> GetAsync<T>(string key);
        public Task SetAsync<T>(string key, T value, TimeSpan? absoluteTtl = null, TimeSpan? slidingTtl = null);
        public Task<T> GetOrSetAsync<T>(string key, T value, TimeSpan? absoluteTtl = null, TimeSpan? slidingTtl = null);
        public Task<bool> RemoveAsync(string key);

    }
}
