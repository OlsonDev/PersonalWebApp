using Microsoft.AspNet.Mvc;

namespace PersonalWebApp.Controllers {
	// TODO: Canonicalize route
	[Route("résumé")]
	[Route("resume")]
	public class ResumeController : Controller {
		public IActionResult Index() {
			ViewData["Title"] = "My résumé";
			return View();
		}
	}
}