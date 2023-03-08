using Microsoft.EntityFrameworkCore;
using TodoApp_DomainEntities;

namespace TodoAppWeb.Models.Repository
{
    public class EFRemindersRepository : IRemindersRepository
    {
        private readonly ITodosDbContext context;

        public EFRemindersRepository(ITodosDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Reminder> Reminders
        {
            get => context.Reminders;
        }

        public void CreateReminder(Reminder reminder)
        {
            if (reminder is null)
            {
                throw new ArgumentNullException(nameof(reminder));
            }

            context.Add(reminder);
            context.SaveChanges();
        }

        public void DeleteReminder(Reminder reminder)
        {
            if (reminder is null)
            {
                throw new ArgumentNullException(nameof(reminder));
            }

            Reminder result = context.Reminders.FirstOrDefault(r => r.Id == reminder.Id)
               ?? throw new InvalidOperationException("No such reminder found.");

            context.Remove(result);
            context.SaveChanges();
        }

        public IEnumerable<Reminder> GetUpcomingReminders(int daysToEnd, string userId)
        {
            if (daysToEnd < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(daysToEnd), "Days to end can't be less than zero.");
            }

            DateTime upperBorder = DateTime.Now.AddDays(daysToEnd);
            IEnumerable<Reminder> reminders = context.Reminders
                .Where(r => r.UserId == userId 
                && r.RemindTime.Date < upperBorder.Date);

            return reminders;
        }
    }
}
