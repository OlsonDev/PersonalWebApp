using Microsoft.AspNet.Mvc;

namespace PersonalWebApp.Controllers {
	public class ContactController : Controller {
		public IActionResult Index() {
			ViewData["Title"] = "Contact me";
			return View();
		}
	}
}