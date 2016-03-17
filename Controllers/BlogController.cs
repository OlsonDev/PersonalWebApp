using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace PersonalWebApp.Controllers {
	public class BlogController : Controller {
		private readonly IConfiguration _configuration;

		public BlogController(IConfiguration configuration) {
			_configuration = configuration;
		}
		public IActionResult Index() {
			ViewData["Title"] = "My blog";
			const string md = @"
# H1 Header
## H2 Header
### H3 Header
#### H4 Header
##### H5 Header
###### H6 Header

This is a paragraph. Check out [this link](https://www.google.com/ 'Google').
Here's [another one][1].\
This is a _separate_ line within the **same** paragraph.

This is a **new** paragraph. Tacos <del>aren't</del> <ins>are</ins> delicious.

> This is a multiline
> quote.
>
> Check out this `inline code block` and this fenced code block:
>
> ```js
> function noop() { return; }
> ```
> > Check out this cool nested quote.
> >
> > Check out this `inline code block` and this fenced code block:
> > ```js
> > function noop() { return; }
> > ```

Check out this `inline code` here.

- one
- two
- three
    - three dot one
    - three dot two
        - three dot two dot one
        - three dot two dot two
- four
* new list item 1
* new list item 2

-----------

Cool thematic break (horizontal rule), no?

1. pirate
1. ninja
1. zombie
    1. three dot one
    1. three dot two
        1. three dot two dot one
        1. three dot two dot two

This list should start at 9:
9. taco
1. pizza
1. beefcake
1. what

```csharp
public class BlogController : Controller {
    public IActionResultIndex() {
        ViewData[""Title""] = ""My blog inception"";
        return View();
    }
}
```

[1]: https://www.reddit.com/ 'Reddit'
";
			var model = CommonMark.CommonMarkConverter.Convert(md);
			return View(model: model);
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