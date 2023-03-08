using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp_DomainEntities;
using TodoApp_Tests.RepositoriesTests.Models;
using TodoAppWeb.Models;
using TodoAppWeb.Models.Repository;

namespace TodoApp_Tests.RepositoriesTests
{
#pragma warning disable CS8625

    public class TodosRepositoryTests
    {
        private readonly ITodosDbContext context;
        private readonly ITodosRepository repository;

        public TodosRepositoryTests()
        {
            this.context = new TestDataDbContext();
            this.repository = new EFTodosRepository(context);
        }

        [Fact]
        public void CanGetTodos()
        {
            var todos = repository.Todos.ToArray();

            AddTestData();

            Assert.NotNull(todos);
            Assert.NotEmpty(todos);
        }

        [Fact]
        public void CanSaveTodo()
        {
            TodoList todoList = GetTestListData();
            context.Add(todoList);
            context.SaveChanges();

            Todo todo = GetTestTodoData(todoList.Id);

            Assert.DoesNotContain(todo, GetAllTodos());

            repository.CreateTodo(todo);

            Assert.Contains(todo, GetAllTodos());
        }

        [Fact]
        public void SaveTodoThrowNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => repository.CreateTodo(null));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void SaveTodoThrowInvalidOperationExceptionWrongIdPassed(int listId)
        {
            Todo todo = new Todo()
            {
                ListId = listId
            };

            Assert.Throws<InvalidOperationException>(() => repository.CreateTodo(todo));
        }

        [Fact]
        public void SaveTodoThrowInvalidOperationExceptionNoIdPassed()
        {
            Assert.Throws<InvalidOperationException>(() => repository.CreateTodo(new Todo()));
        }

        [Fact]
        public void CanDeleteTodo()
        {
            Todo todo = repository.Todos.FirstOrDefault() 
                ?? throw new InvalidOperationException("Todo is null.");

            Assert.Contains(todo, GetAllTodos());

            repository.DeleteTodo(todo);

            Assert.DoesNotContain(todo, GetAllTodos());
        }

        [Fact]
        public void DeleteTodoThrowNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => repository.DeleteTodo(null));
        }

        [Fact]
        public void DeleteTodoThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => repository.DeleteTodo(new Todo()));
        }

        [Fact]
        public void CanUpdateTodo()
        {
            Todo initialTodo = repository.Todos.FirstOrDefault() 
                ?? throw new InvalidOperationException("Todo is null.");

            string expected = "NewTitle";
            initialTodo.Title = expected;
            repository.UpdateTodo(initialTodo);

            Todo updatedTodo = repository.Todos.FirstOrDefault()
                ?? throw new InvalidOperationException("Todo is null.");

            string actual = updatedTodo.Title;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateTodoThrowNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => repository.UpdateTodo(null));
        }

        [Fact]
        public void UpdateTodoThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => repository.UpdateTodo(new Todo()));
        }

        private Todo[] GetAllTodos()
        {
            return context.TodoLists.SelectMany(l => l.Todos).ToArray();
        }

        private Todo GetTestTodoData(long listId)
        {
            return new Todo
            {
                Title = "TestTitle1",
                Description = "TestDescription1",
                DueDate = DateTime.Now.AddDays(3),
                CreationDate = DateTime.Now,
                ListId = listId,
            };
        }

        private TodoList GetTestListData()
        {
            return new TodoList
            {
                Title = "TestTitle1",
            };
        }

        private void AddTestData()
        {
            TodoList todoList = GetTestListData();
            context.Add(todoList);
            context.SaveChanges();

            Todo todo = GetTestTodoData(todoList.Id);

            repository.CreateTodo(todo);
        }
    }
#pragma warning restore CS8625

}
