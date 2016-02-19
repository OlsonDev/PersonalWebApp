using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.AspNet.Razor.TagHelpers;

namespace PersonalWebApp.TagHelpers {
	
	[HtmlTargetElement("icon", TagStructure = TagStructure.WithoutEndTag, Attributes = "name")]
	public class IconTagHelper : TagHelper {
		public string Name { get; set; } = "alert";
		public string Size { get; set; } = "24";
		public string Fill { get; set; } = "fff";

		public override void Process(TagHelperContext context, TagHelperOutput output) {
			output.TagName = "svg";
			output.TagMode = TagMode.StartTagAndEndTag;
			output.Attributes["width"] = Size;
			output.Attributes["height"] = Size;
			output.Attributes["viewBox"] = "0 0 24 24";
			var classPostfix = (" " + output.Attributes["class"]?.Value).TrimEnd();
			output.Attributes["class"] = "icon-" + Name.Replace(",", " icon-") + classPostfix;

			var names = Name.Split(',');
			var fills = Fill.Split(',');

			for (var i = 0; i < names.Length; i++) {
				var name = names[i];
				var fill = i < fills.Length ? fills[i] : fills[fills.Length - 1];
				output.Content.AppendHtml(GetPath(name, fill));
			}
		}

		private string GetPath(string name, string fill) {
			XDocument doc;
			try {
				doc = XDocument.Load($"Client/svg/{name}.svg");
			} catch (FileNotFoundException) {
				doc = XDocument.Load("Client/svg/alert.svg");
				fill = "#fc0";
			}
			var path = doc.Descendants(doc.Root.Name.Namespace + "path").First();
			path.SetAttributeValue("fill", Regex.Replace(fill, "^#?", "#"));
			return path.ToString(SaveOptions.DisableFormatting);
		}
	}
}
