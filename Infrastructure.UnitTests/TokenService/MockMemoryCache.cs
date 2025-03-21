using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.UnitTests.TokenService
{
    public class MockMemoryCache : IMemoryCache
    {
        private Dictionary<string, object> _cache = new Dictionary<string, object>();

        public ICacheEntry CreateEntry(object key)
        {
            return new MockMemoryCacheEntry(key, this);
        }

        public void Set(string key, object value, TimeSpan expiration)
        {
            _cache[key] = value;
        }

        public void Dispose() { }

        public void Remove(object key)
        {
            _cache.Remove(key.ToString());
        }

        public bool TryGetValue(object key, out object? value)
        {
            if (_cache.ContainsKey(key.ToString()))
            {
                value = _cache[key.ToString()];
                return true;
            }

            value = null;
            return false;
        }
    }

    public class MockMemoryCacheEntry : ICacheEntry
    {
        private readonly object _key;
        private readonly MockMemoryCache _cache;
        private object _value;

        public MockMemoryCacheEntry(object key, MockMemoryCache cache)
        {
            _key = key;
            _cache = cache;
        }

        public object Key => _key;
        public object Value { get => _value; set => _value = value; }
        public DateTimeOffset? AbsoluteExpiration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpan? SlidingExpiration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IList<IChangeToken> ExpirationTokens => throw new NotImplementedException();

        public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks => throw new NotImplementedException();

        public CacheItemPriority Priority { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long? Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        // Implement other properties and methods...

        public void Dispose()
        {
            // Add the entry to the cache when disposed
            _cache.Set(_key.ToString(), _value, new TimeSpan(0, 30, 0));
        }
    }
}
