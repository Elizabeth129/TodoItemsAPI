using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.BL;
using TodoApi.DAL;
using TodoApi.DataRepository;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Tests
{
    public class TodoTestBL
    {
        private Mock<IEntityRepository<TodoItem>> todoRepository;
        private List<TodoItem> todoItems;
        [SetUp]
        public void Setup()
        {
            //Set up the mock
            todoRepository = new Mock<IEntityRepository<TodoItem>>();
            todoItems = new List<TodoItem>();
            todoItems.Add(new TodoItem() { Id = 1, Name = "todo1", IsComplete = false, Secret = "todo1" });
            todoItems.Add(new TodoItem() { Id = 2, Name = "todo2", IsComplete = false, Secret = "todo2" });
            todoItems.Add(new TodoItem() { Id = 3, Name = "todo3", IsComplete = false, Secret = "todo3" });
        }

        [Test]
        public async Task TestGetAllItems()
        {
            //Act
            todoRepository.Setup(a => a.GetAllTodoItems()).Returns(todoItems.AsQueryable());
            
            //Arrange
            var todoBL = new TodoBL(todoRepository.Object);
            var todoList = await todoBL.GetTodoItems();
       
            //Assert
            Assert.AreEqual(todoList.ToList().Count, 3);
        }

        [Test]
        public async Task TestGetItemById()
        {
            //Act
            todoRepository.Setup(a => a.GetTodoItemById(It.IsAny<long>())).Returns(todoItems.ElementAt(1));

            //Arrange
            var todoBL = new TodoBL(todoRepository.Object);
            var todoItem = await todoBL.GetTodoItemById(2);

            //Assert
            Assert.AreEqual(todoItem.Id, 2);
            Assert.AreEqual(todoItem.Name, "todo2");
            Assert.AreEqual(todoItem.IsComplete, false);
        }

        [Test]
        public async Task UpdateTodoItem()
        {
            //Act
            todoRepository.Setup(a => a.GetTodoItemById(It.IsAny<long>())).Returns(todoItems.ElementAt(2));
            todoRepository.Setup(a => a.Update(It.IsAny<TodoItem>()));
            //Arrange
            var todoBL = new TodoBL(todoRepository.Object);
            TodoItemDTO item = new TodoItemDTO { Id = 3, Name = "todo31", IsComplete = true };
            await todoBL.Update(item);

            //Assert
            todoRepository.Verify(x => x.Update(It.Is<TodoItem>(b => b.Name == item.Name && b.Id == item.Id && b.IsComplete == item.IsComplete)), Times.Once);
        }

        [Test]
        public async Task AddTodoItem()
        {
            //Act
            
            todoRepository.Setup(a => a.Insert(It.IsAny<TodoItem>()));
            //Arrange
            var todoBL = new TodoBL(todoRepository.Object);
            TodoItemDTO item = new TodoItemDTO { Name = "todo4", IsComplete = true };
            await todoBL.Create(item);
            
            //Assert
            todoRepository.Verify(x => x.Insert(It.Is<TodoItem>(b => b.Name == item.Name && b.IsComplete == item.IsComplete)), Times.Once);
        }

        [Test]
        public async Task DeleteTodoItem()
        {
            //Act
            todoRepository.Setup(a => a.GetAllTodoItems()).Returns(todoItems.AsQueryable());
            todoRepository.Setup(a => a.Delete(It.IsAny<TodoItem>()));
            //Arrange
            var todoBL = new TodoBL(todoRepository.Object);
            await todoBL.Delete(1);

            //Assert
            todoRepository.Verify(x => x.Delete(It.Is<TodoItem>(b => b.Name == "todo1" && b.IsComplete == false && b.Id == 1)), Times.Once);
        }
    }
}
