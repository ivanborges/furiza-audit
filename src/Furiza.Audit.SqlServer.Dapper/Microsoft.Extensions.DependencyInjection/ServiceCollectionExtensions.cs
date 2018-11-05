using Microsoft.Extensions.DependencyInjection;
using System;

namespace Furiza.Audit.SqlServer.Dapper.Microsoft.Extensions.DependencyInjection
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