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
    }
}