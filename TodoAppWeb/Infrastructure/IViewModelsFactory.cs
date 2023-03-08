using TodoApp_DomainEntities;
using TodoAppWeb.Models.ViewModels;

namespace TodoAppWeb.Infrastructure
{
    public interface IViewModelsFactory
    {
        TodoViewModel CreateTodoViewModel();

        TodoViewModel CreateTodoViewModel(long listId);

        TodoViewModel CreateTodoViewModel(Todo todo);

        TodoListViewModel CreateTodoListViewModel();

        TodoListViewModel CreateTodoListViewModel(string title);

        TodoListViewModel CreateTodoListViewModel(TodoList list);

        TodoListViewModel CreateTodoListViewModel(ICollection<Todo> todos);

        ReminderViewModel CreateReminderViewModel(Todo todo);
    }
}
