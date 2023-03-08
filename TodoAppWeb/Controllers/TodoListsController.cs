using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using TodoApp_DomainEntities;
using TodoAppWeb.Infrastructure;
using TodoAppWeb.Infrastructure.Extensions;
using TodoAppWeb.Migrations;
using TodoAppWeb.Models.Repository;
using TodoAppWeb.Models.ViewModels;

namespace TodoAppWeb.Controllers
{
    [Authorize]
    [Route("User")]
    [Route("Lists")]
    public class TodoListsController : Controller
    {
        private readonly ITodoListsRepository repository;
        private readonly IViewModelsFactory factory;
        private readonly UserManager<IdentityUser> userManager;

        public TodoListsController(ITodoListsRepository repository, IViewModelsFactory factory, UserManager<IdentityUser> userManager)
        {
            this.repository = repository;
            this.factory = factory;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("Visible")]
        public IActionResult Lists()
        {
            var userLists = GetCurrentUserLists();
            var listsToShow = GetListsToShow(userLists);

            return base.View(new TodoListsCollectionViewModel
            {
                TodoLists = listsToShow,
            });
        }

        [HttpGet]
        [Route("Hidden")]
        public IActionResult HiddenLists()
        {
            var userLists = GetCurrentUserLists();
            var listsToShow = GetHiddenLists(userLists);

            return base.View(new TodoListsCollectionViewModel
            {
                TodoLists = listsToShow,
            });
        }

        [HttpGet]
        [Route("Info/{listId:long}")]
        public IActionResult Todos(long listId)
        {
            var list = repository.TodoLists
                .Include(l => l.Todos)
                .FirstOrDefault(l => l.Id == listId);

            if (list is null)
            {
                throw new InvalidOperationException("There is no such list.");
            }

            return View(factory.CreateTodoListViewModel(list));
        }

        [Route("Add/")]
        public IActionResult Add()
        {
            return View(factory.CreateTodoListViewModel(string.Empty));
        }

        [HttpPost]
        [Route("Add/")]
        public IActionResult Add(TodoList list)
        {
            if (ModelState.IsValid)
            {
                var currentUser = GetCurrentUser();
                list.UserId = currentUser.Id;

                repository.CreateList(list);

                return RedirectToAction("Lists");
            }

            return View(factory.CreateTodoListViewModel(string.Empty));
        }

        [HttpGet]
        [Route("ConfirmDelete/{listId:long}")]
        public IActionResult ConfirmDelete(long listId)
        {
            var list = GetListWithTodos(listId);

            if (list is null)
            {
                throw new InvalidOperationException("There is no such list.");
            }

            return View(factory.CreateTodoListViewModel(list));
        }

        [HttpPost]
        [Route("Delete/{listId:long}")]
        public IActionResult Delete(long listId)
        {
            var list = GetListWithTodos(listId);

            if (list is null)
            {
                throw new InvalidOperationException("There is no such list.");
            }

            repository.DeleteList(list);

            return RedirectToAction("Lists");
        }

        [HttpGet]
        [Route("Update/{listId:long}")]
        public IActionResult Update(long listId)
        {
            TodoList? list = repository.TodoLists.FirstOrDefault(l => l.Id == listId);

            if (list != null)
            {
                return View(factory.CreateTodoListViewModel(list));
            }

            return RedirectToAction("Lists");
        }

        [HttpPost]
        [Route("Update/{listId:long}")]
        public IActionResult Update(TodoList list)
        {
            if (ModelState.IsValid)
            {
                repository.UpdateList(list);

                return RedirectToAction("Lists");
            }

            TodoList? oldList = repository.TodoLists.FirstOrDefault(l => l.Id == list.Id);

            if (oldList is null)
            {
                throw new InvalidOperationException("There is no such list.");
            }

            return View(factory.CreateTodoListViewModel(oldList));
        }

        [HttpPost]
        [Route("Copy/{listId:long}")]
        public IActionResult Copy(long listId)
        {
            var oldList = GetListWithTodos(listId);

            if (oldList is null)
            {
                throw new InvalidOperationException("There is no such list.");
            }

            TodoList listCopy = new TodoList { Title = oldList.Title, UserId = oldList.UserId };

            long listCopyId = repository.CreateList(listCopy);

            foreach (var todo in oldList.Todos)
            {
                Todo todoCopy = todo.GetCopy();
                todoCopy.ListId = listCopyId;
                listCopy.Todos.Add(todoCopy);
            }

            repository.UpdateList(listCopy);

            return RedirectToAction("Lists");
        }

        [HttpPost]
        [Route("HideCompletedTodos/{listId:long}")]
        public IActionResult HideCompletedTodos(long listId)
        {
            var list = GetListWithTodos(listId);

            if (list is null)
            {
                throw new InvalidOperationException("There is no such list.");
            }

            var todos = list.Todos.Where(t => t.Status != TodoStatus.Completed).ToArray();
            list.Todos.Clear();

            foreach (var todo in todos)
            {
                list.Todos.Add(todo);
            }

            return this.View("Todos", factory.CreateTodoListViewModel(list));
        }

        [HttpPost]
        [Route("Hide/{listId:long}")]
        public IActionResult Hide(long listId)
        {
            TodoList? list = repository.TodoLists.FirstOrDefault(l => l.Id == listId);

            if (list is null)
            {
                throw new InvalidOperationException("There is no such list.");
            }

            list.IsDeleted = true;
            repository.UpdateList(list);

            return this.RedirectToAction("Lists");
        }

        [HttpPost]
        [Route("Unhide/{listId:long}")]
        public IActionResult Unhide(long listId)
        {
            TodoList? list = repository.TodoLists.FirstOrDefault(l => l.Id == listId);

            if (list is null)
            {
                throw new InvalidOperationException("There is no such list.");
            }

            list.IsDeleted = false;
            repository.UpdateList(list);

            return this.RedirectToAction("HiddenLists");
        }

        private IdentityUser GetCurrentUser()
        {
            var getUserTask = this.userManager.GetUserAsync(HttpContext.User);
            getUserTask.Wait();
            return getUserTask.Result;
        }

        private TodoList? GetListWithTodos(long listId)
        {
            return repository.TodoLists
                .Include(l => l.Todos)
                .FirstOrDefault(l => l.Id == listId);
        }

        private IEnumerable<TodoList> GetCurrentUserLists()
        {
            var currentUser = GetCurrentUser();

            return repository.TodoLists.Where(l => l.UserId == currentUser.Id);
        }

        private IEnumerable<TodoList> GetListsToShow(IEnumerable<TodoList> lists)
        {
            return lists.Where(l => !l.IsDeleted);
        }

        private IEnumerable<TodoList> GetHiddenLists(IEnumerable<TodoList> lists)
        {
            return lists.Where(l => l.IsDeleted);
        }
    }
}
