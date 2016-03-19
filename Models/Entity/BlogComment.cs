using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalWebApp.Models.Entity {
	[Table("Comment", Schema = "blog")]
	public class BlogComment {
		[Key]
		public Guid CommentId { get; set; }
		public Guid EntryId { get; set; }

		public DateTimeOffset DateSubmitted { get; set; }
		public DateTimeOffset? DateReviewed { get; set; }
		public bool IsApproved { get; set; }
		
		[Required]
		public string AuthorName { get; set; }

		[Required]
		public string AuthorEmail { get; set; }

		public string AuthorWebsite { get; set; }
		public string MarkdownContent { get; set; }
		public string HtmlContent { get; set; }
	}
}