using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ondato.Models;

namespace ondato.Services
{
    public interface ICacheService
    {
        void ResetExpiracy(string key);

        void Cleanup();

    }

    public class CacheService : ICacheService
    {
        private readonly IConfiguration configuration;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        
        public CacheService(IConfiguration configuration, ILogger<ICacheService> logger)
        {
            this.configuration = configuration;
            this._logger = logger;
        }

        public void ResetExpiracy(string key)
        {
            if (DictionaryModel.MyDictionary.ContainsKey(key)) {
                DictionaryModel.MyDictionary[key].Expire = null;
            }
        }

        public void Cleanup()
        {
            List<string> keysForRemoval = new List<string>();

            int maxExpiration = configuration.GetSection("Dictionary").GetValue<int>("MaxExpiration");

            foreach (var item in DictionaryModel.MyDictionary)
            {
                if (item.Value.Expire != null)
                {
                    if (item.Value.Expire > DateTime.Now.AddSeconds(maxExpiration)) {
                        keysForRemoval.Add(item.Key);
                    }
                }
            }

            foreach (var item in keysForRemoval)
            {
                DictionaryModel.MyDictionary.Remove(item);
            }
        }
    }
}
