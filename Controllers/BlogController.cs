using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PersonalWebApp.Services;

namespace PersonalWebApp.Controllers {
	public class BlogController : BaseController {
		private readonly IConfiguration _configuration;
		private readonly BlogService _blogService;

		public BlogController(IConfiguration configuration, BlogService blogService) {
			_configuration = configuration;
			_blogService = blogService;
		}
		public IActionResult Index() {
			ViewData["Title"] = "My blog";
			return View(_blogService.GetAll());
		}

		public IActionResult Admin() {
			ViewData["Title"] = "My blog admin";
			return View();
		}

		// ReSharper disable once InconsistentNaming
		public async Task<IActionResult> Auth(string id_token) {
			var pairs = new Dictionary<string, string> { ["id_token"] = id_token };
			var httpContent = new FormUrlEncodedContent(pairs);
			using (var client = new HttpClient()) {
				client.BaseAddress = new Uri("https://www.googleapis.com/");
				var response = await client.PostAsync("oauth2/v3/tokeninfo", httpContent);
				if (!response.IsSuccessStatusCode) return NoInfo();
				var apiResponse = await response.Content.ReadAsStringAsync();
				dynamic apiResponseObject = JObject.Parse(apiResponse);

				var googleClientId = _configuration["Data:GoogleClientId"];
				var responseEmail = apiResponseObject.email.ToString().ToLower();
				var validEmail = _configuration["Data:Email"].ToLower();
				
				return apiResponseObject.aud == googleClientId && responseEmail == validEmail
					? Json(new {
						error = false,
						apiResponseObject.email,
						apiResponseObject.picture,
						apiResponseObject.name
					})
					: NoInfo()
				;
			}
		}

		private IActionResult NoInfo() {
			return Json(new { error = true });
		}
	}
}