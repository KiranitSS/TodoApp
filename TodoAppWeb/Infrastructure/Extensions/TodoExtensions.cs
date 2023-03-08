using TodoApp_DomainEntities;

namespace TodoAppWeb.Infrastructure.Extensions
{
    public static class TodoExtensions
    {
        public static Todo GetCopy(this Todo todo) 
        {
            Todo copy = new Todo
            {
                Title = todo.Title,
                Description = todo.Description,
                Status = todo.Status,
                DueDate = todo.DueDate,
                CreationDate = todo.CreationDate,
            };

            return copy;
        }
    }
}
