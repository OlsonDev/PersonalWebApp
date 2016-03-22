using System;
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
		/// <returns>Returns a ValidApiResponse wrapping your object or an InvalidApiResponse wrapping the thrown exception's message.</returns>
		protected IActionResult ApiResponse(Func<object> responseObjectLambda) {
			// TODO Returning pretty formatted JSON as we build; make this a global configurable setting
			var settings = new JsonSerializerSettings { Formatting = Formatting.Indented, ContractResolver = CustomContractResolver.Instance };
			try {
				return Json(new ValidApiResponse(responseObjectLambda()), settings);
			} catch (Exception ex) {
				return Json(new InvalidApiResponse(ex), settings);
			}
		}
	}
}