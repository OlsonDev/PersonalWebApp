using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalWebApp.Models.Entity {
	[Table("Entry", Schema = "blog")]
	public class BlogEntry {
		[Key]
		public Guid EntryId { get; set; }

		public DateTimeOffset DateCreated { get; set; }
		public DateTimeOffset DateLastModified { get; set; }
		public DateTimeOffset? DatePublished { get; set; }

		[Required, MaxLength(160)]
		public string Title { get; set; }

		[Required, MaxLength(160)]
		public string Slug { get; set; }

		public string MarkdownContent { get; set; }
		public string HtmlContent { get; set; }

		public ICollection<BlogComment> Comments { get; set; }
	}
}