using Microsoft.AspNet.Mvc;

namespace PersonalWebApp.Controllers {
	public class PortfolioController : Controller {
		public IActionResult Index() {
			ViewData["Title"] = "My portfolio";
			return View();
		}
	}
}