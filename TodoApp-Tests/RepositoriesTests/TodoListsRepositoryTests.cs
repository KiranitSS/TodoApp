using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp_DomainEntities;
using TodoApp_Tests.RepositoriesTests.Models;
using TodoAppWeb.Models.Repository;
using TodoAppWeb.Models;
using TodoAppWeb.Migrations;

namespace TodoApp_Tests.RepositoriesTests
{
#pragma warning disable CS8625

    public class TodoListsRepositoryTests
    {
        private readonly ITodosDbContext context;
        private readonly ITodoListsRepository repository;

        public TodoListsRepositoryTests()
        {
            this.context = new TestDataDbContext();
            this.repository = new EFTodoListsRepository(context);
        }

        [Fact]
        public void CanGetTodoLists()
        {
            var lists = repository.TodoLists.ToArray();

            AddTestData();

            Assert.NotNull(lists);
            Assert.NotEmpty(lists);
        }

        [Fact]
        public void CanSaveTodoList()
        {
            TodoList todoList = new TodoList();

            Assert.DoesNotContain(todoList, GetAllLists());

            repository.CreateList(todoList);

            Assert.Contains(todoList, GetAllLists());
        }

        [Fact]
        public void SaveTodoListThrowNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => repository.CreateList(null));
        }

        [Fact]
        public void CanDeleteTodoList()
        {
            TodoList todoList = new TodoList
            {
                Title = "ListToDelete"
            };

            context.Add(todoList);
            context.SaveChanges();

            Assert.Contains(todoList, GetAllLists());

            repository.DeleteList(todoList);

            Assert.DoesNotContain(todoList, GetAllLists());
        }

        [Fact]
        public void DeleteTodosListThrowNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => repository.DeleteList(null));
        }

        [Fact]
        public void DeleteTodoListThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => repository.DeleteList(new TodoList()));
        }

        [Fact]
        public void CanUpdateTodoList()
        {
            TodoList initialList = new TodoList
            {
                Title = "ListToUpdate"
            };

            context.Add(initialList);
            context.SaveChanges();

            string expected = "NewTitle";
            initialList.Title = expected;
            repository.UpdateList(initialList);

            TodoList updatedList = repository.TodoLists.FirstOrDefault(l => l.Id == initialList.Id)
                ?? throw new InvalidOperationException("Todo is null.");

            string actual = updatedList.Title;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateTodoListThrowNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => repository.UpdateList(null));
        }

        [Fact]
        public void UpdateTodoListThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => repository.UpdateList(new TodoList()));
        }

        private TodoList[] GetAllLists()
        {
            return context.TodoLists.ToArray();
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
            repository.CreateList(todoList);
        }
    }
#pragma warning restore CS8625

}
