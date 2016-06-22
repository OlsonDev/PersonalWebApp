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
			ViewData["IsAuthed"] = IsAuthed();
			return View(_blogService.GetAll());
		}

		public IActionResult New() {
			var x = RouteData.Values;
			ViewData["Title"] = "New blog";
			ViewData["IsAuthed"] = IsAuthed();
			var model = new BlogEntry();
			return View("Edit", model);
		}

		[HttpGet("/blog/{year}/{month}/{day}/{slug}/edit")]
		public IActionResult Edit(int year, int month, int day, string slug) {
			ViewData["Title"] = "Edit blog";
			ViewData["IsAuthed"] = IsAuthed();

			var date = new DateTime(year, month, day);
			var entries = _blogService.GetAll(date, slug).ToList();
			if (entries.FirstOrDefault()?.Slug == slug || entries.Count == 1) {
				var model = entries.First();
				ViewData["Title"] = $"Editing blog: {model.Title}";
				return View(model);
			} else {
				ViewData["Title"] = $"Cannot edit blog; {entries.Count} entries for {date.ToString("MMMM d, yyyy")}";
				ViewData["RenderBlogContent"] = false;
				return View("Index", entries);
			}
		}

		public IActionResult Preview(string title, string slug, string markdownContent) {
			return ApiResponse(() => {
				if (!IsAuthed()) return NotAuthedApiResponse();
				ViewData["IsAuthed"] = true;
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

		public IActionResult Save(Guid entryId, string title, string slug, string markdownContent) {
			return ApiResponse(() => {
				if (!IsAuthed()) return NotAuthedApiResponse();

				var htmlContent = CommonMark.CommonMarkConverter.Convert(markdownContent);
				var now = DateTimeOffset.Now;

				BlogEntry model;
				if (entryId == Guid.Empty) {
					model = new BlogEntry {
						MarkdownContent = markdownContent,
						HtmlContent = htmlContent,
						Title = title,
						Slug = slug,
						DateCreated = now,
						DateLastModified = now,
						DatePublished = now
					};
				} else {
					// TODO: Handle lookup failure
					model = _blogService.GetById(entryId);
					model.MarkdownContent = markdownContent;
					model.HtmlContent = htmlContent;
					model.Title = title;
					model.Slug = slug;
					model.DateLastModified = now;
				}

				_blogService.SaveBlogEntry(model);

				return new { RedirectUrl = model.Url };
			});
		}

		[HttpGet("/blog/{year}/{month}/{day}/{slug?}")]
		public IActionResult Single(int year, int month, int day, string slug) {
			ViewData["IsAuthed"] = IsAuthed();

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