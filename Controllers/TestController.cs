using Microsoft.AspNetCore.Mvc;
using MvcAuth.Repositories;

namespace MvcAuth.Controllers
{
    public class TestController : Controller
    {
        private readonly UserRepository _userRepository;

        public TestController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userRepository.GetByEmail("test@gmail.com");

            if (user == null)
            {
                return Content("User not found");
            }

            return Content($"User: {user.Name}, Email: {user.Email}");
        }
    }
}