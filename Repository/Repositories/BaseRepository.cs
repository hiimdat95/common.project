using Domain.Interfaces;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly ILogger<IBaseRepository> _logger;
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext dbContext, ILogger<IBaseRepository> logger)
        {
            _logger = logger;
            _context = dbContext;
        }

        public virtual IQueryable<TEntity> GetQueryable<TEntity>()
          where TEntity : class, IEntity<Guid>
        {
            return GetQueryable<TEntity, Guid>();
        }

        public virtual IQueryable<TEntity> GetQueryable<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return _context.Set<TEntity>();
        }

        public virtual TEntity GetById<TEntity>(Guid id) where TEntity : class
        {
            return _context.Set<TEntity>().Find(id);
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : class
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public IEnumerable<TEntity> ListAll<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>().AsEnumerable();
        }

        public async Task<List<TEntity>> ListAllAsync<TEntity>() where TEntity : class
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public async Task<List<TEntity>> AddRangeAsync<TEntity>(List<TEntity> entity) where TEntity : class
        {
            await _context.Set<TEntity>().AddRangeAsync(entity);

            return entity;
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateManyAsync<TEntity>(List<TEntity> entities) where TEntity : class
        {
            for (int i = 0; i < entities.Count; i++)
            {
                _context.Entry(entities[i]).State = EntityState.Modified;
            }
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(ISoftDelete)))
            {
                ((ISoftDelete)entity).IsDeleted = true;
                Update<TEntity>(entity);
            }
            else
            {
                _context.Set<TEntity>().Remove(entity);
            }
        }

        public void DeleteMany<TEntity>(List<TEntity> entities) where TEntity : class
        {
            foreach (var item in entities)
            {
                Delete<TEntity>(item);
            }
        }

        public List<TEntity> Where<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return _context.Set<TEntity>()
                            .Where(expression)
                            .ToList();
        }

        public async Task<List<TEntity>> WhereAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return await _context.Set<TEntity>()
                            .Where(expression)
                            .ToListAsync();
        }

        public IQueryable<TEntity> AsQueryable<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            if (expression != null)
            {
                return _context.Set<TEntity>()
                                .Where(expression);
            }
            else
            {
                return _context.Set<TEntity>().AsQueryable();
            }
        }

        public async Task<TEntity> FistOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return await _context.Set<TEntity>()
                            .FirstOrDefaultAsync(expression);
        }

        public TEntity FistOrDefault<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return _context.Set<TEntity>()
                            .FirstOrDefault(expression);
        }
    }
}