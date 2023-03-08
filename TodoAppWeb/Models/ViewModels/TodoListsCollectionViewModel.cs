using TodoApp_DomainEntities;

namespace TodoAppWeb.Models.ViewModels
{
    public class TodoListsCollectionViewModel
    {
        public IEnumerable<TodoList> TodoLists { get; set; } = Enumerable.Empty<TodoList>();
    }
}
