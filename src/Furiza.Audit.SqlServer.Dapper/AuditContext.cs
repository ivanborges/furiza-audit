using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Furiza.Audit.SqlServer.Dapper
{
    internal class AuditContext
    {
        private readonly AuditConfiguration auditConfigurationSqlServer;
        private readonly IDbConnection connection;

        public AuditContext(AuditConfiguration auditConfigurationSqlServer)
        {
            this.auditConfigurationSqlServer = auditConfigurationSqlServer ?? throw new ArgumentNullException(nameof(auditConfigurationSqlServer));
            connection = new SqlConnection(this.auditConfigurationSqlServer.SqlServer.ConnectionString);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null) => await connection.QueryAsync<T>(sql, param);

        public async Task<int> ExecuteAsync(string sql, object param = null) => await connection.ExecuteAsync(sql, param);

        public async Task<int> ExecuteWithTransactionAsync(string sql, object param = null)
        {
            var @return = 0;

            using (var ownConnection = new SqlConnection(auditConfigurationSqlServer.SqlServer.ConnectionString))
            {
                ownConnection.Open();
                using (var transaction = ownConnection.BeginTransaction())
                {
                    @return = await ownConnection.ExecuteAsync(sql, param, transaction);
                    transaction.Commit();
                }
            }

            return @return;
        }
    }
}