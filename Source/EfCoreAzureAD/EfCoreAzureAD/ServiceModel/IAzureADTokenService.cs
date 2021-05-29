using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EfCoreAzureAD.ServiceModel
{
    public interface IAzureADTokenService
    {
        Task<(string AccessToken, DateTimeOffset ExpiresOn)> GetAccessTokenAsync(CancellationToken cancellationToken = default);
    }
}
