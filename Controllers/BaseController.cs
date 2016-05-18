using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using PersonalWebApp.Extensions;
using PersonalWebApp.Models;
using PersonalWebApp.Utility;

namespace PersonalWebApp.Controllers {
	public abstract class BaseController : Controller {
		[ActionContext]
		public ActionContext ActionContext { get; set; }

		public IServiceProvider Resolver => ActionContext?.HttpContext?.RequestServices;

		protected string RenderPartialViewToString(string viewName, object model, bool minify = true) {
			if (string.IsNullOrEmpty(viewName)) {
				viewName = ActionContext.ActionDescriptor.Name;
			}
			ViewData.Model = model;
			using (var sw = new StringWriter()) {
				var engine = Resolver.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
				Debug.Assert(engine != null, "engine != null");
				var viewResult = engine.FindView(ActionContext, viewName, false);
				var viewContext = new ViewContext(ActionContext, viewResult.View, ViewData, TempData, sw, new HtmlHelperOptions());
				var t = viewResult.View.RenderAsync(viewContext);
				t.Wait();
				var rendered = sw.GetStringBuilder().ToString();
				return minify ? rendered.NaiveHtmlMinify() : rendered;
			}
		}

		/// <summary>
		/// All API methods should return ApiResponse(lambda => object) to the client.
		/// </summary>
		/// <param name="responseObjectLambda"></param>
		/// <returns>Returns a ApiResponse wrapping your object or an ExceptionalApiResponse wrapping the thrown exception's message.</returns>
		protected IActionResult ApiResponse(Func<object> responseObjectLambda) {
			try {
				return ApiResponse(responseObjectLambda());
			} catch (Exception ex) {
				return ExceptionalApiResponse(ex);
			}
		}

		/// <summary>
		/// All async API methods should return ApiResponse(async lambda => object) to the client.
		/// </summary>
		/// <param name="responseObjectLambda"></param>
		/// <returns>Returns a ApiResponse wrapping your object or an ExceptionalApiResponse wrapping the thrown exception's message.</returns>
		protected async Task<IActionResult> ApiResponse(Func<Task<object>> responseObjectLambda) {
			try {
				return ApiResponse(await responseObjectLambda());
			} catch (Exception ex) {
				return ExceptionalApiResponse(ex);
			}
		}

		private static JsonSerializerSettings GetSettings() {
			return new JsonSerializerSettings { Formatting = Formatting.Indented, ContractResolver = CustomContractResolver.Instance };
		}

		private IActionResult ApiResponse(object result) {
			if (result is ValidApiResponse || result is InvalidApiResponse) {
				return Json(result, GetSettings());
			}
			return Json(new ValidApiResponse(result), GetSettings());
		}

		private IActionResult ExceptionalApiResponse(Exception ex) {
			return Json(new InvalidApiResponse(ex), GetSettings());
		}
	}
}