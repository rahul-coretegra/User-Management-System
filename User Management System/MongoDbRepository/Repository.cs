using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SharpCompress.Common;
using System.Linq.Expressions;
using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbRepository.IMongoRepository;

namespace User_Management_System.MongoDbRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MongoDbApplicationDbContext _context;
        internal IMongoCollection<T> _collection;

        public Repository(MongoDbApplicationDbContext dbContext)
        {
            _context = dbContext;
            _collection = dbContext.Database.GetCollection<T>(typeof(T).Name);
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null)
        {
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
                return await _collection.Find(_ => true).ToListAsync();
            else
                return await _collection.Find(filter).ToListAsync();
        }

        public async Task RemoveAsync(Expression<Func<T, bool>> filter = null)
        {
            await _collection.DeleteOneAsync(filter);
        }

        public async Task UpdateAsync(Expression<Func<T, bool>> filter, Func<T, Task> updateAction)
        {
            var entityToUpdate = await _collection.Find(filter).FirstOrDefaultAsync();
            if (entityToUpdate != null)
            {
                await updateAction(entityToUpdate);
                await _collection.ReplaceOneAsync(filter, entityToUpdate);
            }
        }
    }
}
