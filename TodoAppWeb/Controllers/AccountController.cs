using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TodoAppWeb.Infrastructure;
using TodoAppWeb.Models.Repository;
using TodoAppWeb.Models.ViewModels;

namespace TodoAppWeb.Controllers
{
    [Authorize]
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IIdentityUsersFactory usersFactory;
        private readonly PasswordValidator<IdentityUser> passwordValidator;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IIdentityUsersFactory usersFactory)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.usersFactory = usersFactory;
            this.passwordValidator = new PasswordValidator<IdentityUser>();
        }

        [Route("Login/")]
        [AllowAnonymous]
        public ViewResult Login()
        {
            return this.View(new LoginViewModel());
        }

        [HttpPost]
        [Route("Login/")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (this.ModelState.IsValid)
            {
                IdentityUser user = await this.userManager.FindByNameAsync(loginViewModel.Name);

                if (user != null)
                {
                    await this.signInManager.SignOutAsync();

                    var signResult = await this.signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

                    if (signResult.Succeeded)
                    {
                        return this.RedirectToAction("Lists", "TodoLists");
                    }
                }

                this.ModelState.AddModelError(string.Empty, "Invalid name or password.");
            }

            return this.View(loginViewModel);
        }

        [Route("Register/")]
        [AllowAnonymous]
        public ViewResult Register()
        {
            return this.View("Register", new RegisterViewModel());
        }

        [HttpPost]
        [Route("Register/")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (this.ModelState.IsValid 
                && registerViewModel.Password == registerViewModel.PasswordConfirmation)
            {
                await this.signInManager.SignOutAsync();

                IdentityUser existingUser = await this.userManager.FindByNameAsync(registerViewModel.Name);

                if (existingUser != null)
                {
                    this.ModelState.AddModelError(string.Empty, "Such name already taken.");
                    return this.View("Register");
                }

                IdentityUser createdUser = usersFactory.CreateUserFromView(registerViewModel);

                var result = await passwordValidator.ValidateAsync(userManager, createdUser, registerViewModel.Password);

                if (!result.Succeeded)
                {
                    this.ModelState.AddModelError(string.Empty, "Not enough password complexity.");
                    return this.View("Register");
                }

                await userManager.CreateAsync(createdUser, registerViewModel.Password);

                return this.View("Login");
            }

            return this.View(registerViewModel);
        }

        [HttpGet]
        [Route("Logout/")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return this.RedirectToAction("Login");
        }
    }
}
