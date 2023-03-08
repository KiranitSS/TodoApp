using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using TodoApp.Controllers;
using TodoApp_DomainEntities;
using TodoAppWeb.Controllers;
using TodoAppWeb.Infrastructure;
using TodoAppWeb.Models.Repository;
using TodoAppWeb.Models.ViewModels;

namespace TodoApp_Tests.ControllersTests
{
    public class TodosControllerTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void CanGetInfo(int todoId)
        {
            Todo[] todos = new Todo[]
            {
                new Todo {
                    Id = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(3).Date,
                    ListId = 1,
                },

                new Todo {
                    Id = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(4).Date,
                    ListId = 1,
                },
            };

            Mock<ITodosRepository> mockTodosRepository = new Mock<ITodosRepository>();
            mockTodosRepository.Setup(m => m.Todos).Returns(todos.AsQueryable());

            Mock<ITodoListsRepository> mockTodoListsRepository = new Mock<ITodoListsRepository>();
            mockTodoListsRepository.Setup(m => m.TodoLists).Returns((new TodoList[]
            {
                new TodoList {
                    Id = 1,
                    Title = "ListTestTitle1",
                },

                new TodoList {
                    Id = 2,
                    Title = "ListTestTitle1",
                },
            }).AsQueryable());

            Mock<IRemindersRepository> mockRemindersRepository = new Mock<IRemindersRepository>();
            mockRemindersRepository.Setup(m => m.Reminders).Returns((new Reminder[]
            {
                new Reminder {
                    Id = 1,
                    RemindTime = DateTime.Now.AddDays(6).Date,
                    UserId = "TestUserId1",
                },

                new Reminder {
                    Id = 2,
                    RemindTime = DateTime.Now.AddDays(7).Date,
                    UserId = "TestUserId1",
                },
            }).AsQueryable());

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Mock<UserManager<IdentityUser>> mockUsersManager = GetMockUserManager();

            mockUsersManager.Setup(m => m.GetUserAsync(claimsPrincipal)).Returns((
                Task.Run(() =>
                {
                    return new IdentityUser
                    {
                        Id = "TestUserId1",
                        UserName = "TestUser",
                        PasswordHash = "Pass123".GetHashCode().ToString()
                    };
                })));

            TodosController controller = new TodosController(
                mockTodosRepository.Object,
                mockTodoListsRepository.Object,
                mockRemindersRepository.Object,
                new ViewModelsFactory(),
                mockUsersManager.Object);

            TodoViewModel? result =
                 (controller.Info(todoId) as ViewResult)?.ViewData.Model
                 as TodoViewModel;

            Assert.NotNull(result);

            const int idxCorrection = 1;

            Assert.Equal(todos[todoId - idxCorrection].Id, result.Id);
            Assert.Equal(todos[todoId - idxCorrection].Title, result.Title);
            Assert.Equal(todos[todoId - idxCorrection].Description, result.Description);
            Assert.Equal(todos[todoId - idxCorrection].DueDate, result.DueDate);
            Assert.Equal(todos[todoId - idxCorrection].CreationDate, result.CreationDate);
            Assert.Equal(todos[todoId - idxCorrection].ListId, result.ListId);
        }

        [Fact]
        public void CanSendListIndexUsingAdd()
        {
            long expectedListId = 1;

            Todo[] todos = new Todo[]
            {
                new Todo {
                    Id = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(3).Date,
                    ListId = 1,
                },

                new Todo {
                    Id = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(4).Date,
                    ListId = 1,
                },
            };

            Mock<ITodosRepository> mockTodosRepository = new Mock<ITodosRepository>();
            mockTodosRepository.Setup(m => m.Todos).Returns(todos.AsQueryable());

            Mock<ITodoListsRepository> mockTodoListsRepository = new Mock<ITodoListsRepository>();
            mockTodoListsRepository.Setup(m => m.TodoLists).Returns((new TodoList[]
            {
                new TodoList {
                    Id = 1,
                    Title = "ListTestTitle1",
                },

                new TodoList {
                    Id = 2,
                    Title = "ListTestTitle1",
                },
            }).AsQueryable());

            Mock<IRemindersRepository> mockRemindersRepository = new Mock<IRemindersRepository>();
            mockRemindersRepository.Setup(m => m.Reminders).Returns((new Reminder[]
            {
                new Reminder {
                    Id = 1,
                    RemindTime = DateTime.Now.AddDays(6).Date,
                    UserId = "TestUserId1",
                },

                new Reminder {
                    Id = 2,
                    RemindTime = DateTime.Now.AddDays(7).Date,
                    UserId = "TestUserId1",
                },
            }).AsQueryable());

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Mock<UserManager<IdentityUser>> mockUsersManager = GetMockUserManager();

            mockUsersManager.Setup(m => m.GetUserAsync(claimsPrincipal)).Returns((
                Task.Run(() =>
                {
                    return new IdentityUser
                    {
                        Id = "TestUserId1",
                        UserName = "TestUser",
                        PasswordHash = "Pass123".GetHashCode().ToString()
                    };
                })));

            TodosController controller = new TodosController(
                mockTodosRepository.Object,
                mockTodoListsRepository.Object,
                mockRemindersRepository.Object,
                new ViewModelsFactory(),
                mockUsersManager.Object);

            TodoViewModel? result =
                 (controller.Add(expectedListId) as ViewResult)?.ViewData.Model
                 as TodoViewModel;

            Assert.NotNull(result);

            Assert.Equal(todos[0].ListId, result.ListId);
        }

        [Fact]
        public void CanAddValidTodo()
        {
            Todo todoToAdd = new Todo
            {
                Id = 3,
                Title = "TestTitle3",
                Description = "TestDescription3",
                CreationDate = DateTime.Now.Date,
                DueDate = DateTime.Now.AddDays(3).Date,
                ListId = 1,
            };

            Todo[] todos = new Todo[]
            {
                new Todo {
                    Id = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(3).Date,
                    ListId = 1,
                },

                new Todo {
                    Id = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(4).Date,
                    ListId = 1,
                },
            };

            Mock<ITodosRepository> mockTodosRepository = new Mock<ITodosRepository>();
            mockTodosRepository.Setup(m => m.Todos).Returns(todos.AsQueryable());

            Mock<ITodoListsRepository> mockTodoListsRepository = new Mock<ITodoListsRepository>();
            mockTodoListsRepository.Setup(m => m.TodoLists).Returns((new TodoList[]
            {
                new TodoList {
                    Id = 1,
                    Title = "ListTestTitle1",
                },

                new TodoList {
                    Id = 2,
                    Title = "ListTestTitle1",
                },
            }).AsQueryable());

            Mock<IRemindersRepository> mockRemindersRepository = new Mock<IRemindersRepository>();
            mockRemindersRepository.Setup(m => m.Reminders).Returns((new Reminder[]
            {
                new Reminder {
                    Id = 1,
                    RemindTime = DateTime.Now.AddDays(6).Date,
                    UserId = "TestUserId1",
                },

                new Reminder {
                    Id = 2,
                    RemindTime = DateTime.Now.AddDays(7).Date,
                    UserId = "TestUserId1",
                },
            }).AsQueryable());

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Mock<UserManager<IdentityUser>> mockUsersManager = GetMockUserManager();

            mockUsersManager.Setup(m => m.GetUserAsync(claimsPrincipal)).Returns((
                Task.Run(() =>
                {
                    return new IdentityUser
                    {
                        Id = "TestUserId1",
                        UserName = "TestUser",
                        PasswordHash = "Pass123".GetHashCode().ToString()
                    };
                })));

            TodosController controller = new TodosController(
                mockTodosRepository.Object,
                mockTodoListsRepository.Object,
                mockRemindersRepository.Object,
                new ViewModelsFactory(),
                mockUsersManager.Object);

            RedirectToActionResult? result = controller.Add(todoToAdd) as RedirectToActionResult;

            Assert.NotNull(result);

            string expectedActionName = "Todos";
            string expectedControllerName = "TodoLists";

            Assert.Equal(expectedActionName, result.ActionName);
            Assert.Equal(expectedControllerName, result.ControllerName);
        }

        [Fact]
        public void CanResendFormOnInvalidTodo()
        {
            Todo todoToAdd = new Todo
            {
                ListId = 1,
            };

            Todo[] todos = new Todo[]
            {
                new Todo {
                    Id = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(3).Date,
                    ListId = 1,
                },

                new Todo {
                    Id = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(4).Date,
                    ListId = 1,
                },
            };

            Mock<ITodosRepository> mockTodosRepository = new Mock<ITodosRepository>();
            mockTodosRepository.Setup(m => m.Todos).Returns(todos.AsQueryable());

            Mock<ITodoListsRepository> mockTodoListsRepository = new Mock<ITodoListsRepository>();
            mockTodoListsRepository.Setup(m => m.TodoLists).Returns((new TodoList[]
            {
                new TodoList {
                    Id = 1,
                    Title = "ListTestTitle1",
                },

                new TodoList {
                    Id = 2,
                    Title = "ListTestTitle1",
                },
            }).AsQueryable());

            Mock<IRemindersRepository> mockRemindersRepository = new Mock<IRemindersRepository>();
            mockRemindersRepository.Setup(m => m.Reminders).Returns((new Reminder[]
            {
                new Reminder {
                    Id = 1,
                    RemindTime = DateTime.Now.AddDays(6).Date,
                    UserId = "TestUserId1",
                },

                new Reminder {
                    Id = 2,
                    RemindTime = DateTime.Now.AddDays(7).Date,
                    UserId = "TestUserId1",
                },
            }).AsQueryable());

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Mock<UserManager<IdentityUser>> mockUsersManager = GetMockUserManager();

            mockUsersManager.Setup(m => m.GetUserAsync(claimsPrincipal)).Returns((
                Task.Run(() =>
                {
                    return new IdentityUser
                    {
                        Id = "TestUserId1",
                        UserName = "TestUser",
                        PasswordHash = "Pass123".GetHashCode().ToString()
                    };
                })));

            TodosController controller = new TodosController(
                mockTodosRepository.Object,
                mockTodoListsRepository.Object,
                mockRemindersRepository.Object,
                new ViewModelsFactory(),
                mockUsersManager.Object);

            TodoViewModel? result =
                 (controller.Add(todoToAdd) as ViewResult)?.ViewData.Model
                 as TodoViewModel;

            Assert.NotNull(result);

            Assert.Equal(todoToAdd.ListId, result.ListId);
        }

        [Fact]
        public void CanDeleteExistingTodo()
        {
            Todo[] todos = new Todo[]
            {
                new Todo {
                    Id = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(3).Date,
                    ListId = 1,
                },

                new Todo {
                    Id = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(4).Date,
                    ListId = 1,
                },
            };

            Mock<ITodosRepository> mockTodosRepository = new Mock<ITodosRepository>();
            mockTodosRepository.Setup(m => m.Todos).Returns(todos.AsQueryable());

            Mock<ITodoListsRepository> mockTodoListsRepository = new Mock<ITodoListsRepository>();
            mockTodoListsRepository.Setup(m => m.TodoLists).Returns((new TodoList[]
            {
                new TodoList {
                    Id = 1,
                    Title = "ListTestTitle1",
                },

                new TodoList {
                    Id = 2,
                    Title = "ListTestTitle1",
                },
            }).AsQueryable());

            Mock<IRemindersRepository> mockRemindersRepository = new Mock<IRemindersRepository>();
            mockRemindersRepository.Setup(m => m.Reminders).Returns((new Reminder[]
            {
                new Reminder {
                    Id = 1,
                    RemindTime = DateTime.Now.AddDays(6).Date,
                    UserId = "TestUserId1",
                },

                new Reminder {
                    Id = 2,
                    RemindTime = DateTime.Now.AddDays(7).Date,
                    UserId = "TestUserId1",
                },
            }).AsQueryable());

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Mock<UserManager<IdentityUser>> mockUsersManager = GetMockUserManager();

            mockUsersManager.Setup(m => m.GetUserAsync(claimsPrincipal)).Returns((
                Task.Run(() =>
                {
                    return new IdentityUser
                    {
                        Id = "TestUserId1",
                        UserName = "TestUser",
                        PasswordHash = "Pass123".GetHashCode().ToString()
                    };
                })));

            TodosController controller = new TodosController(
                mockTodosRepository.Object,
                mockTodoListsRepository.Object,
                mockRemindersRepository.Object,
                new ViewModelsFactory(),
                mockUsersManager.Object);

            RedirectToActionResult? result = controller.Delete(todos[0].Id) as RedirectToActionResult;

            Assert.NotNull(result);

            string expectedActionName = "Todos";
            string expectedControllerName = "TodoLists";

            Assert.Equal(expectedActionName, result.ActionName);
            Assert.Equal(expectedControllerName, result.ControllerName);
        }

        [Fact]
        public void DeleteTodoThrowInvalidOperationException()
        {
            Todo[] todos = new Todo[]
            {
                new Todo {
                    Id = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(3).Date,
                    ListId = 1,
                },

                new Todo {
                    Id = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(4).Date,
                    ListId = 1,
                },
            };

            Mock<ITodosRepository> mockTodosRepository = new Mock<ITodosRepository>();
            mockTodosRepository.Setup(m => m.Todos).Returns(todos.AsQueryable());

            Mock<ITodoListsRepository> mockTodoListsRepository = new Mock<ITodoListsRepository>();
            mockTodoListsRepository.Setup(m => m.TodoLists).Returns((new TodoList[]
            {
                new TodoList {
                    Id = 1,
                    Title = "ListTestTitle1",
                },

                new TodoList {
                    Id = 2,
                    Title = "ListTestTitle1",
                },
            }).AsQueryable());

            Mock<IRemindersRepository> mockRemindersRepository = new Mock<IRemindersRepository>();
            mockRemindersRepository.Setup(m => m.Reminders).Returns((new Reminder[]
            {
                new Reminder {
                    Id = 1,
                    RemindTime = DateTime.Now.AddDays(6).Date,
                    UserId = "TestUserId1",
                },

                new Reminder {
                    Id = 2,
                    RemindTime = DateTime.Now.AddDays(7).Date,
                    UserId = "TestUserId1",
                },
            }).AsQueryable());

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Mock<UserManager<IdentityUser>> mockUsersManager = GetMockUserManager();

            mockUsersManager.Setup(m => m.GetUserAsync(claimsPrincipal)).Returns((
                Task.Run(() =>
                {
                    return new IdentityUser
                    {
                        Id = "TestUserId1",
                        UserName = "TestUser",
                        PasswordHash = "Pass123".GetHashCode().ToString()
                    };
                })));

            TodosController controller = new TodosController(
                mockTodosRepository.Object,
                mockTodoListsRepository.Object,
                mockRemindersRepository.Object,
                new ViewModelsFactory(),
                mockUsersManager.Object);

            long todoId = 0;

            Assert.Throws<InvalidOperationException>(() => controller.Delete(todoId));
        }

        [Fact]
        public void CanUpdateExistingTodo()
        {
            Todo[] todos = new Todo[]
            {
                new Todo {
                    Id = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(3).Date,
                    ListId = 1,
                },

                new Todo {
                    Id = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(4).Date,
                    ListId = 1,
                },
            };

            Mock<ITodosRepository> mockTodosRepository = new Mock<ITodosRepository>();
            mockTodosRepository.Setup(m => m.Todos).Returns(todos.AsQueryable());

            Mock<ITodoListsRepository> mockTodoListsRepository = new Mock<ITodoListsRepository>();
            mockTodoListsRepository.Setup(m => m.TodoLists).Returns((new TodoList[]
            {
                new TodoList {
                    Id = 1,
                    Title = "ListTestTitle1",
                },

                new TodoList {
                    Id = 2,
                    Title = "ListTestTitle1",
                },
            }).AsQueryable());

            Mock<IRemindersRepository> mockRemindersRepository = new Mock<IRemindersRepository>();
            mockRemindersRepository.Setup(m => m.Reminders).Returns((new Reminder[]
            {
                new Reminder {
                    Id = 1,
                    RemindTime = DateTime.Now.AddDays(6).Date,
                    UserId = "TestUserId1",
                },

                new Reminder {
                    Id = 2,
                    RemindTime = DateTime.Now.AddDays(7).Date,
                    UserId = "TestUserId1",
                },
            }).AsQueryable());

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Mock<UserManager<IdentityUser>> mockUsersManager = GetMockUserManager();

            mockUsersManager.Setup(m => m.GetUserAsync(claimsPrincipal)).Returns((
                Task.Run(() =>
                {
                    return new IdentityUser
                    {
                        Id = "TestUserId1",
                        UserName = "TestUser",
                        PasswordHash = "Pass123".GetHashCode().ToString()
                    };
                })));

            TodosController controller = new TodosController(
                mockTodosRepository.Object,
                mockTodoListsRepository.Object,
                mockRemindersRepository.Object,
                new ViewModelsFactory(),
                mockUsersManager.Object);

            RedirectToActionResult? result = controller.Update(todos[0]) as RedirectToActionResult;

            Assert.NotNull(result);

            string expectedActionName = "Info";

            Assert.Equal(expectedActionName, result.ActionName);
        }

        [Fact]
        public void UpdateTodoThrowInvalidOperationException()
        {
            Todo[] todos = new Todo[]
            {
                new Todo {
                    Id = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(3).Date,
                    ListId = 1,
                },

                new Todo {
                    Id = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(4).Date,
                    ListId = 1,
                },
            };

            Mock<ITodosRepository> mockTodosRepository = new Mock<ITodosRepository>();
            mockTodosRepository.Setup(m => m.Todos).Returns(todos.AsQueryable());

            Mock<ITodoListsRepository> mockTodoListsRepository = new Mock<ITodoListsRepository>();
            mockTodoListsRepository.Setup(m => m.TodoLists).Returns((new TodoList[]
            {
                new TodoList {
                    Id = 1,
                    Title = "ListTestTitle1",
                },

                new TodoList {
                    Id = 2,
                    Title = "ListTestTitle1",
                },
            }).AsQueryable());

            Mock<IRemindersRepository> mockRemindersRepository = new Mock<IRemindersRepository>();
            mockRemindersRepository.Setup(m => m.Reminders).Returns((new Reminder[]
            {
                new Reminder {
                    Id = 1,
                    RemindTime = DateTime.Now.AddDays(6).Date,
                    UserId = "TestUserId1",
                },

                new Reminder {
                    Id = 2,
                    RemindTime = DateTime.Now.AddDays(7).Date,
                    UserId = "TestUserId1",
                },
            }).AsQueryable());

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Mock<UserManager<IdentityUser>> mockUsersManager = GetMockUserManager();

            mockUsersManager.Setup(m => m.GetUserAsync(claimsPrincipal)).Returns((
                Task.Run(() =>
                {
                    return new IdentityUser
                    {
                        Id = "TestUserId1",
                        UserName = "TestUser",
                        PasswordHash = "Pass123".GetHashCode().ToString()
                    };
                })));

            TodosController controller = new TodosController(
                mockTodosRepository.Object,
                mockTodoListsRepository.Object,
                mockRemindersRepository.Object,
                new ViewModelsFactory(),
                mockUsersManager.Object);

            long todoId = 0;

            Assert.Throws<InvalidOperationException>(() => controller.Update(todoId));
        }

        [Fact]
        public void CanSetReminder()
        {
            Todo[] todos = new Todo[]
           {
                new Todo {
                    Id = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(3).Date,
                    ListId = 1,
                },

                new Todo {
                    Id = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(4).Date,
                    ListId = 1,
                },
           };

            Mock<ITodosRepository> mockTodosRepository = new Mock<ITodosRepository>();
            mockTodosRepository.Setup(m => m.Todos).Returns(todos.AsQueryable());

            Mock<ITodoListsRepository> mockTodoListsRepository = new Mock<ITodoListsRepository>();
            mockTodoListsRepository.Setup(m => m.TodoLists).Returns((new TodoList[]
            {
                new TodoList {
                    Id = 1,
                    Title = "ListTestTitle1",
                },

                new TodoList {
                    Id = 2,
                    Title = "ListTestTitle1",
                },
            }).AsQueryable());

            Mock<IRemindersRepository> mockRemindersRepository = new Mock<IRemindersRepository>();
            mockRemindersRepository.Setup(m => m.Reminders).Returns((new Reminder[]
            {
                new Reminder {
                    Id = 1,
                    RemindTime = DateTime.Now.AddDays(6).Date,
                    UserId = "TestUserId1",
                },

                new Reminder {
                    Id = 2,
                    RemindTime = DateTime.Now.AddDays(7).Date,
                    UserId = "TestUserId1",
                },
            }).AsQueryable());

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Mock<UserManager<IdentityUser>> mockUsersManager = GetMockUserManager();

            mockUsersManager.Setup(m => m.GetUserAsync(claimsPrincipal)).Returns((
                Task.Run(() =>
                {
                    return new IdentityUser
                    {
                        Id = "TestUserId1",
                        UserName = "TestUser",
                        PasswordHash = "Pass123".GetHashCode().ToString()
                    };
                })));

            TodosController controller = new TodosController(
                mockTodosRepository.Object,
                mockTodoListsRepository.Object,
                mockRemindersRepository.Object,
                new ViewModelsFactory(),
                mockUsersManager.Object);

            ReminderViewModel? result =
                 (controller.SetReminder(todos[0].Id) as ViewResult)?.ViewData.Model
                 as ReminderViewModel;

            Assert.NotNull(result);

            Assert.Equal(todos[0].Id, result.TodoId);
            Assert.Equal(todos[0].ListId, result.ListId);
        }

        [Fact]
        public void SetReminderThrowInvalidOperationException()
        {
            Todo[] todos = new Todo[]
            {
                new Todo {
                    Id = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(3).Date,
                    ListId = 1,
                },

                new Todo {
                    Id = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(4).Date,
                    ListId = 1,
                },
            };

            Mock<ITodosRepository> mockTodosRepository = new Mock<ITodosRepository>();
            mockTodosRepository.Setup(m => m.Todos).Returns(todos.AsQueryable());

            Mock<ITodoListsRepository> mockTodoListsRepository = new Mock<ITodoListsRepository>();
            mockTodoListsRepository.Setup(m => m.TodoLists).Returns((new TodoList[]
            {
                new TodoList {
                    Id = 1,
                    Title = "ListTestTitle1",
                },

                new TodoList {
                    Id = 2,
                    Title = "ListTestTitle1",
                },
            }).AsQueryable());

            Mock<IRemindersRepository> mockRemindersRepository = new Mock<IRemindersRepository>();
            mockRemindersRepository.Setup(m => m.Reminders).Returns((new Reminder[]
            {
                new Reminder {
                    Id = 1,
                    RemindTime = DateTime.Now.AddDays(6).Date,
                    UserId = "TestUserId1",
                },

                new Reminder {
                    Id = 2,
                    RemindTime = DateTime.Now.AddDays(7).Date,
                    UserId = "TestUserId1",
                },
            }).AsQueryable());

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Mock<UserManager<IdentityUser>> mockUsersManager = GetMockUserManager();

            mockUsersManager.Setup(m => m.GetUserAsync(claimsPrincipal)).Returns((
                Task.Run(() =>
                {
                    return new IdentityUser
                    {
                        Id = "TestUserId1",
                        UserName = "TestUser",
                        PasswordHash = "Pass123".GetHashCode().ToString()
                    };
                })));

            TodosController controller = new TodosController(
                mockTodosRepository.Object,
                mockTodoListsRepository.Object,
                mockRemindersRepository.Object,
                new ViewModelsFactory(),
                mockUsersManager.Object);

            long todoId = 0;

            Assert.Throws<InvalidOperationException>(() => controller.SetReminder(todoId));
        }

        [Fact]
        public void CanResendFormOnInvalidReminder()
        {
            Reminder reminderToAdd = new Reminder
            {
                TodoId = 1,
            };

            Todo[] todos = new Todo[]
            {
                new Todo {
                    Id = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(3).Date,
                    ListId = 1,
                },

                new Todo {
                    Id = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2",
                    CreationDate = DateTime.Now.Date,
                    DueDate = DateTime.Now.AddDays(4).Date,
                    ListId = 1,
                },
            };

            Mock<ITodosRepository> mockTodosRepository = new Mock<ITodosRepository>();
            mockTodosRepository.Setup(m => m.Todos).Returns(todos.AsQueryable());

            Mock<ITodoListsRepository> mockTodoListsRepository = new Mock<ITodoListsRepository>();
            mockTodoListsRepository.Setup(m => m.TodoLists).Returns((new TodoList[]
            {
                new TodoList {
                    Id = 1,
                    Title = "ListTestTitle1",
                },

                new TodoList {
                    Id = 2,
                    Title = "ListTestTitle1",
                },
            }).AsQueryable());

            Mock<IRemindersRepository> mockRemindersRepository = new Mock<IRemindersRepository>();
            mockRemindersRepository.Setup(m => m.Reminders).Returns((new Reminder[]
            {
                new Reminder {
                    Id = 1,
                    RemindTime = DateTime.Now.AddDays(6).Date,
                    UserId = "TestUserId1",
                },

                new Reminder {
                    Id = 2,
                    RemindTime = DateTime.Now.AddDays(7).Date,
                    UserId = "TestUserId1",
                },
            }).AsQueryable());

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            Mock<UserManager<IdentityUser>> mockUsersManager = GetMockUserManager();

            mockUsersManager.Setup(m => m.GetUserAsync(claimsPrincipal)).Returns((
                Task.Run(() =>
                {
                    return new IdentityUser
                    {
                        Id = "TestUserId1",
                        UserName = "TestUser",
                        PasswordHash = "Pass123".GetHashCode().ToString()
                    };
                })));

            TodosController controller = new TodosController(
                mockTodosRepository.Object,
                mockTodoListsRepository.Object,
                mockRemindersRepository.Object,
                new ViewModelsFactory(),
                mockUsersManager.Object);

            RedirectToActionResult? result = controller.SetReminder(reminderToAdd) as RedirectToActionResult;

            Assert.NotNull(result);

            string expectedActionName = "SetReminder";
            Assert.Equal(expectedActionName, result.ActionName);
        }

        private static Mock<UserManager<IdentityUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();

            return new Mock<UserManager<IdentityUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}
