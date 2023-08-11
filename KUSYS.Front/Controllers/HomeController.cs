using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KUSYS.Front.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="admin")]
        public string AdminPage()
        {

            return "AdminPage";
        }
        [Authorize(Roles = "student")]
        public string Student()
        {
            return "sStudent Page";
        }

    }
}
