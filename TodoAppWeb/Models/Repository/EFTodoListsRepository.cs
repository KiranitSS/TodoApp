using TodoApp_DomainEntities;

namespace TodoAppWeb.Models.Repository
{
    public class EFTodoListsRepository : ITodoListsRepository
    {
        private readonly ITodosDbContext context;

        public EFTodoListsRepository(ITodosDbContext context)
        {
            this.context = context;
        }

        public IQueryable<TodoList> TodoLists
        {
            get
            {
                return this.context.TodoLists;
            }
        }

        public long CreateList(TodoList list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            context.Add(list);
            context.SaveChanges();
            return list.Id;
        }

        public void DeleteList(TodoList list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var listToDelete = GetListById(list.Id)
                ?? throw new InvalidOperationException("No such list found.");

            context.RemoveRange(listToDelete.Todos);
            context.Remove(listToDelete);
            context.SaveChanges();
        }

        public void UpdateList(TodoList list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var listToUpdate = GetListById(list.Id)
                ?? throw new InvalidOperationException("No such list found.");

            listToUpdate.Title = list.Title;
            listToUpdate.IsDeleted = list.IsDeleted;
            context.SaveChanges();
        }

        public TodoList? GetListById(long id)
        {
            return context.TodoLists.FirstOrDefault(l => l.Id == id);
        }
    }
}
