using Furiza.Audit.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using FurizaSqlServerDapper = Furiza.Audit.SqlServer.Dapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFurizaAudit(this IServiceCollection services, IConfiguration configuration, string applicationId = null)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var auditConfiguration = configuration.TryGet<AuditConfiguration>().AddApplicationId(applicationId);
            services.AddSingleton(auditConfiguration);

            switch (auditConfiguration.Tool.Value)
            {
                case AuditTool.SqlServerAndDapper:
                    services.AddFurizaAuditWithSqlServerAndDapper(configuration.TryGet<FurizaSqlServerDapper.AuditConfiguration>().AddApplicationId(applicationId) as FurizaSqlServerDapper.AuditConfiguration);
                    break;                
            }

            return services;
        }
    }
}