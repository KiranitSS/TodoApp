using Microsoft.AspNetCore.Identity;
using TodoAppWeb.Models.ViewModels;

namespace TodoAppWeb.Infrastructure
{
    public interface IIdentityUsersFactory
    {
        IdentityUser CreateUserFromView(RegisterViewModel registerData);
    }
}
