using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CacheManager.Core;
using CacheManager.Core.Internal;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace TestAppInsightsPerformance
{
    public class CacheDataClient
    {
        private readonly ICacheManager<IList<WorkLocation>> _cache;
        //private readonly ILogger _logger;
        private readonly DataClient _dataClient;
        private readonly string _key = "list";
        private readonly string _refresherKey = "refresher";
        private readonly TimeSpan _timeout;

        public CacheDataClient(DataClient dataClient/*, ILogger log*/, ICacheManager<IList<WorkLocation>> cache)
        {
            _dataClient = dataClient;
            //_logger = log;
            _cache = cache;
            _timeout = new TimeSpan(0, 5, 0);

            _cache.OnRemoveByHandle += CacheOnOnRemoveByHandle;
        }

        private void CacheOnOnRemoveByHandle(object sender, CacheItemRemovedEventArgs cacheItemRemovedEventArgs)
        {
            var unused = Refresh(); //fire-and-forget, we will lose exceptions
        }

        public async Task<IList<WorkLocation>> GetAll()
        {
            return await GetAllFromCache();
        }

        protected async Task<IList<WorkLocation>> FetchAll()
        {
            var locations = await _dataClient.GetTestData2();
            return locations;
        }

        protected async Task<IList<WorkLocation>> GetAllFromCache()
        {
            var cachedList = _cache.Get(_key);
            if (cachedList == null)
            {
                //_logger.LogInformation("cache empty, calling refresh to populate");
                return await Refresh();
            }
            return cachedList;
        }

        public virtual async Task Prepopuluate()
        {
            await GetAllFromCache();
        }

        private async Task<IList<WorkLocation>> Refresh()
        {
            //throwing an exception in this method will DO nothing because it's called as a fire-and-forget async
            //method
            //must log anything that would normally be an exception
            try
            {
                //_logger.LogInformation("refreshing cache, calling FetchAll");
                var list = await FetchAll();
                //work with ListResult instead and log error if .HasNextPage returns true
                if (list == null || !list.Any())
                {
                    //_logger.LogError("refreshing cache, FetchAll failed to return any results");
                    return null; //this should cause NullPointerException elsewhere, which is what we desire
                }

                //_logger.LogInformation("refreshing cache, populating with {count} items", list.Count);
                _cache.AddOrUpdate(_key, list, _ => list);

                return list;
            }
            catch (Exception ex)
            {
                //_logger.LogError(500, ex, "refreshing cache, exception from FetchAll");
                return null; //this should cause NullPointerException elsewhere, which is what we desire
            }
            finally
            {
                _cache.Remove(_refresherKey);
                var refresher = new CacheItem<IList<WorkLocation>>(_refresherKey, new List<WorkLocation>(), ExpirationMode.Absolute, _timeout);
                _cache.Add(refresher);
            }
        }
    }
}