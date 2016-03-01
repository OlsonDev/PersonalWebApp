using Microsoft.AspNet.Mvc;
using PersonalWebApp.Services;

namespace PersonalWebApp.Controllers {
	public class SkillsController : Controller {
		private readonly SkillService _skillService;

		public SkillsController(SkillService skillService) {
			_skillService = skillService;
		}

		public IActionResult Index() {
			ViewData["Title"] = "My skills";
			return View(_skillService.GetAll());
		}

		public IActionResult Json() {
			return Json(_skillService.GetAll());
		}
	}
}