using Furiza.Audit.Abstractions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseFurizaAuditIpAddressRetriever(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));

            applicationBuilder.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            return applicationBuilder;
        }

        public static IApplicationBuilder RunFurizaAuditInitializer(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));

            var auditConfiguration = applicationBuilder.ApplicationServices.GetService<AuditConfiguration>();
            switch (auditConfiguration.Tool.Value)
            {
                case AuditTool.SqlServerAndDapper:
                    applicationBuilder.RunFurizaAuditSqlServerDapperInitializer();
                    break;
            }

            return applicationBuilder;
        }
    }
}