using System.Collections.Generic;
using System.Threading.Tasks;

namespace Furiza.Audit.Abstractions
{
    public interface IAuditTrailProvider
    {
        Task AddTrailsAsync<T>(AuditOperation auditOperation, string user, string origin, AuditableObjects<T> auditableObjects)
            where T : class;

        Task<IEnumerable<AuditTrail>> ViewHistoryAsync<T>(string objectId)
            where T : class;
    }
}