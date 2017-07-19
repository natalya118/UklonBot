using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UklonBot.Models.Repositories.Exact
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public IEnumerable<TEntity> FindAll(Func<TEntity, bool> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public bool Any(Func<TEntity, bool> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public void Create(TEntity item)
        {
            _dbSet.Add(item);
        }

        public void Update(TEntity itemToUpdate)
        {
            if (itemToUpdate != null)
            {
                _dbSet.Attach(itemToUpdate);
                _context.Entry(itemToUpdate).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            TEntity entity = _dbSet.Find(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public TEntity FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }
    }
}