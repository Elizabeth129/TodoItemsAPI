using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.DAL
{
    public interface IEntityRepository<T> where T : class, new()
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAllTodoItems();
        T GetTodoItemById(long id);
    }
}
