﻿using System;
using System.Globalization;
using Microsoft.AspNet.Mvc;

namespace PersonalWebApp.Controllers {
	public class UptimeController : Controller {
		public IActionResult Ping() {
			return Content(DateTime.Now.ToString("o"));
		}
	}
}