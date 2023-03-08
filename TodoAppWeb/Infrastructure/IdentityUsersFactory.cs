using Microsoft.AspNetCore.Identity;
using TodoAppWeb.Models.ViewModels;

namespace TodoAppWeb.Infrastructure
{
    public class IdentityUsersFactory : IIdentityUsersFactory
    {
        public IdentityUser CreateUserFromView(RegisterViewModel registerData)
        {
            return new IdentityUser
            {
                UserName = registerData.Name,
                Email = registerData.Email
            };
        }
    }
}
