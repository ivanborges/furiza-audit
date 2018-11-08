using Furiza.Audit.Abstractions;
using Furiza.Audit.SqlServer.Dapper;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFurizaAudit(this IServiceCollection services, AuditConfiguration auditConfiguration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (auditConfiguration == null)
                throw new ArgumentNullException(nameof(auditConfiguration));

            switch (auditConfiguration.Tool.Value)
            {
                case AuditTool.SqlServerWithDapper:
                    if (!(auditConfiguration is AuditConfigurationSqlServer))
                        throw new InvalidOperationException($"Audit configuration object '{auditConfiguration.GetType().Name}' does not match the audit tool '{AuditTool.SqlServerWithDapper.ToString()}'.");

                    services.AddFurizaAuditWithSqlServerAndDapper(auditConfiguration as AuditConfigurationSqlServer);
                    break;                
            }

            return services;
        }
    }
}