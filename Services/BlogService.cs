using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Entity;
using PersonalWebApp.Models.Db;
using Conceptual = PersonalWebApp.Models.Conceptual;
using Entity = PersonalWebApp.Models.Entity;

namespace PersonalWebApp.Services {
	public class BlogService : BaseService {
		private readonly ApplicationDbContext _dbContext;

		public BlogService(ApplicationDbContext dbContext) {
			_dbContext = dbContext;
		}

		public Conceptual.BlogEntry GetById(string blogEntryId) => GetById(Guid.Parse(blogEntryId));

		public Conceptual.BlogEntry GetById(Guid blogEntryId) {
			var entry = _dbContext.BlogEntries.Include(m => m.Comments).Where(be => be.EntryId == blogEntryId).ToList().Select(bee => new Conceptual.BlogEntry {
				EntryId = bee.EntryId,
				DatePublished = bee.DatePublished,
				DateCreated = bee.DateCreated,
				DateLastModified = bee.DateLastModified,
				Title = bee.Title,
				Slug = bee.Slug,
				MarkdownContent = bee.MarkdownContent,
				HtmlContent = bee.HtmlContent,
				Comments = bee.Comments.Select(bce => new Conceptual.BlogComment {
					CommentId = bce.CommentId,
					EntryId = bce.EntryId,
					DateSubmitted = bce.DateSubmitted,
					DateReviewed = bce.DateReviewed,
					IsApproved = bce.IsApproved,
					AuthorName = bce.AuthorName,
					AuthorEmail = bce.AuthorEmail,
					AuthorWebsite = bce.AuthorWebsite,
					MarkdownContent = bce.MarkdownContent,
					HtmlContent = bce.HtmlContent
				}).ToList()
			}).FirstOrDefault();

			if (entry == null) {
				throw new InvalidOperationException($"BlogEntry with BlogEntryId {blogEntryId} does not exist");
			}
			
			return entry;
		}

		public IEnumerable<Conceptual.BlogEntry> GetAll() {
			return _dbContext.BlogEntries.Include(m => m.Comments).ToList().Select(bee => new Conceptual.BlogEntry {
				EntryId = bee.EntryId,
				DatePublished = bee.DatePublished,
				DateCreated = bee.DateCreated,
				DateLastModified = bee.DateLastModified,
				Title = bee.Title,
				Slug = bee.Slug,
				MarkdownContent = bee.MarkdownContent,
				HtmlContent = bee.HtmlContent,
				Comments = bee.Comments.Select(bce => new Conceptual.BlogComment {
					CommentId = bce.CommentId,
					EntryId = bce.EntryId,
					DateSubmitted = bce.DateSubmitted,
					DateReviewed = bce.DateReviewed,
					IsApproved = bce.IsApproved,
					AuthorName = bce.AuthorName,
					AuthorEmail = bce.AuthorEmail,
					AuthorWebsite = bce.AuthorWebsite,
					MarkdownContent = bce.MarkdownContent,
					HtmlContent = bce.HtmlContent
				}).ToList()
			});
		}

		public Conceptual.BlogEntry SaveBlogEntry(Conceptual.BlogEntry model) {
			model.EntryId = model.EntryId == Guid.Empty
				? _dbContext.NewGuidComb()
				: model.EntryId
			;
			var entity = new Entity.BlogEntry {
				EntryId = model.EntryId,
				DatePublished = model.DatePublished,
				DateCreated = model.DateCreated,
				DateLastModified = model.DateLastModified,
				Title = model.Title,
				Slug = model.Slug,
				MarkdownContent = model.MarkdownContent,
				HtmlContent = model.HtmlContent,
			};
			_dbContext.BlogEntries.Add(entity);
			_dbContext.SaveChanges();
			return model;
		}
	}
}