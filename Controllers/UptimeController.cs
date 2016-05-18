using System;
using Microsoft.AspNetCore.Mvc;

namespace PersonalWebApp.Controllers {
	public class UptimeController : BaseController {
		public IActionResult Ping() {
			return Content(DateTime.Now.ToString("o"));
		}
	}
}