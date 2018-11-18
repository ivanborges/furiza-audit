using System;
using System.Collections.Generic;

namespace Furiza.Audit.Abstractions
{
    public class AuditableObjects<T> : Dictionary<string, T>
        where T : class
    {
        public Guid TransactionId { get; }
        public DateTime Timestamp { get; }

        public AuditableObjects()
        {
            TransactionId = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
        }

        public AuditableObjects(string objectId, T @object) : this()
        {
            Add(objectId, @object);
        }

        public AuditableObjects<T> AddObject(string objectId, T @object)
        {
            Add(objectId, @object);
            return this;
        }
    }
}