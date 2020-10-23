using KayakBackend.Configurations;
using KayakBackend.Persistence.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace KayakBackend.Persistence.Repositories
{
    public interface ITripRepository : IMongoRepository<Trip>
    {
    }
    
    public class TripRepository : BaseMongoRepository<Trip>, ITripRepository
    {
        public TripRepository(IMongoClient mongoClient, IOptions<PersistenceConfiguration> config) : base(mongoClient, config)
        {
        }
    }
}