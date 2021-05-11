using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ondato.Models;

namespace ondato.Services
{
    public interface IDictionaryService
    {
        void AddData(string key, List<Object> value, DateTime? valid);

        List<Object> GetData(string key);

        List<Object> UpdateData(string key, List<Object> value);

        string RemoveData(string key);
    }
     
    public class DictionaryService : IDictionaryService
    {
        private readonly IConfiguration configuration;
        
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        private readonly ICacheService cacheService;

        private int timeout;
        
        public DictionaryService(IConfiguration configuration, ILogger<IDictionaryService> logger, ICacheService cacheService)
        {
            this.configuration = configuration;
            this._logger = logger;
            this.cacheService = cacheService;
            DictionaryModel.MyDictionary = new Dictionary<string, DictionaryViewModel>();
        }

        public void AddData(string key, List<Object> value, DateTime? valid)
        {
            if (valid == null)
            {
                valid = DateTime.Now.AddSeconds(timeout);
            }

            DictionaryModel.MyDictionary.Add(key, new DictionaryViewModel()
            {
                Key = key,
                Data = value,
                Expire = valid.Value,
            });
        }

        public List<Object> GetData(string key)
        {
            try
            {
                var item = DictionaryModel.MyDictionary.GetValueOrDefault(key).Data;
                cacheService.ResetExpiracy(key);
                return item;
            }             
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Object> UpdateData(string key, List<Object> value)
        {
            if (DictionaryModel.MyDictionary.ContainsKey(key))
            {
                DictionaryModel.MyDictionary[key].Data = value;
            }
            return value;
            
        }

        public string RemoveData(string key)
        {
            if (DictionaryModel.MyDictionary.ContainsKey(key)) {
                DictionaryModel.MyDictionary.Remove(key);
            }

            return key;
        }
    }
}
