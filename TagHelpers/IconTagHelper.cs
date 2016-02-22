using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

		public IconTagHelper(IApplicationEnvironment appEnvironment) {
			_appEnvironment = appEnvironment;
		}

		public override void Process(TagHelperContext context, TagHelperOutput output) {
			output.TagName = "svg";
			output.TagMode = TagMode.StartTagAndEndTag;
			output.Attributes["width"] = Size;
			output.Attributes["height"] = Size;
			output.Attributes["viewBox"] = output.Attributes["viewBox"]?.Value?.ToString() ?? "0 0 24 24";
			var classPostfix = (" " + output.Attributes["class"]?.Value).TrimEnd();
			output.Attributes["class"] = "icon-" + Name.Replace(";", " icon-") + classPostfix;

			var names = Name.Split(';');
			foreach (var name in names) {
				output.Content.AppendHtml(GetSvgContent(name));
			}
		}

		private string GetSvgContent(string name) {
			XDocument doc;
			try {
				doc = GetSvgDocument(name);
			} catch (FileNotFoundException) {
				try {
					doc = GetSvgDocument("alert");
				} catch (FileNotFoundException) {
					return $"<!-- File not found: {name} -->";
				}
			}
			using (var reader = doc.CreateReader()) {
				reader.MoveToContent();
				return reader.ReadInnerXml();
			}
		}

		private XDocument GetSvgDocument(string name) {
			var basePath = _appEnvironment.ApplicationBasePath;
			return XDocument.Load(Path.Combine(basePath, $"Client/svg/{name}.svg"));
		}
	}
}
