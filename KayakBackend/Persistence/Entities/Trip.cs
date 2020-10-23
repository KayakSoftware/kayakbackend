using System;

namespace KayakBackend.Persistence.Entities
{
    public class Trip : IEntity
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}