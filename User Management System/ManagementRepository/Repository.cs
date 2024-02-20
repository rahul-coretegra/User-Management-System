using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using User_Management_System.ManagementConfigurations;
using User_Management_System.ManagementRepository.IManagementRepository;

namespace User_Management_System.ManagementRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public async Task<T> GetAsync(string code)
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

        public async Task RemoveAsync(string code)
        {
            var entity = await GetAsync(code);
            if (entity != null)
                await Remove(entity);
        }

        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
