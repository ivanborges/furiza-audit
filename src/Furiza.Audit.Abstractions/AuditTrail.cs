using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Furiza.Audit.Abstractions
{
    public class AuditTrail
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime Timestamp { get; set; }
        public string ApplicationId { get; set; }
        public string Operation { get; set; }
        public string User { get; set; }
        public string Origin { get; set; }
        public string ObjectAssembly { get; set; }
        public string ObjectId { get; set; }
        public string ObjectSerial { get; set; }

        public AuditTrail()
        {
        }

        public AuditTrail(Guid transactionId, DateTime timestamp, string applicationId, AuditOperation operation, string user, string origin, string objectId, object @object, IEnumerable<string> namesOfPropertiesToIgnore = null)
        {
            Id = Guid.NewGuid();
            TransactionId = transactionId;
            Timestamp = timestamp;
            ApplicationId = !string.IsNullOrWhiteSpace(applicationId)
                ? applicationId
                : throw new ArgumentNullException(nameof(applicationId));
            Operation = operation.ToString();
            User = !string.IsNullOrWhiteSpace(user)
                ? user
                : throw new ArgumentNullException(nameof(user));
            Origin = origin;
            ObjectAssembly = @object?.GetType().FullName 
                ?? throw new ArgumentNullException(nameof(@object));
            ObjectId = !string.IsNullOrWhiteSpace(objectId)
                ? objectId
                : throw new ArgumentNullException(nameof(objectId));
            ObjectSerial = operation == AuditOperation.Create || operation == AuditOperation.Update
                ? JsonConvert.SerializeObject(@object, @object.GetType(), new GeneralJsonSerializerSettings().IgnoreProperties(@object.GetType(), namesOfPropertiesToIgnore))
                : string.Empty;
        }
    }
}