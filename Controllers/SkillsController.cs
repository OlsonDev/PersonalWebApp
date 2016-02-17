using Microsoft.AspNet.Mvc;

namespace PersonalWebApp.Controllers {
	public class SkillsController : Controller {
		public IActionResult Index() {
			ViewData["Title"] = "My skills";
			return View();
		}
	}
}