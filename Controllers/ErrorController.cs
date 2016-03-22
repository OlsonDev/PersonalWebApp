using Microsoft.AspNet.Mvc;

namespace PersonalWebApp.Controllers {
	public class ErrorController : BaseController {
		public IActionResult Index() {
			ViewData["Title"] = "Error";
			return View();
		}
	}
}