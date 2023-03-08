using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp_DomainEntities;

namespace TodoApp_Tests.RepositoriesTests.Models
{
    public class TestDataSeed
    {
        private readonly static TestDataSeed dataRepository = new TestDataSeed();

        protected TestDataSeed() 
        {           
        }

        public virtual void SeedData()
        {
            using (var context = new TestDataDbContext())
            {
                TestUser user = new TestUser
                {
                    Name = "Bob",
                    Email = "Uncle@Bob.com",
                    Password = "Cj321$"
                };

                TodoList[] lists = GetLists(user);

                AddListsToContext(context, lists);

                context.SaveChanges();

                Todo[] todos = GetTodos(lists);

                AddTodosToLists(lists, todos);

                context.Todos.AddRange(todos);

                context.Users.Add(user);
                context.SaveChanges();

                Reminder reminder = new Reminder
                {
                    RemindTime = DateTime.Now.AddDays(3),
                    TodoId = todos[0].Id,
                };

                context.Reminders.Add(reminder);
                context.SaveChanges();
            }
        }

        public static TestDataSeed GetInstanse()
        {
            return dataRepository;
        }

        private static void AddListsToContext(TestDataDbContext context, TodoList[] lists)
        {
            context.TodoLists.Add(lists[0]);
            context.TodoLists.Add(lists[1]);
            context.TodoLists.Add(lists[2]);
        }

        private static Todo[] GetTodos(TodoList[] lists)
        {
            return new Todo[]
            {
                    new Todo
                    {
                        Title = "Buy tea",
                        Description = "Go to the shop and buy some tea.",
                        DueDate = new DateTime(2023, 3, 23),
                        CreationDate = new DateTime(2023, 3, 13),
                        Status = TodoStatus.NotStarted,
                        ListId = lists[0].Id,
                    },

                    new Todo
                    {
                        Title = "Refuel the car",
                        Description = "Go to the gas station and refuel the car.",
                        DueDate = new DateTime(2023, 4, 5),
                        CreationDate = new DateTime(2023, 3, 13),
                        Status = TodoStatus.NotStarted,
                        ListId = lists[0].Id,
                    },

                    new Todo
                    {
                        Title = "Feed the cat",
                        Description = "Open the fridge, take a box of cat food and feed it.",
                        DueDate = new DateTime(2023, 3, 13, 14, 0, 0),
                        CreationDate = new DateTime(2023, 3, 13, 12, 0, 0),
                        Status = TodoStatus.InProgress,
                        ListId = lists[0].Id,
                    },

                    new Todo
                    {
                        Title = "Find a dungeon for raid",
                        Description = "Go to the map and search for the dungeon.",
                        DueDate = new DateTime(2023, 4, 24),
                        CreationDate = new DateTime(2023, 4, 22),
                        Status = TodoStatus.Completed,
                        ListId = lists[1].Id,
                    },

                    new Todo
                    {
                        Title = "Gather your party",
                        Description = "Call friends for the raid by phone or face to face.",
                        DueDate = new DateTime(2023, 4, 29),
                        CreationDate = new DateTime(2023, 4, 26),
                        Status = TodoStatus.InProgress,
                        ListId = lists[1].Id,
                    },

                    new Todo
                    {
                        Title = "Clear the dungeon with friends",
                        Description = "Go together with friends to the dungeon, clear it and get awesome rewards.",
                        DueDate = new DateTime(2023, 5, 3),
                        CreationDate = new DateTime(2023, 4, 28),
                        Status = TodoStatus.NotStarted,
                        ListId = lists[1].Id,
                    },

                    new Todo
                    {
                        Title = "Jack's Wedding",
                        Description = "I'm waiting for you at my wedding! Together we will make this day unforgettable!",
                        DueDate = new DateTime(1998, 7, 1),
                        CreationDate = new DateTime(1998, 5, 26),
                        Status = TodoStatus.NotStarted,
                        ListId = lists[2].Id,
                    },

                    new Todo
                    {
                        Title = "Marie's Birthday",
                        Description = "We mustn't forget to celebrate Marie's birthday...",
                        DueDate = new DateTime(2007, 6, 8),
                        CreationDate = new DateTime(2007, 6, 6),
                        Status = TodoStatus.InProgress,
                        ListId = lists[2].Id,
                    },

                    new Todo
                    {
                        Title = "Snowboards Party",
                        Description = "Show. Boards. Fun.",
                        DueDate = new DateTime(2023, 11, 3),
                        CreationDate = new DateTime(2023, 4, 17),
                        Status = TodoStatus.NotStarted,
                        ListId = lists[2].Id,
                    },
            };
        }

        private static void AddTodosToLists(TodoList[] lists, Todo[] todos)
        {
            lists[0].Todos.Add(todos[0]);
            lists[0].Todos.Add(todos[1]);
            lists[0].Todos.Add(todos[2]);

            lists[1].Todos.Add(todos[3]);
            lists[1].Todos.Add(todos[4]);
            lists[1].Todos.Add(todos[5]);

            lists[2].Todos.Add(todos[6]);
            lists[2].Todos.Add(todos[7]);
            lists[2].Todos.Add(todos[8]);
        }

        private static TodoList[] GetLists(TestUser user)
        {
            return new TodoList[]
            {
                new TodoList
                {
                    Title = "Housework",
                    UserId = user.Id.ToString(),
                },

                new TodoList
                {
                    Title = "Computer games",
                    UserId = user.Id.ToString(),
                },

                new TodoList
                {
                    Title = "Important meetings",
                    UserId = user.Id.ToString(),
                }
            };
        }
    }
}
