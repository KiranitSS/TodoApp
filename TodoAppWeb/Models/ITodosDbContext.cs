using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoApp_DomainEntities;

namespace TodoAppWeb.Models
{
    public interface ITodosDbContext : IRemindersDbContext
    {
        DbSet<TodoList> TodoLists { get; }
    }
}