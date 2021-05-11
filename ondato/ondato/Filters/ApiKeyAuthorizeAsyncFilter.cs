using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using ondato.Services;

namespace ondato.Filters
{
    public class ApiKeyAuthorizeAsyncFilter : IAsyncAuthorizationFilter
    {
        public string ApiKeyHeaderName = "myhttpheadername";
        public string ClientIdHeaderName = "ClientId";

        private readonly ILogger<ApiKeyAuthorizeAsyncFilter> _logger;
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyAuthorizeAsyncFilter(ILogger<ApiKeyAuthorizeAsyncFilter> logger, IApiKeyService apiKeyService)
        {
            _logger = logger;
            _apiKeyService = apiKeyService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;
            var hasApiKeyHeader = request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyValue);

            if (hasApiKeyHeader)
            {
                _logger.LogDebug("Found the header {ApiKeyHeader}. Starting API Key validation", ApiKeyHeaderName);

                if (apiKeyValue.Count != 0 && !string.IsNullOrWhiteSpace(apiKeyValue))
                {
                    
                    if (await _apiKeyService.IsAuthorized(apiKeyValue))
                    {
                        _logger.LogDebug("Client {ClientId} successfully logged in with key {ApiKey}", "default", apiKeyValue);
                        return;
                    }

                    _logger.LogWarning("ClientId {ClientId} with ApiKey {ApiKey} is not authorized", "default", apiKeyValue);
                }
                else
                {
                    _logger.LogWarning("{HeaderName} header found, but api key was null or empty", ApiKeyHeaderName);
                }
            }
            else
            {
                _logger.LogWarning("No ApiKey header found.");
            }

            context.Result = new UnauthorizedResult();
        }
    }
}