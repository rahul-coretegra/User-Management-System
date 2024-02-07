using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository;

namespace User_Management_System.MicrosoftSqlServerRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MicrosoftSqlServerApplicationDbContext _context;

        internal DbSet<T> dbSet;

        public Repository(MicrosoftSqlServerApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public async Task<T> GetAsync(string code)
        {
            return await dbSet.FindAsync(code);
        }

        public async Task<T> GetByIntAsync(int code)
        {
            return await dbSet.FindAsync(code);
        }

        public async Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null) query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (orderBy != null) query = orderBy(query);

            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateAsync(string entityCode, Func<T, Task> updateAction)
        {
            _context.ChangeTracker.Clear();
            var entity = await GetAsync(entityCode);

            if (entity != null)
            {
                await updateAction(entity);
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
        }

        public async Task UpdateByIntAsync(int entityCode, Func<T, Task> updateAction)
        {
            _context.ChangeTracker.Clear();
            var entity = await GetByIntAsync(entityCode);

            if (entity != null)
            {
                await updateAction(entity);
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(string code)
        {
            var entity = await GetAsync(code);
            if (entity != null)
            {
                await RemoveAsync(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task RemoveByIntAsync(int code)
        {
            var entity = await GetByIntAsync(code);
            if (entity != null)
            {
                await RemoveAsync(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task RemoveRangeAsync(IEnumerable<T> values)
        {
            dbSet.RemoveRange(values);
            await _context.SaveChangesAsync();
        }

    }

}
