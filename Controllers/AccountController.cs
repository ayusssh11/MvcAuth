using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcAuth.Models;
using MvcAuth.Repositories;

namespace MvcAuth.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository;

        public AccountController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
       {
            return View();
       }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await _userRepository.GetByEmail(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "Email",
                    "Email already exists."
                );

                return View(model);
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email
            };

            var passwordHasher = new PasswordHasher<User>();

            user.PasswordHash =
                passwordHasher.HashPassword(
                    user,
                    model.Password
                );

            await _userRepository.Create(user);

            return Content("Registration successful");
        }
        [HttpPost]
public async Task<IActionResult> Login(LoginViewModel model)
{
    if (!ModelState.IsValid)
    {
        return View(model);
    }

    var user = await _userRepository.GetByEmail(model.Email);

    if (user == null)
    {
        ModelState.AddModelError("", "Invalid email or password.");
        return View(model);
    }

    var passwordHasher = new PasswordHasher<User>();

    var result = passwordHasher.VerifyHashedPassword(
        user,
        user.PasswordHash,
        model.Password
    );

    if (result == PasswordVerificationResult.Failed)
    {
        ModelState.AddModelError("", "Invalid email or password.");
        return View(model);
    }

    return Content($"Login successful. Welcome {user.Name}");
}
    }
}