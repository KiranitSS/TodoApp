using Microsoft.EntityFrameworkCore;
using TodoApp_DomainEntities;

namespace TodoAppWeb.Models
{
    public interface IRemindersDbContext : IDbContext
    {
        DbSet<ReminderList> ReminderLists { get; }

        DbSet<Reminder> Reminders { get; }
    }
}