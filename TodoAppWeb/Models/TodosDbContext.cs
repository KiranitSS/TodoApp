using Microsoft.EntityFrameworkCore;
using TodoApp_DomainEntities;

namespace TodoAppWeb.Models
{
    public class TodosDbContext : DbContext, ITodosDbContext
    {
        public TodosDbContext(DbContextOptions<TodosDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoList> TodoLists { get => this.Set<TodoList>(); }

        public DbSet<ReminderList> ReminderLists { get => this.Set<ReminderList>(); }

        public DbSet<Reminder> Reminders { get => this.Set<Reminder>(); }
    }
}
