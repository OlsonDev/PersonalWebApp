using Microsoft.AspNet.Mvc;
using PersonalWebApp.Models.ViewModels;
using PersonalWebApp.Services;

namespace PersonalWebApp.Controllers {
	public class SkillsController : Controller {
		private readonly SkillService _skillService;

		public SkillsController(SkillService skillService) {
			_skillService = skillService;
		}

		public IActionResult Index() {
			ViewData["Title"] = "My skills";
			var viewModel = new SkillsViewModel {
				Skills = _skillService.GetAll(),
				Tags = _skillService.GetAllSkillTags()
			};
			return View(viewModel);
		}
	}
}