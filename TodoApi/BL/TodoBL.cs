using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.DAL;
using TodoApi.DataRepository;

namespace TodoApi.BL
{
    public class TodoBL
    {
        private IEntityRepository<TodoItem> _TodoRepository { get; set; }
        public TodoBL(TodoContext context)
        {
            _TodoRepository = new EntityRepository<TodoItem>(context);
        }
        public async Task<IEnumerable<TodoItemDTO>> GetTodoItems()
        {
            return  await Task.FromResult(_TodoRepository.GetAllTodoItems().Select(x => ItemToDTO(x)).ToList());
        }
        public async Task<TodoItemDTO> GetTodoItemById(long id)
        {
            return await Task.FromResult(_TodoRepository.GetAllTodoItems().Select(x => ItemToDTO(x)).ToList().Where(x => x.Id == id).FirstOrDefault());
        }
        public async Task Update(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                throw new ArgumentException("Error Id"); 
            }

            var todoItem = await Task.FromResult(_TodoRepository.GetAllTodoItems().Select(x => ItemToDTO(x)).ToList().Where(x => x.Id == id).FirstOrDefault());
            if (todoItem == null)
            {
                throw new ArgumentException("Error Id"); ;
            }
            _TodoRepository.Update(new TodoItem { Id = id, IsComplete = todoItemDTO.IsComplete, Name = todoItemDTO.Name });

        }
        public async Task<TodoItem> Create(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };
            _TodoRepository.Insert(todoItem);
            return await Task.FromResult(todoItem);
        }
        public async Task Delete(long id)
        {
            var todoItem = await Task.FromResult(_TodoRepository.GetAllTodoItems().ToList().Where(x => x.Id == id).FirstOrDefault());
            if (todoItem == null)
            {
                throw new ArgumentException("Error Id"); 
            }
            _TodoRepository.Delete(todoItem);
        }
        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        new TodoItemDTO
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
    }
}
