using EfCoreAzureAD.ServiceModel;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EfCoreAzureAD
{
    public class AadAuthenticationInterceptor : DbConnectionInterceptor
    {
        private readonly IAzureADTokenService _tokenService;
        public AadAuthenticationInterceptor(IAzureADTokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result,
            CancellationToken cancellationToken)
        {
            var sqlConnection = (SqlConnection)connection;

            var connectionStringBuilder = new SqlConnectionStringBuilder(sqlConnection.ConnectionString);
            if (connectionStringBuilder.DataSource.Contains("database.windows.net", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(connectionStringBuilder.UserID))
            {
                var (token, _) = await _tokenService.GetAccessTokenAsync(cancellationToken);
                sqlConnection.AccessToken = token;
            }

            return await base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
        }
    }
}
