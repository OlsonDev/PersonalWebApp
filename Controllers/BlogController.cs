using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PersonalWebApp.Models;
using PersonalWebApp.Models.Conceptual;
using PersonalWebApp.Services;
using System.Linq;
using Microsoft.AspNetCore.Http;

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

		public IActionResult Preview(string title, string slug, string markdownContent) {
			return ApiResponse(() => {
				if (!IsAuthed()) return NotAuthedApiResponse();
				var now = DateTimeOffset.Now;
				var model = new BlogEntry {
					MarkdownContent = markdownContent,
					HtmlContent = CommonMark.CommonMarkConverter.Convert(markdownContent),
					Title = title,
					Slug = slug,
					DateCreated = now,
					DateLastModified = now,
					DatePublished = now
				};
				return RenderPartialViewToString("_BlogEntry", model);
			});
		}

		public IActionResult Save(string title, string slug, string markdownContent) {
			return ApiResponse(() => {
				if (!IsAuthed()) return NotAuthedApiResponse();
				var now = DateTimeOffset.Now;
				var model = new BlogEntry {
					MarkdownContent = markdownContent,
					HtmlContent = CommonMark.CommonMarkConverter.Convert(markdownContent),
					Title = title,
					Slug = slug,
					DateCreated = now,
					DateLastModified = now,
					DatePublished = now
				};

				_blogService.SaveBlogEntry(model);

				return new { RedirectUrl = model.Url };
			});
		}

		[HttpGet("/blog/{year}/{month}/{day}/{slug?}")]
		public IActionResult Single(int year, int month, int day, string slug) {
			var date = new DateTime(year, month, day);
			var model = _blogService.GetAll(date, slug).ToList();
			if (model.FirstOrDefault()?.Slug == slug) {
				ViewData["Title"] = $"{model.First().Title} | My blog";
				model = model.Take(1).ToList();
			} else if (model.Count == 1) {
				ViewData["Title"] = $"1 entry for {date.ToString("MMMM d, yyyy")} | My blog";
			} else {
				ViewData["Title"] = $"{model.Count} entries for {date.ToString("MMMM d, yyyy")} | My blog";
			}

			return View("Index", model);
		}

		private static InvalidApiResponse NotAuthedApiResponse() {
			return new InvalidApiResponse("Not authenticated and/or authorized.");
		}

		private bool IsAuthed() {
			return HttpContext.Session.GetInt32("isAuthed") == 1;
		}

		// ReSharper disable once InconsistentNaming
		public async Task<IActionResult> Auth(string id_token) {
			return await ApiResponse(async () => {
				var pairs = new Dictionary<string, string> { ["id_token"] = id_token };
				var httpContent = new FormUrlEncodedContent(pairs);
				using (var client = new HttpClient()) {
					client.BaseAddress = new Uri("https://www.googleapis.com/");
					var response = await client.PostAsync("oauth2/v3/tokeninfo", httpContent);
					if (!response.IsSuccessStatusCode) return new InvalidApiResponse();
					var apiResponse = await response.Content.ReadAsStringAsync();
					dynamic apiResponseObject = JObject.Parse(apiResponse);

					var googleClientId = _configuration["Data:GoogleClientId"];
					var responseEmail = apiResponseObject.email.ToString().ToLower();
					var validEmail = _configuration["Data:Email"].ToLower();
					var isValid = apiResponseObject.aud == googleClientId && responseEmail == validEmail;
					if (!isValid) return new InvalidApiResponse();

					// Sure would like a Set<bool>() method...
					// See https://github.com/aspnet/Session/issues/3
					HttpContext.Session.SetInt32("isAuthed", 1);

					return new ValidApiResponse(new { apiResponseObject.email, apiResponseObject.picture, apiResponseObject.name });
				}
			});
		}
	}
}