using Furiza.Audit.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Furiza.Audit.SqlServer.Dapper
{
    public class AuditConfigurationSqlServer : AuditConfiguration
    {
        [Required]
        public AuditConfigurationSqlServerDapper SqlServer { get; set; }
    }
}