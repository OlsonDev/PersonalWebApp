using Microsoft.AspNet.Builder;

namespace PersonalWebApp.Middleware {
	public static class MiddlewareExtensions {
		public static IApplicationBuilder UseStripWhitespace(this IApplicationBuilder builder) {
			return builder.UseMiddleware<StripWhitespaceMiddleware>();
		}
	}
}