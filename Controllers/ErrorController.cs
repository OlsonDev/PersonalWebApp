using Microsoft.AspNet.Mvc;

namespace PersonalWebApp.Controllers {
	public class ErrorController : Controller {
		public IActionResult Index() {
			ViewData["Title"] = "Error";
			return View();
		}
	}
}