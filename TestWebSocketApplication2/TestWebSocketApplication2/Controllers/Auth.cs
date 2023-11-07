using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using TestWebSocketApplication2.Models;
using System;

namespace TestWebSocketApplication2.Controllers
{
    public class Auth : Controller
    { 
        private readonly ApplicationDbContext _dbContext;
        public Auth(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public IActionResult Index()
		{
			var claim = User.FindFirst(ClaimTypes.NameIdentifier);
			Guid guid = new Guid();

			if (claim != null) {
				guid = new Guid(claim.Value);
			}
			else {
				return RedirectToAction("Login");
			}
			var user = _dbContext.Users.Find(guid);
			
			if (user == null) {
				return RedirectToAction("Login");	
			}

			if (!user.IsAuth) {
				return RedirectToAction("Login");
			}

			return View();
        }

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
		{
			var user = _dbContext.Users.SingleOrDefault(a => a.Email == email && a.Password == password);
			
			if (user != null)
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.Email),
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
				};

				var claimsIdentity = new ClaimsIdentity(
					claims, CookieAuthenticationDefaults.AuthenticationScheme);

				await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					new ClaimsPrincipal(claimsIdentity));

				ViewBag.User = JsonConvert.SerializeObject(user);

				return RedirectToAction("AuthingWebSocket", "Auth");
			}

			return View();
		}

		public IActionResult AuthingWebSocket()
		{
			var user = _dbContext.Users.Find(new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));

			if (!user.IsAuth) {
				return View();
			}
			return RedirectToAction("Index", "Auth");
		}
	}
}
