using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KayakBackend.Configurations;
using KayakBackend.Persistence.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace KayakBackend.Persistence.Repositories
{
    public class BaseMongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : IEntity
    {
        // Client for establishing a connection
        private readonly IMongoClient _mongoClient;
        // Database configurations
        private readonly PersistenceConfiguration _config;

        // Connection ready database
        protected readonly IMongoDatabase Database;
        protected readonly string CollectionName = $"{typeof(TEntity).Name.ToLower()}";
        
        // Shorthand methods for update, sort, filter, projections and indexKeys
        protected UpdateDefinitionBuilder<TEntity> Update => Builders<TEntity>.Update;
        protected SortDefinitionBuilder<TEntity> Sort => Builders<TEntity>.Sort;
        protected FilterDefinitionBuilder<TEntity> Filter => Builders<TEntity>.Filter;
        protected ProjectionDefinitionBuilder<TEntity> Projection => Builders<TEntity>.Projection;
        protected IndexKeysDefinitionBuilder<TEntity> IndexKeys => Builders<TEntity>.IndexKeys;
        
        protected FilterDefinitionBuilder<TCustom> CustomFilter<TCustom>()
        {
            return Builders<TCustom>.Filter;
        }

        public BaseMongoRepository(IMongoClient mongoClient, IOptions<PersistenceConfiguration> config)
        {
            _mongoClient = mongoClient;
            _config = config.Value;
            
            // Connect to database --> Find database by name or create if none exists
            Database = _mongoClient.GetDatabase(_config.DefaultDatabaseName);
            
            // Search for collection of type TEntity, if none exists create an empty collection
            if (!CollectionExists(Database, CollectionName))
            {
                Database.CreateCollection(CollectionName);
            }
            
            ApplyConventions();
        }

        private void ApplyConventions()
        {
            // Apply enum string convention pack
            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String)
            };
            ConventionRegistry.Register("EnumStringConvention", pack, t=> true);
        }

        private bool CollectionExists(IMongoDatabase db, string collectionName)
        {
            return db.ListCollections(
                new ListCollectionsOptions {Filter = new BsonDocument("name", collectionName)}).Any();
        }

        protected IMongoCollection<TEntity> Collection()
        {
            return Database.GetCollection<TEntity>(CollectionName);
        }
        
        public virtual async Task<bool> Delete(Guid id)
        {
            return (await Collection().DeleteOneAsync(entity => entity.Id == id)).IsAcknowledged;
        }

        public async Task<IEnumerable<TEntity>> GetAll(int skip = 0, int limit = 100)
        {
            return await Collection()
                .Find(Filter.Empty)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetPaged(int page, int pageSize)
        {
            return await GetAll(page * pageSize, pageSize);
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await Collection().Find(entity => entity.Id == id).SingleOrDefaultAsync();
        }

        public virtual async Task<Guid> Insert(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection().InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);

            return entity.Id;
        }
    }
}