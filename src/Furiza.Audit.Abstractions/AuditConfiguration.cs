using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Furiza.Audit.Abstractions
{
    public class AuditConfiguration
    {
        [Required]
        public AuditTool? Tool { get; set; }

        [Required]
        public bool? Enable { get; set; }

        public bool EnableInitializer { get; set; } = false;

        public string ApplicationId { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;

        public AuditConfiguration AddApplicationId(string applicationId)
        {
            if (!string.IsNullOrWhiteSpace(applicationId))
                ApplicationId = applicationId.Replace(" ", "_");

            return this;
        }
    }
}