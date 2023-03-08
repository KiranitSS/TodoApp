using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TodoApp_DomainEntities;
using TodoAppWeb.Infrastructure;
using TodoAppWeb.Migrations;
using TodoAppWeb.Models.Repository;
using TodoAppWeb.Models.ViewModels;

namespace TodoAppWeb.Controllers
{
    [Authorize]
    [Route("User")]
    [Route("List/Todos")]
    public class TodosController : Controller
    {
        private readonly ITodosRepository todosRepository;
        private readonly ITodoListsRepository todoListsRepository;
        private readonly IRemindersRepository remindersRepository;
        private readonly IViewModelsFactory factory;
        private readonly UserManager<IdentityUser> userManager;

        public TodosController(
            ITodosRepository todosRepository,
            ITodoListsRepository todoListsRepository,
            IRemindersRepository remindersRepository,
            IViewModelsFactory factory,
            UserManager<IdentityUser> userManager)
        {
            this.todosRepository = todosRepository;
            this.todoListsRepository = todoListsRepository;
            this.remindersRepository = remindersRepository;
            this.factory = factory;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("Info/{todoId:long}")]
        public IActionResult Info(long todoId)
        {
            Todo? todo = todosRepository.Todos.FirstOrDefault(t => t.Id == todoId);

            if (todo is null)
            {
                throw new InvalidOperationException("There is no such todo.");
            }

            return View(factory.CreateTodoViewModel(todo));
        }

        [HttpGet]
        [Route("Add/{listId:long}")]
        public IActionResult Add(long listId)
        {
            return View(factory.CreateTodoViewModel(listId));
        }

        [HttpPost]
        [Route("Add/{listId:long}")]
        public IActionResult Add(Todo todo)
        {
            if (ModelState.IsValid && todo.DueDate > todo.CreationDate)
            {
                todosRepository.CreateTodo(todo);

                return RedirectToAction("Todos", "TodoLists", new { listId = todo.ListId });
            }

            return View(factory.CreateTodoViewModel(todo.ListId));
        }

        [HttpPost]
        [Route("Delete/{todoId:long}")]
        public IActionResult Delete(long todoId)
        {
            Todo? todo = todosRepository.Todos.FirstOrDefault(t => t.Id == todoId);

            if (todo is null)
            {
                throw new InvalidOperationException("There is no such todo.");
            }

            todosRepository.DeleteTodo(todo);

            return RedirectToAction("Todos", "TodoLists", new { listId = todo.ListId });
        }

#pragma warning disable S4144
        [HttpGet]
        [Route("Update/{todoId:long}")]
        public IActionResult Update(long todoId)
        {
            Todo? todo = todosRepository.Todos.FirstOrDefault(t => t.Id == todoId);

            if (todo is null)
            {
                throw new InvalidOperationException("There is no such todo.");
            }

            return View(factory.CreateTodoViewModel(todo));
        }
#pragma warning restore S4144

        [HttpPost]
        [Route("Update/{todoId:long}")]
        public IActionResult Update(Todo todo)
        {
            if (ModelState.IsValid && todo.DueDate > todo.CreationDate)
            {
                todosRepository.UpdateTodo(todo);

                return RedirectToAction("Info", new { todoId = todo.Id });
            }

            Todo? oldTodo = todosRepository.Todos.FirstOrDefault(t => t.Id == todo.Id);

            if (oldTodo is null)
            {
                throw new InvalidOperationException("There is no such todo.");
            }

            return View(factory.CreateTodoViewModel(oldTodo));
        }

        [HttpGet]
        [Route("Upcoming")]
        public IActionResult Upcoming()
        {
            const int daysToEnd = 3;
            DateTime upcoming = DateTime.Now.AddDays(daysToEnd);

            var todos = GetCurrentUserTodos()
                .Where(t => t.DueDate.Date <= upcoming.Date).ToArray();

            return View(factory.CreateTodoListViewModel(todos));
        }

        [HttpGet]
        [Route("Notifications")]
        public IActionResult Notifications()
        {
            const int daysToEnd = 30;

            var reminders = remindersRepository.GetUpcomingReminders(daysToEnd, GetCurrentUser().Id);
            var todoIds = reminders.Select(r => r.TodoId);

            var todos = GetCurrentUserTodos()
                .Where(t => t.Status != TodoStatus.Completed
                && todoIds.Contains(t.Id))
                .ToArray();

            return View(factory.CreateTodoListViewModel(todos));
        }

        [HttpGet]
        [Route("SetReminder/{todoId:long}")]
        public IActionResult SetReminder(long todoId)
        {
            Todo? todo = todosRepository.Todos.FirstOrDefault(t => t.Id == todoId);

            if (todo is null)
            {
                throw new InvalidOperationException("There is no such todo.");
            }

            return View(factory.CreateReminderViewModel(todo));
        }

        [HttpPost]
        [Route("SetReminder/{todoId:long}")]
        public IActionResult SetReminder(Reminder reminder)
        {
            if (ModelState.IsValid && reminder.RemindTime > DateTime.Now)
            {
                reminder.UserId = GetCurrentUser().Id;

                remindersRepository.CreateReminder(reminder);

                return RedirectToAction("Notifications");
            }

            return RedirectToAction("SetReminder", new { todoId = reminder.TodoId });
        }

        private IdentityUser GetCurrentUser()
         {
            var getUserTask = this.userManager.GetUserAsync(HttpContext.User);
            getUserTask.Wait();
            return getUserTask.Result;
        }

        private IEnumerable<Todo> GetCurrentUserTodos()
        {
            var currentUser = GetCurrentUser();

            return todoListsRepository.TodoLists
                .Where(l => l.UserId == currentUser.Id)
                .Include(l => l.Todos)
                .SelectMany(l => l.Todos);
        }
    }
}
