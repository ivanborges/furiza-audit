using Furiza.Audit.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Furiza.Audit.SqlServer.Dapper
{
    internal class AuditTrailProvider : IAuditTrailProvider
    {
        private readonly AuditConfiguration auditConfigurationSqlServer;
        private readonly AuditContext auditContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuditTrailProvider(AuditConfiguration auditConfigurationSqlServer,
            AuditContext auditContext,
            IHttpContextAccessor httpContextAccessor)
        {
            this.auditConfigurationSqlServer = auditConfigurationSqlServer ?? throw new ArgumentNullException(nameof(auditConfigurationSqlServer));
            this.auditContext = auditContext ?? throw new ArgumentNullException(nameof(auditContext));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task AddTrailsAsync<T>(AuditOperation auditOperation, string user, AuditableObjects<T> auditableObjects) where T : class =>
            await AddTrailsAsync(auditOperation, user, auditableObjects, null);

        public async Task AddTrailsAsync<T>(AuditOperation auditOperation, string user, AuditableObjects<T> auditableObjects, IEnumerable<string> namesOfPropertiesToIgnore) where T : class
        {
            if (!auditConfigurationSqlServer.Enable.Value)
                return;

            var trails = auditableObjects.Select(o => new AuditTrail(
                auditableObjects.TransactionId,
                auditableObjects.Timestamp,
                auditConfigurationSqlServer.ApplicationId,
                auditOperation,
                user,
                httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                o.Key,
                o.Value,
                namesOfPropertiesToIgnore));

            var transaction = string.Empty;
            foreach (var trail in trails)
                transaction += $@"
                    insert into
                        [{auditConfigurationSqlServer.SqlServer.DatabaseName}].
                        [{auditConfigurationSqlServer.SqlServer.SchemaName}].
                        [{auditConfigurationSqlServer.SqlServer.TableName}]
                        (
                            [{nameof(AuditTrail.Id)}], 
                            [{nameof(AuditTrail.TransactionId)}], 
                            [{nameof(AuditTrail.Timestamp)}], 
                            [{nameof(AuditTrail.ApplicationId)}], 
                            [{nameof(AuditTrail.Operation)}], 
                            [{nameof(AuditTrail.User)}],
                            [{nameof(AuditTrail.Origin)}],
                            [{nameof(AuditTrail.ObjectAssembly)}],
                            [{nameof(AuditTrail.ObjectId)}],
                            [{nameof(AuditTrail.ObjectSerial)}]
                        )
                    values
                        (
                            '{trail.Id}',
                            '{trail.TransactionId}',
                            '{trail.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff") }',
                            '{trail.ApplicationId}',
                            '{trail.Operation}',
                            '{trail.User}',
                            {(!string.IsNullOrWhiteSpace(trail.Origin) ? $"'{trail.Origin}'" : "NULL")},
                            '{trail.ObjectAssembly}',
                            '{trail.ObjectId}',
                            {(!string.IsNullOrWhiteSpace(trail.ObjectSerial) ? $"'{trail.ObjectSerial}'" : "NULL")}
                        );";

            await auditContext.ExecuteWithTransactionAsync(transaction);
        }

        public async Task<IEnumerable<AuditTrail>> ViewHistoryAsync<T>(string objectId) where T : class
        {
            if (!auditConfigurationSqlServer.Enable.Value)
                return default(IEnumerable<AuditTrail>);

            await Task.Delay(1);
            throw new NotImplementedException();
        }
    }
}