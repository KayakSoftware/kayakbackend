using System;

namespace KayakBackend.Persistence.Entities
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}