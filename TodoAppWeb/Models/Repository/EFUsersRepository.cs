using Microsoft.AspNetCore.Identity;

namespace TodoAppWeb.Models.Repository
{
    public class EFUsersRepository : IUsersRepository
    {
        private readonly AppIdentityDbContext context;

        public EFUsersRepository(AppIdentityDbContext context)
        {
            this.context = context;
        }

        public IQueryable<IdentityUser> Users
        {
            get
            {
                return this.context.Users;
            }
        }

        public void CreateUser(IdentityUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            context.Add(user);
            context.SaveChanges();
        }

        public void DeleteUser(IdentityUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IdentityUser userToDelete = context.Users.FirstOrDefault(u => u.Id == user.Id)
                ?? throw new InvalidOperationException("No such user found.");

            context.Remove(userToDelete);
            context.SaveChanges();
        }

        /// <summary>
        /// Updates user data.
        /// </summary>
        /// <param name="user">New user data.</param>
        /// <exception cref="NotSupportedException">User update not supported now.</exception>
        public void UpdateUser(IdentityUser user)
        {
            throw new NotSupportedException();
        }
    }
}
