using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.AspNetCore.Hosting;

namespace PersonalWebApp.TagHelpers {
	[HtmlTargetElement("icon", TagStructure = TagStructure.WithoutEndTag, Attributes = "name")]
	public class IconTagHelper : TagHelper {
		private IHostingEnvironment env;

		public string Name { get; set; } = "alert";
		public string Size { get; set; } = "24";

		public IconTagHelper(IHostingEnvironment env) {
			this.env = env;
		}

		public override void Process(TagHelperContext context, TagHelperOutput output) {
			output.TagName = "svg";
			output.TagMode = TagMode.StartTagAndEndTag;
			output.Attributes.SetAttribute("width", Size);
			output.Attributes.SetAttribute("height", Size);
			output.Attributes.SetAttribute("viewBox", output.Attributes["viewBox"]?.Value?.ToString() ?? "0 0 24 24");
			var classPostfix = (" " + output.Attributes["class"]?.Value).TrimEnd();
			output.Attributes.SetAttribute("class", "icon-" + Name.Replace(";", " icon-") + classPostfix);

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
			var basePath = env.WebRootPath;
			return XDocument.Load(Path.Combine(basePath, $"svg/{name}.svg"));
		}
	}
}
