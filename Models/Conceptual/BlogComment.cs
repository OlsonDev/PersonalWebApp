using System;

namespace PersonalWebApp.Models.Conceptual {
	public class BlogComment {
		public Guid CommentId { get; set; }
		public Guid EntryId { get; set; }

		public DateTimeOffset DateSubmitted { get; set; }
		public DateTimeOffset? DateReviewed { get; set; }
		public bool IsApproved { get; set; }
		
		public string AuthorName { get; set; }
		public string AuthorEmail { get; set; }
		public string AuthorWebsite { get; set; }

		public string MarkdownContent { get; set; }
		public string HtmlContent { get; set; }
	}
}