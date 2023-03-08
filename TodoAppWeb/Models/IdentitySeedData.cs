using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TodoAppWeb.Models
{
    public class IdentitySeedData
    {
        private const string DefaultUser = "Alex";
        private const string DefaultUserPassword = "Secret123$";

        public async Task EnsurePopulated(IApplicationBuilder app)
        {
            AppIdentityDbContext context = app.ApplicationServices
            .CreateScope().ServiceProvider.GetRequiredService<AppIdentityDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            UserManager<IdentityUser> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            IdentityUser user = await userManager.FindByNameAsync(DefaultUser);

            if (user is null)
            {
                user = new IdentityUser("Alex")
                {
                    Email = "alex@lock.com",
                    PhoneNumber = "123-4567",
                };

                await userManager.CreateAsync(user, DefaultUserPassword);
            }
        }
    }
}
