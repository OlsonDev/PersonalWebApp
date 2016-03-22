using Microsoft.AspNet.Mvc;

namespace PersonalWebApp.Controllers {
	public class HomeController : BaseController {
		public IActionResult Index() {
			ViewData["Title"] = "Home";
			return View();
		}
	}
}