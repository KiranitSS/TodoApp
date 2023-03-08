using TodoApp_DomainEntities;

namespace TodoAppWeb.Models.Repository
{
    public interface IRemindersRepository
    {
        IQueryable<Reminder> Reminders { get; }

        void CreateReminder(Reminder reminder);

        void DeleteReminder(Reminder reminder);

        IEnumerable<Reminder> GetUpcomingReminders(int daysToEnd, string userId);
    }
}
