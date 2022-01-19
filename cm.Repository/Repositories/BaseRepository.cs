using cm.Domain.Interfaces;
using cm.Infrastructure.EF;
using cm.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cm.Repository.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly ILogger<IBaseRepository> Logger;
        protected readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseRepository(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext, ILogger<IBaseRepository> logger)
        {
            Logger = logger;
            _context = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AuditEntity(IEnumerable<EntityEntry<IAuditedEntity>> modifiedEntries)
        {
            var now = DateTime.Now;
            var canBoId = _httpContextAccessor.HttpContext.GetCurrentCanBoId();
            foreach (var entry in modifiedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy = canBoId;
                        AuditUpdatingField(entry);

                        Logger.LogInformation("Added entity {@Entity} by user {CanBoId}", entry.Entity, canBoId);
                        break;

                    case EntityState.Modified:
                        AuditUpdatingField(entry);

                        Logger.LogInformation("Updated entity to {@Entity} by user {CanBoId}", entry.Entity, canBoId);
                        break;

                    case EntityState.Deleted:
                        Logger.LogInformation("Deleted entity {@Entity} by user {CanBoId}", entry.Entity, canBoId);
                        break;
                }
            }

            void AuditUpdatingField(EntityEntry<IAuditedEntity> entry)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = canBoId;
            }
        }

        public virtual Task SaveChangeAsync()
        {
            var currentDetectChangesSetting = _context.ChangeTracker.AutoDetectChangesEnabled;

            try
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
                _context.ChangeTracker.DetectChanges();

                var modifiedEntries = _context.ChangeTracker.Entries<IAuditedEntity>()
                    .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted);

                AuditEntity(modifiedEntries);

                _context.ChangeTracker.DetectChanges();

                return _context.SaveChangesAsync();
            }
            finally
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = currentDetectChangesSetting;
            }
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

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(ISoftDelete)))
            {
                ((ISoftDelete)entity).IsDeleted = true;
                Update<TEntity>(entity);
                await SaveChangeAsync();
            }
            else
            {
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();
            }
        }

        public async Task DeleteManyAsync<TEntity>(List<TEntity> entities) where TEntity : class
        {
            foreach (var item in entities)
            {
                await DeleteAsync<TEntity>(item);
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

    public static class HttpContextHeper
    {
        public static Guid GetCurrentUserId(this HttpContext httpContext)
        {
            var currentUserId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(currentUserId, out Guid userId);
            return userId;
        }

        public static Guid GetCurrentCanBoId(this HttpContext httpContext)
        {
            var currentCanBoId = httpContext?.User?.FindFirst(ClaimTypes.Hash)?.Value;
            Guid.TryParse(currentCanBoId, out Guid canBoId);
            return canBoId;
        }
    }
}