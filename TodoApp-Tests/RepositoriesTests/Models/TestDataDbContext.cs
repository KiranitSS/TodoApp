using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApp_DomainEntities;
using TodoAppWeb.Models;

namespace TodoApp_Tests.RepositoriesTests.Models
{
    public class TestDataDbContext : DbContext, ITodosDbContext
    {
        static TestDataDbContext()
        {
            TestDataSeed.GetInstanse().SeedData();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "TestDb");
        }

        public DbSet<TodoList> TodoLists { get => this.Set<TodoList>(); }

        public DbSet<Todo> Todos { get => this.Set<Todo>(); }

        public DbSet<TestUser> Users { get => this.Set<TestUser>(); }

        public DbSet<ReminderList> ReminderLists { get => this.Set<ReminderList>(); }

        public DbSet<Reminder> Reminders { get => this.Set<Reminder>(); }
    }
}
