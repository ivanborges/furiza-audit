using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Furiza.Audit.Abstractions
{
    [Serializable]
    public class AuditableObjects<T> : Dictionary<string, T>
        where T : class
    {
        public Guid TransactionId { get; }
        public DateTime Timestamp { get; }

        public AuditableObjects() : base()
        {
            TransactionId = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
        }

        public AuditableObjects(string objectId, T @object) : this()
        {
            Add(objectId, @object);
        }

        protected AuditableObjects(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AuditableObjects<T> AddObject(string objectId, T @object)
        {
            Add(objectId, @object);
            return this;
        }
    }
}