using Furiza.Audit.Abstractions;
using Furiza.Audit.SqlServer.Dapper;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFurizaAuditWithSqlServerAndDapper(this IServiceCollection services, AuditConfigurationSqlServer auditConfigurationSqlServer)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton(auditConfigurationSqlServer ?? throw new ArgumentNullException(nameof(auditConfigurationSqlServer)));
            services.AddTransient<AuditContext>();
            services.AddScoped<IAuditTrailProvider, AuditTrailProvider>();

            return services;
        }
    }
}