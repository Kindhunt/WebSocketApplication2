using Microsoft.AspNetCore.Mvc;
using TestWebSocketApplication2.Models;

namespace TestWebSocketApplication2.Controllers
{
    public class Auth : Controller
    { 
        private readonly ApplicationDbContext _dbContext;
		private User _user;
        public Auth(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public IActionResult Index() {
			if (_user == null) {
				return View("Login");	
			}

			if (!_user.IsAuth) {
				return View("Login");
			}

			return View();
        }

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login([FromForm] string email, [FromForm] string password)
		{
			_user = _dbContext.Users.SingleOrDefault(a => a.Email == email && a.Password == password);
			
			if (_user != null) {
				if (_user.IsAuth) {
					return RedirectToAction("Index", "Auth");
				}
			}

			return View();
		}
	}
}
