using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace PersonalWebApp.Middleware {
	public class StripWhitespaceMiddleware {
		private readonly RequestDelegate _next;

		public StripWhitespaceMiddleware(RequestDelegate next) {
			_next = next;
		}

		public async Task Invoke(HttpContext context) {
			var body = context.Response.Body;
			var intercept = new MemoryStream();
			context.Response.Body = intercept;

			await _next.Invoke(context);

			var contentType = context.Response.ContentType;
			if (contentType.StartsWith("text/html")) {
				intercept.Seek(0, SeekOrigin.Begin);
				var reader = new StreamReader(intercept);
				var responseBody = await reader.ReadToEndAsync();
				var stripped = Regex.Replace(responseBody, @">\s+<", "><");
				var bytes = Encoding.UTF8.GetBytes(stripped);
				await body.WriteAsync(bytes, 0, bytes.Length);
			} else {
				await intercept.CopyToAsync(body);
			}
		}
	}
}