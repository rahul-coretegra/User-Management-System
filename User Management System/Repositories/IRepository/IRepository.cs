using System.Linq.Expressions;

namespace User_Management_System.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string code);

        Task<T> GetByIntAsync(int code);

        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null);

        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null);

        Task AddAsync(T entity);

        Task UpdateAsync(string entityCode, Func<T, Task> updateAction);

        Task UpdateByIntAsync(int entityCode, Func<T, Task> updateAction);

        Task RemoveAsync(T entity);

        Task RemoveAsync(string code);

        Task RemoveByIntAsync(int code);

        Task RemoveRangeAsync(IEnumerable<T> values);

    }
}