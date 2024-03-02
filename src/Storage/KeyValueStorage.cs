using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;
using Webapi.Services;
using Webapi.Types.Constant;


namespace Webapi.Storage;
    
public class KeyValueStorage(IMemoryCache memoryCache): IKeyValueStorage
{
    public int OnGet(string key)
    {
        var currentDateTime = DateTime.Now;

        if (!memoryCache.TryGetValue(key , out DateTime cacheValue))
        {
            cacheValue = currentDateTime;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30));

            memoryCache.Set(key, cacheValue, cacheEntryOptions);
        }

        
        return int.Parse(key);
    }

    public decimal? GetValue(string key)
    {
        var exists = memoryCache.TryGetValue(key, out CacheItem? cacheItem);
        if (!exists || cacheItem == null)
            return null;
            
        var result = DateTime.Now - cacheItem.timestamp;
        if (result.Seconds > Constants.MaxSecondsOldCacheEntry)
            return null;
            
        return cacheItem.value;
    }

    public void SetValue(string key, decimal value)
    {
        memoryCache.Set<CacheItem>(key, new CacheItem(value, DateTime.Now));
    }
    
    private class CacheItem
    {
        public CacheItem(decimal value, DateTime timestamp)
        {
            this.value = value; 
            this.timestamp = timestamp;
        }

        public decimal value { get; set; }
        public DateTime timestamp { get; set; }
    }
}
