using Furiza.Audit.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Furiza.Audit.SqlServer.Dapper
{
    internal class AuditSqlServerDapperInitializer
    {
        private readonly AuditConfiguration auditConfigurationSqlServer;
        private readonly AuditContext auditContext;
        private readonly ILogger logger;

        public AuditSqlServerDapperInitializer(AuditConfiguration auditConfigurationSqlServer,
            AuditContext auditContext,
            ILoggerFactory loggerFactory)
        {
            this.auditConfigurationSqlServer = auditConfigurationSqlServer ?? throw new ArgumentNullException(nameof(auditConfigurationSqlServer));
            this.auditContext = auditContext ?? throw new ArgumentNullException(nameof(auditContext));
            logger = loggerFactory?.CreateLogger<AuditSqlServerDapperInitializer>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public void Initialize()
        {
            if (!auditConfigurationSqlServer.Enable.Value)
            {
                logger.LogInformation("Audit with SqlServer and Dapper disabled.");
                return;
            }

            try
            {
                logger.LogInformation($"Performing query in 'INFORMATION_SCHEMA' in order to check if audit table '{auditConfigurationSqlServer.SqlServer.TableName}' already exists...");

                var dmlTableExists = $@"
                    SELECT 
                        [TABLE_NAME]
                    FROM
                        [{auditConfigurationSqlServer.SqlServer.DatabaseName}].
                        [INFORMATION_SCHEMA].
                        [TABLES]
                    WHERE
                        TABLE_NAME = @tableName";

                var tableExists = auditContext
                    .QueryAsync<string>(dmlTableExists, new { tableName = auditConfigurationSqlServer.SqlServer.TableName })
                    .GetAwaiter()
                    .GetResult()
                    .Any();

                logger.LogInformation($"Audit table '{auditConfigurationSqlServer.SqlServer.TableName}' exists: {tableExists}");

                if (tableExists)
                    return;
                else if (!auditConfigurationSqlServer.EnableInitializer)
                    throw new NotImplementedException($"Audit table '{auditConfigurationSqlServer.SqlServer.TableName}' does not exist in the supplied database '{auditConfigurationSqlServer.SqlServer.DatabaseName}'.");

                logger.LogInformation($"Attempting to create audit table '{auditConfigurationSqlServer.SqlServer.TableName}'...");

                var ddlCreateTable = $@"
                    CREATE TABLE 
                        [{auditConfigurationSqlServer.SqlServer.DatabaseName}].
                        [{auditConfigurationSqlServer.SqlServer.SchemaName}].
                        [{auditConfigurationSqlServer.SqlServer.TableName}] 
                    (
                        [{nameof(AuditTrail.Id)}] uniqueidentifier NOT NULL, 
                        [{nameof(AuditTrail.TransactionId)}] uniqueidentifier NOT NULL, 
                        [{nameof(AuditTrail.Timestamp)}] datetime NOT NULL, 
                        [{nameof(AuditTrail.ApplicationId)}] varchar(100) NOT NULL, 
                        [{nameof(AuditTrail.Operation)}] varchar(50) NOT NULL, 
                        [{nameof(AuditTrail.User)}] varchar(50) NOT NULL,
                        [{nameof(AuditTrail.Origin)}] varchar(500) NULL,
                        [{nameof(AuditTrail.ObjectAssembly)}] varchar(500) NOT NULL,
                        [{nameof(AuditTrail.ObjectId)}] varchar(200) NOT NULL,
                        [{nameof(AuditTrail.ObjectSerial)}] text NULL,
	                    CONSTRAINT [PK_{auditConfigurationSqlServer.SqlServer.TableName}] PRIMARY KEY ([{nameof(AuditTrail.Id)}])
                    );
                    CREATE INDEX 
                        [IX_{auditConfigurationSqlServer.SqlServer.TableName}_ViewHistory] 
                    ON 
                        [{auditConfigurationSqlServer.SqlServer.SchemaName}].
                        [{auditConfigurationSqlServer.SqlServer.TableName}]
                        ([{nameof(AuditTrail.ApplicationId)}], [{nameof(AuditTrail.ObjectAssembly)}], [{nameof(AuditTrail.ObjectId)}]);
                    CREATE INDEX 
                        [IX_{auditConfigurationSqlServer.SqlServer.TableName}_{nameof(AuditTrail.User)}] 
                    ON 
                        [{auditConfigurationSqlServer.SqlServer.SchemaName}].
                        [{auditConfigurationSqlServer.SqlServer.TableName}] 
                        ([{nameof(AuditTrail.User)}]);";

                auditContext.ExecuteWithTransactionAsync(ddlCreateTable).GetAwaiter();

                logger.LogInformation($"Audit table '{auditConfigurationSqlServer.SqlServer.TableName}' created.");
            }
            catch (Exception e)
            {
                logger.LogError(e, "An internal error occurred while trying to initialize the audit schema.");
            }
        }
    }
}