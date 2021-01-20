using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.DataRepository;

namespace TodoApi.DAL
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class, new()
    {
        private TodoContext _dbContext;
        private DbSet<T> _dbSet;

        public EntityRepository(TodoContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public virtual IEnumerable<T> GetAllTodoItems()
        {
            return _dbSet;
        }

        public T GetTodoItemById(long id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
