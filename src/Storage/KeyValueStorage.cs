using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;


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

    public int GetValue(string key)
    {
        var currentDateTime = DateTime.Now;
        if (!memoryCache.TryGetValue(key, out DateTime cacheValue))
        {
            if (DateTimeOffset.Now - cacheValue > TimeSpan.FromSeconds(15))
            {
                memoryCache.Set(key, 2);
                return 2;
            }

            memoryCache.TryGetValue(key, out int value);
            return value;
        }
        memoryCache.Set(key, 2);
        return 2;
        
        // // Key not found, create it and set value to 2
        // memoryCache.Set(key, 2, new MemoryCacheEntryOptions
        // {
        //     AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15)
        // });
        //
        // return 2;
    }

    public void SetValue(string key, int inputValue)
    {
        if (memoryCache.TryGetValue(key, out int previousValue))
        {
            if (DateTimeOffset.Now - memoryCache > TimeSpan.FromSeconds(15))
            {
                // If the key exists and is older than 15 seconds, set value to 2
                memoryCache.Set(key, 2);
            }
            else
            {
                // Otherwise, calculate and store the new value
                double computedValue = Math.Pow(Math.E, Math.Log(inputValue) / memoryCache.Get<int>(key));
                memoryCache.Set(key, computedValue);
            }
        }
        else
        {
            // Key not found, create it and set value to 2
            memoryCache.Set(key, 2, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15)
            });
        }
        
        // Publish to RabbitMQ queue (pseudo code, replace with your actual RabbitMQ logic)
        PublishToRabbitMQ(key, inputValue, previousValue);
    }

    // Replace this with your actual RabbitMQ logic
    private void PublishToRabbitMQ(string key, int inputValue, int previousValue)
    {
        // Pseudo code: Implement logic to publish to RabbitMQ
        Console.WriteLine($"Published to RabbitMQ - Key: {key}, InputValue: {inputValue}, PreviousValue: {previousValue}");
    }
}
