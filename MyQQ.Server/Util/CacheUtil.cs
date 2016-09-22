using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace MyQQ.Server.Util
{
    public class CacheUtil
    {
        private static Cache Cache = HttpRuntime.Cache;

        /// <summary>
        /// Check if the specified cache key exists or not
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static bool Exists(string cacheKey)
        {
            bool isExists = false;

            if (Cache.Get(cacheKey) != null)
            {
                isExists = true;
            }

            return isExists;
        }

        /// <summary>
        /// Add one cache item
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <returns></returns>
        public static bool Add(string cacheKey, object cacheValue)
        {
            DateTime absoluteExpiration = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23,59,59);
            TimeSpan slidingExpiration = TimeSpan.Zero;
            CacheItemRemovedCallback removeCallback = new CacheItemRemovedCallback(onRemove);
            Cache.Add(cacheKey, cacheValue, null, absoluteExpiration, slidingExpiration, CacheItemPriority.AboveNormal, removeCallback);

            return true;
        }

        /// <summary>
        /// Update one Cache item
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <returns></returns>
        public static bool Update(string cacheKey, object cacheValue)
        {
            if (Exists(cacheKey))
            {
                Cache.Remove(cacheKey);
            }

            Add(cacheKey, cacheValue);

            return true;
        }

        /// <summary>
        /// Get one cache item
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static object Get(string cacheKey)
        {
            return Cache.Get(cacheKey);
        }

        /// <summary>
        /// One callback handler when remove one cache item from cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="reason"></param>
        private static void onRemove(string key, object value, CacheItemRemovedReason reason)
        {
            System.Console.WriteLine("Remove cache key: " + key);
            System.Console.WriteLine("The reason: " + reason);
        }
    }
}