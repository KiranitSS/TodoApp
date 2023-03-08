using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoAppWeb.Infrastructure;
using TodoAppWeb.Models;
using TodoAppWeb.Models.Repository;

namespace TodoApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            AddServices(builder);
            AddDbContexts(builder);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            AddControllerRoutes(app);

            IdentitySeedData identityData = new IdentitySeedData();
            identityData.EnsurePopulated(app).Wait();
            TodosSeedData todosSeed = new TodosSeedData();
            todosSeed.EnsurePopulated(app);

            app.Run();
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ITodosDbContext, TodosDbContext>();
            builder.Services.AddSingleton<IViewModelsFactory, ViewModelsFactory>();
            builder.Services.AddSingleton<IIdentityUsersFactory, IdentityUsersFactory>();
            builder.Services.AddScoped<ITodosRepository, EFTodosRepository>();
            builder.Services.AddScoped<ITodoListsRepository, EFTodoListsRepository>();
            builder.Services.AddScoped<IUsersRepository, EFUsersRepository>();
            builder.Services.AddScoped<IRemindersRepository, EFRemindersRepository>();
        }

        private static void AddDbContexts(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<TodosDbContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration["ConnectionStrings:TodosConnection"]);
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"]);
            });
        }

        private static void AddControllerRoutes(WebApplication app)
        {
            app.MapControllerRoute(
                name: "default",
                pattern: "/",
                defaults: new { Controller = "Home", Action = "Index" });

            app.MapControllerRoute(
                name: "lists",
                pattern: "/Lists",
                defaults: new { Controller = "TodoLists", Action = "Lists" });

            app.MapControllerRoute(
                name: "listInfo",
                pattern: "/Lists/Info/{listId:long}",
                defaults: new { Controller = "TodoLists", Action = "TodosList" });

            app.MapControllerRoute(
                name: "AddList",
                pattern: "/Lists/Add/{title}",
                defaults: new { Controller = "TodoLists", Action = "AddList" });
        }
    }
}