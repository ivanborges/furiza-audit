using System.ComponentModel.DataAnnotations;

namespace Furiza.Audit.SqlServer.Dapper
{
    public class AuditConfiguration : Abstractions.AuditConfiguration
    {
        [Required]
        public AuditConfigurationSqlServerDapper SqlServer { get; set; }
    }
}