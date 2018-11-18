using Furiza.Audit.SqlServer.Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder RunFurizaAuditSqlServerDapperInitializer(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));

            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                serviceScope.ServiceProvider.GetService<AuditSqlServerDapperInitializer>().Initialize();                       

            return applicationBuilder;
        }        
    }
}