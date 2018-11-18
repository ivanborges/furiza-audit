using System.ComponentModel.DataAnnotations;

namespace Furiza.Audit.SqlServer.Dapper
{
    public class AuditConfigurationSqlServerDapper
    {
        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string DatabaseName { get; set; }

        public string SchemaName { get; set; } = "dbo";

        public string TableName { get; set; } = "FurizaAuditTrails";
    }
}