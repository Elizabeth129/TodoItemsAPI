using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.BL;
using TodoApi.DataRepository;

namespace TodoApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private TodoBL _todoBL;

        public TodoItemsController(TodoContext context)
        {
            _todoBL = new TodoBL(context);
        }
     
        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            /*return await _context.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();*/
            IEnumerable<TodoItemDTO> todoItems;
            try
            {
                todoItems = await _todoBL.GetTodoItems();
            }
            catch
            {
                return BadRequest();
            }
            return Ok(todoItems);
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _todoBL.GetTodoItemById(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
  
        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
       // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            await _todoBL.Update(id, todoItemDTO);

            return NoContent();
        }

          // POST: api/TodoItems
          /// <summary>
          /// Creates a TodoItem.
          /// </summary>
          /// <remarks>
          /// Sample request:
          ///
          ///     POST /Todo
          ///     {
          ///        "id": 1,
          ///        "name": "Item1",
          ///        "isComplete": true
          ///     }
          ///
          /// </remarks>
          /// <param name="item"></param>
          /// <returns>A newly created TodoItem</returns>
          /// <response code="201">Returns the newly created item</response>
          /// <response code="400">If the item is null</response>            
          [HttpPost]
          [ProducesResponseType(StatusCodes.Status201Created)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
          {
             var todoItem =  await _todoBL.Create(todoItemDTO);

              return CreatedAtAction(
                  nameof(GetTodoItem),
                  new { id = todoItem.Id },
                  ItemToDTO(todoItem));
          }


          // DELETE: api/TodoItems/5
          /// <summary>
          /// Deletes a specific TodoItem.
          /// </summary>
          /// <param name="id"></param>  
          [HttpDelete("{id}")]
          public async Task<IActionResult> DeleteTodoItem(long id)
          {
              await _todoBL.Delete(id);

              return NoContent();
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
