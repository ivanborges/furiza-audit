using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Furiza.Audit.Abstractions
{
    public abstract class AuditConfiguration
    {
        [Required]
        public AuditTool? Tool { get; set; }

        [Required]
        public bool? Enable { get; set; }

        public string ApplicationId { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;

        public AuditConfiguration AddApplicationId(string applicationId)
        {
            ApplicationId = applicationId;
            return this;
        }
    }
}