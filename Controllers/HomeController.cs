using Microsoft.AspNet.Mvc;

namespace PersonalWebApp.Controllers {
	public class HomeController : Controller {
		public IActionResult Index() {
			ViewData["Title"] = "Home";
			return View();
		}
	}
}