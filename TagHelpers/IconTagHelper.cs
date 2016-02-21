using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.AspNet.Razor.TagHelpers;
using Microsoft.AspNet.Server.Kestrel;
using Microsoft.Extensions.PlatformAbstractions;

namespace PersonalWebApp.TagHelpers {
	[HtmlTargetElement("icon", TagStructure = TagStructure.WithoutEndTag, Attributes = "name")]
	public class IconTagHelper : TagHelper {
		private readonly IApplicationEnvironment _appEnvironment;

		public string Name { get; set; } = "alert";
		public string Size { get; set; } = "24";
		public string Fill { get; set; } = "fff";

		public IconTagHelper(IApplicationEnvironment appEnvironment) {
			_appEnvironment = appEnvironment;
		}

		public override void Process(TagHelperContext context, TagHelperOutput output) {
			output.TagName = "svg";
			output.TagMode = TagMode.StartTagAndEndTag;
			output.Attributes["width"] = Size;
			output.Attributes["height"] = Size;
			output.Attributes["viewBox"] = "0 0 24 24";
			var classPostfix = (" " + output.Attributes["class"]?.Value).TrimEnd();
			output.Attributes["class"] = "icon-" + Name.Replace(";", " icon-") + classPostfix;

			var names = Name.Split(';');
			var fills = Fill.Split(';');

			for (var i = 0; i < names.Length; i++) {
				var name = names[i];
				var fill = i < fills.Length ? fills[i] : fills[fills.Length - 1];
				output.Content.AppendHtml(GetPaths(name, fill.Split(',')));
			}
		}

		private string GetPaths(string name, string[] fills) {
			XDocument doc;
			try {
				doc = GetSvgDocument(name);
			} catch (FileNotFoundException) {
				try {
					doc = GetSvgDocument("alert");
					fills = new [] { "#fc0" };
				} catch (FileNotFoundException) {
					return $"<!-- File not found: {name} -->";
				}
			}
			
			var builder = new StringBuilder();
			var paths = doc.Descendants(doc.Root.Name.Namespace + "path");
			var i = 0;
			foreach (var path in paths) {
				var fill = fills[Math.Min(i, fills.Length - 1)];
				path.SetAttributeValue("fill", Regex.Replace(fill, "^#?", "#"));
				builder.Append(path.ToString(SaveOptions.DisableFormatting));
				i++;
			}
			return builder.ToString();
		}

		private XDocument GetSvgDocument(string name) {
			var basePath = _appEnvironment.ApplicationBasePath;
			return XDocument.Load(Path.Combine(basePath, $"Client/svg/{name}.svg"));
		}
	}
}
