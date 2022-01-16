using System;
using System.Collections.Generic;
using System.Reflection;

namespace NoZ.Tweenz.Internals
{
    internal struct ProviderCache<TKey, TProvider> where TProvider : TweenProvider
    {
        private static Dictionary<TKey, TProvider> _cache;

        public static TProvider Get(TKey key)
        {
            if (null == _cache)
                _cache = new Dictionary<TKey, TProvider>();
            else if (_cache.TryGetValue(key, out var existingProvider))
                return existingProvider;

            var newProvider = (TProvider)Activator.CreateInstance(
                typeof(TProvider), 
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, 
                null, 
                new object[] { key }, 
                null, 
                null);

            _cache.Add(key, newProvider);

            return newProvider;
        }
    }
}
