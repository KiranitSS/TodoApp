using TodoApp_DomainEntities;

namespace TodoAppWeb.Models.Repository
{
    public interface ITodosRepository
    {
        IQueryable<Todo> Todos { get; }

        void CreateTodo(Todo todo);

        void UpdateTodo(Todo todo);

        void DeleteTodo(Todo todo);
    }
}
