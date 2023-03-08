using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp_DomainEntities;
using TodoApp_Tests.RepositoriesTests.Models;
using TodoAppWeb.Models.Repository;
using TodoAppWeb.Models;

namespace TodoApp_Tests.RepositoriesTests
{
#pragma warning disable CS8625

    public class RemindersRepositoryTests
    {
        private readonly ITodosDbContext context;
        private readonly IRemindersRepository repository;

        public RemindersRepositoryTests()
        {
            this.context = new TestDataDbContext();
            this.repository = new EFRemindersRepository(context);
        }

        [Fact]
        public void CanGetReminders()
        {
            var lists = repository.Reminders.ToArray();

            AddTestData();

            Assert.NotNull(lists);
            Assert.NotEmpty(lists);
        }

        [Fact]
        public void CanSaveReminder()
        {
            Reminder reminder = GetTestReminderData();

            Assert.DoesNotContain(reminder, GetAllReminders());

            repository.CreateReminder(reminder);

            Assert.Contains(reminder, GetAllReminders());
        }

        [Fact]
        public void SaveReminderThrowNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => repository.CreateReminder(null));
        }

        [Fact]
        public void CanDeleteReminder()
        {
            Reminder reminder = repository.Reminders.FirstOrDefault()
                ?? throw new InvalidOperationException("Reminder is null.");

            Assert.Contains(reminder, GetAllReminders());

            repository.DeleteReminder(reminder);

            Assert.DoesNotContain(reminder, GetAllReminders());
        }

        [Fact]
        public void DeleteReminderThrowNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => repository.DeleteReminder(null));
        }

        [Fact]
        public void DeleteReminderThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => repository.DeleteReminder(new Reminder()));
        }

        private Reminder[] GetAllReminders()
        {
            return context.Reminders.ToArray();
        }

        private Reminder GetTestReminderData()
        {
            return new Reminder
            {
                RemindTime = DateTime.Now.AddDays(3),
            };
        }

        private void AddTestData()
        {
            Reminder reminder = GetTestReminderData();
            repository.CreateReminder(reminder);
        }
    }
#pragma warning restore CS8625
}
