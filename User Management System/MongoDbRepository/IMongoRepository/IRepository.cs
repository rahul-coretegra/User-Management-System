using System.Linq.Expressions;

namespace User_Management_System.MongoDbRepository.IMongoRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null);

        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null);

        Task AddAsync(T entity);

        Task UpdateAsync(Expression<Func<T, bool>> filter, Func<T, Task> updateAction);

        Task RemoveAsync(Expression<Func<T, bool>> filter = null);

    }
}
