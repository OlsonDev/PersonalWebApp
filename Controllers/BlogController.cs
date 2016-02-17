using Microsoft.AspNet.Mvc;

namespace PersonalWebApp.Controllers {
	public class BlogController : Controller {
		public IActionResult Index() {
			ViewData["Title"] = "My blog";
			return View();
		}
	}
}