using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlRepository.IPsqlRepository;

namespace User_Management_System.PostgreSqlRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PostgreSqlApplicationDbContext _context;

        internal DbSet<T> dbSet;

        public Repository(PostgreSqlApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public async Task<T> GetAsync(string code)
        {
            try
            {
                return await dbSet.FindAsync(code);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<T> GetByIntAsync(int code)
        {
            try
            {
                return await dbSet.FindAsync(code);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }


        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }


        }

        public async Task UpdateAsync(string entityCode, Func<T, Task> updateAction)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        public async Task UpdateByIntAsync(int entityCode, Func<T, Task> updateAction)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        public async Task RemoveAsync(T entity)
        {
            try
            {
                dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        public async Task RemoveAsync(string code)
        {
            try
            {
                var entity = await GetAsync(code);
                if (entity != null)
                {
                    await RemoveAsync(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        public async Task RemoveByIntAsync(int code)
        {
            try
            {
                var entity = await GetByIntAsync(code);
                if (entity != null)
                {
                    await RemoveAsync(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        public async Task RemoveRangeAsync(IEnumerable<T> values)
        {
            try
            {
                dbSet.RemoveRange(values);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

    }
}
