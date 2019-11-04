using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NNNDataModel;
using NNNDataContext.Context;
using Microsoft.EntityFrameworkCore;

namespace NNNDataContext.Repositories
{
    public interface IBaseRepository<TModel> where TModel : BaseModel
    {
        IQueryable<TModel> GetAll();
        Task<TModel> GetById(int id);
        Task<TModel> Create(TModel entity);
        Task CreateMany(IEnumerable<TModel> entities);
        Task Update(int id, TModel entity);
        Task Delete(int id);
        Task ExecuteSql(string query);
    }

    public class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : BaseModel
    {
        private readonly NewsContext _dbContext;
        protected NewsContext DbContext => _dbContext;

        public BaseRepository(NewsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TModel> GetAll()
        {
            return _dbContext.Set<TModel>().AsNoTracking();
        }

        public async Task<TModel> GetById(int id)
        {
            return await _dbContext.Set<TModel>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<TModel> Create(TModel entity)
        {
            await _dbContext.Set<TModel>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task CreateMany(IEnumerable<TModel> entities)
        {
            await _dbContext.Set<TModel>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, TModel entity)
        {
            var local = _dbContext.Set<TModel>().Local.FirstOrDefault(entry => entry.ID.Equals(id));
            if (local != null)
            {
                _dbContext.Entry(local).State = EntityState.Detached;
            }
            _dbContext.Set<TModel>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            _dbContext.Set<TModel>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ExecuteSql(string query)
        {
            await _dbContext.Database.ExecuteSqlCommandAsync(query);
        }
    }
}
