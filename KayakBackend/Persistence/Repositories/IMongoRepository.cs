using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KayakBackend.Persistence.Entities;

namespace KayakBackend.Persistence.Repositories
{
    public interface IMongoRepository<TEntity> where TEntity : IEntity
    {
        Task<bool> Delete(Guid id);
        Task<IEnumerable<TEntity>> GetAll(int skip = 0, int limit = 100);
        Task<IEnumerable<TEntity>> GetPaged(int page, int pageSize);
        Task<TEntity> GetById(Guid id);
        Task<Guid> Insert(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
    }
}