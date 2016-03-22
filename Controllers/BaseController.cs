using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using PersonalWebApp.Models;
using PersonalWebApp.Utility;

namespace PersonalWebApp.Controllers {
	public abstract class BaseController : Controller {
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