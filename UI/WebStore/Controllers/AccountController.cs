using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
        private readonly ILogger<AccountController> _Logger;

        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager, ILogger<AccountController> Logger)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
            _Logger = Logger;
        }

        public async Task<IActionResult> IsNameFree(string UserName)
        {
            var user = await _UserManager.FindByNameAsync(UserName);
            if(user != null)
            {
                return Json("Пользователь с таким именем уже существует");
            }
            return Json("true");
        }

        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                UserName = model.UserName
            };

            _Logger.LogInformation("Регистрация нового пользователя {0}", model.UserName);

            var registration_result = await _UserManager.CreateAsync(user, model.Password);

            if (registration_result.Succeeded)
            {
                await _UserManager.AddToRoleAsync(user, Role.User);
                _Logger.LogInformation("Пользователь {0} успешно зарегестрирован", model.UserName);
                await _SignInManager.SignInAsync(user, false);
                _Logger.LogInformation("Пользователь {0} вошел в систему", model.UserName);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in registration_result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            _Logger.LogWarning("Ошибка при регистрации нового пользователя {0} : {1}", model.UserName, string.Join(", ", registration_result.Errors.Select(e => e.Description)));
            return View(model);
        }

        public IActionResult Login(string ReturnUrl) => View(new LoginViewModel { ReturnUrl = ReturnUrl});

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var login_result = await _SignInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                false);

            if (login_result.Succeeded)
            {
                _Logger.LogInformation("Пользователь {0} вошел в систему", model.UserName);

                if (Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Неверное имя пользователя или пароль");
            _Logger.LogWarning("Ошибка при входе пользователя {0} в систему", model.UserName);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var user_name = User.Identity.Name;

            await _SignInManager.SignOutAsync();
            _Logger.LogInformation("Пользователь {0} вышел из системы", user_name);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenided() => View();
    }
}