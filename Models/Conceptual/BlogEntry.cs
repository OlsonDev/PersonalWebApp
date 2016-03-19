using System;
using System.Collections.Generic;

namespace PersonalWebApp.Models.Conceptual {
	public class BlogEntry {
		public Guid EntryId { get; set; }
		public DateTimeOffset DateCreated { get; set; }
		public DateTimeOffset DateLastModified { get; set; }
		public DateTimeOffset? DatePublished { get; set; }
		public string Title { get; set; }
		public string Slug { get; set; }
		public string MarkdownContent { get; set; }
		public string HtmlContent { get; set; }
		public List<BlogComment> Comments { get; set; }
	}
}