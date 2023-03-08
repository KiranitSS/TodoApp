using TodoApp_DomainEntities;

namespace TodoAppWeb.Models.Repository
{
    public interface ITodoListsRepository
    {
        IQueryable<TodoList> TodoLists { get; }

        long CreateList(TodoList list);

        void UpdateList(TodoList list);

        void DeleteList(TodoList list);
    }
}
