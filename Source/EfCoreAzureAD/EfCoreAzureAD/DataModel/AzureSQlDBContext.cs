using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfCoreAzureAD.DataModel
{
    public class AzureSQlDBContext :DbContext
    {
        public AzureSQlDBContext(DbContextOptions<AzureSQlDBContext> options) : base(options)
        {
        }

    }
}
