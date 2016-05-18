using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PersonalWebApp.Models;

namespace PersonalWebApp.Controllers {
	public class ContactController : BaseController {
		private readonly IConfiguration _configuration;

		public ContactController(IConfiguration configuration) {
			_configuration = configuration;
		}

		public IActionResult Index() {
			ViewData["Title"] = "Contact me";
			return View();
		}

		public async Task<IActionResult> Info(string gRecaptchaResponse) {
			return await ApiResponse(async () => {
				var secret = _configuration["Data:GRecaptchaSecret"];

				var pairs = new Dictionary<string, string> { ["secret"] = secret, ["response"] = gRecaptchaResponse };
				var httpContent = new FormUrlEncodedContent(pairs);
				using (var client = new HttpClient()) {
					client.BaseAddress = new Uri("https://www.google.com/");
					var response = await client.PostAsync("recaptcha/api/siteverify", httpContent);
					if (!response.IsSuccessStatusCode) return new InvalidApiResponse();

					var apiResponse = await response.Content.ReadAsStringAsync();
					dynamic apiResponseObject = JObject.Parse(apiResponse);

					var email = _configuration["Data:Email"];
					var phone = _configuration["Data:Phone"];

					return (bool)apiResponseObject.success
						? (ApiResponse)new ValidApiResponse(new { email, phone })
						: new InvalidApiResponse(new { errors = apiResponseObject["error-codes"] })
					;
				}
			});
		}
	}
}