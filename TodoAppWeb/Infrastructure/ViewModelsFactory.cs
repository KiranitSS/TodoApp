using System.Collections.Generic;
using TodoApp_DomainEntities;
using TodoAppWeb.Models.ViewModels;

namespace TodoAppWeb.Infrastructure
{
    public class ViewModelsFactory : IViewModelsFactory
    {
        public TodoViewModel CreateTodoViewModel()
        {
            return new TodoViewModel();
        }

        public TodoViewModel CreateTodoViewModel(long listId)
        {
            return new TodoViewModel
            {
                ListId = listId,
                DueDate = DateTime.Now.Date
            };
        }

        public TodoViewModel CreateTodoViewModel(Todo todo)
        {
            return new TodoViewModel
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                DueDate = todo.DueDate,
                CreationDate = todo.CreationDate,
                Status = todo.Status,
                ListId = todo.ListId,
            };
        }

        public TodoListViewModel CreateTodoListViewModel()
        {
            return new TodoListViewModel();
        }

        public TodoListViewModel CreateTodoListViewModel(string title)
        {
            return new TodoListViewModel { Title = title };
        }

        public TodoListViewModel CreateTodoListViewModel(TodoList list)
        {
            IEnumerable<TodoViewModel> todos = list.Todos.Select(t => this.CreateTodoViewModel(t)).ToArray();

            return new TodoListViewModel
            {
                Id = list.Id,
                Title = list.Title,
                Todos = todos,
                IsDeleted = list.IsDeleted,
            };
        }

        public TodoListViewModel CreateTodoListViewModel(ICollection<Todo> todos)
        {
            IEnumerable<TodoViewModel> todoViewModels = todos.Select(t => this.CreateTodoViewModel(t)).ToArray();

            return new TodoListViewModel
            {
                Todos = todoViewModels,
            };
        }

        public ReminderViewModel CreateReminderViewModel(Todo todo)
        {
            ReminderViewModel reminder = new ReminderViewModel
            {
                ListId = todo.ListId,
                TodoId = todo.Id,
                RemindTime = DateTime.Now.Date,
            };

            return reminder;
        }
    }
}
