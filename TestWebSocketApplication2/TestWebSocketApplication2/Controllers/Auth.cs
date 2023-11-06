using Microsoft.AspNetCore.Mvc;
using TestWebSocketApplication2.Models;

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
            return View();
        }

        public IActionResult Login() {
			//var userFromDb = _dbContext.Users.Where(a => a.Email == user.Email && a.Password == user.Password).FirstOrDefault();
			//if (userFromDb != null)
			//{
			//    if(!userFromDb.IsAuth)
			//    {
			//        return View();
			//    }
			//    else return BadRequest();
			//}
			//else return BadRequest();
			return View();
		}
    }
}
