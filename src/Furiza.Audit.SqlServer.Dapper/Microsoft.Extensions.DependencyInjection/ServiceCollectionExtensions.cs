using Furiza.Audit.Abstractions;
using Furiza.Audit.SqlServer.Dapper;
using System;
using FurizaSqlServerDapper = Furiza.Audit.SqlServer.Dapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFurizaAuditWithSqlServerAndDapper(this IServiceCollection services, FurizaSqlServerDapper.AuditConfiguration auditConfigurationSqlServer)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton(auditConfigurationSqlServer ?? throw new ArgumentNullException(nameof(auditConfigurationSqlServer)));
            services.AddTransient<AuditContext>();
            services.AddScoped<IAuditTrailProvider, AuditTrailProvider>();
            services.AddScoped<AuditSqlServerDapperInitializer>();

            return services;
        }
    }
}