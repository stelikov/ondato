using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ondato.Services
{
    public interface IApiKeyService
    {
        Task<bool> IsAuthorized( string apiKey);
    }

    public class ApiKeyService : IApiKeyService
    {
        private readonly IConfiguration configuration;
        
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
       
        public ApiKeyService(IConfiguration configuration, ILogger<IDictionaryService> logger)
        {
            this.configuration = configuration;
            this._logger = logger;
        }

        public Task<bool> IsAuthorized(string apiKey){

            string apiKeyFromConfig = this.configuration.GetSection("Dictionary").GetValue<string>("ApiKey"); // Encoded !!!:) 

            if (apiKey == apiKeyFromConfig)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }
    }
}
