using Microsoft.AspNetCore.Identity;
using TodoApp_DomainEntities;

namespace TodoAppWeb.Models.Repository
{
    public interface IUsersRepository
    {
        IQueryable<IdentityUser> Users { get; }

        void CreateUser(IdentityUser user);

        void UpdateUser(IdentityUser user);

        void DeleteUser(IdentityUser user);
    }
}
