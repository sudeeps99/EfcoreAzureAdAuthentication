using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EfCoreAzureAD.ServiceModel
{
    public class AzureADTokenService : IAzureADTokenService
    {
        private const string _cacheKey = nameof(AzureADTokenService);
        private readonly IMemoryCache _cache;
        public AzureADTokenService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public async Task<(string AccessToken, DateTimeOffset ExpiresOn)> GetAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            
            return await _cache.GetOrCreateAsync(_cacheKey, async cacheEntry =>
            {
                var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net//.default" });
                var tokenRequestResult= await new DefaultAzureCredential().GetTokenAsync(tokenRequestContext, cancellationToken);

                // AAD access tokens have a default lifetime of 1 hour, so we take a small safety margin
                cacheEntry.SetAbsoluteExpiration(tokenRequestResult.ExpiresOn.AddMinutes(-10));

                return (tokenRequestResult.Token, tokenRequestResult.ExpiresOn);
            });
        }
    }
}
