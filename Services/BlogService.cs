using System;
using System.Collections.Generic;
using System.Linq;
using PersonalWebApp.Models.Db;
using Conceptual = PersonalWebApp.Models.Conceptual;

namespace PersonalWebApp.Services {
	public class BlogService : BaseService {
		private readonly ApplicationDbContext _dbContext;

		public BlogService(ApplicationDbContext dbContext) {
			_dbContext = dbContext;
		}

		public Conceptual.BlogEntry GetById(string blogEntryId) => GetById(Guid.Parse(blogEntryId));

		public Conceptual.BlogEntry GetById(Guid blogEntryId) {
			var results =
				from be in _dbContext.BlogEntries
				join bc in _dbContext.BlogComments on be.EntryId equals bc.EntryId
				where be.EntryId == blogEntryId
				select new {
					be.EntryId,
					be.DatePublished,
					be.DateCreated,
					be.DateLastModified,
					be.Title,
					be.Slug,
					EntryMarkdownContent = be.MarkdownContent,
					EntryHtmlContent = be.HtmlContent,

					bc.CommentId,
					bc.DateSubmitted,
					bc.DateReviewed,
					bc.IsApproved,
					bc.AuthorName,
					bc.AuthorEmail,
					bc.AuthorWebsite,
					CommentMarkdownContent = bc.MarkdownContent,
					CommentHtmlContent = bc.HtmlContent,

				}
			;

			var iter = results.GetEnumerator();
			if (!iter.MoveNext()) {
				throw new InvalidOperationException($"BlogEntry with BlogEntryId {blogEntryId} does not exist");
			}

			var first = iter.Current;
			var entry = new Conceptual.BlogEntry {
				EntryId = first.EntryId,
				DatePublished = first.DatePublished,
				DateCreated = first.DateCreated,
				DateLastModified = first.DateLastModified,
				Title = first.Title,
				Slug = first.Slug,
				MarkdownContent = first.EntryMarkdownContent,
				HtmlContent = first.CommentMarkdownContent,

				Comments = new List<Conceptual.BlogComment> {
					new Conceptual.BlogComment() {
						CommentId = first.CommentId,
						EntryId = first.EntryId,
						DateSubmitted = first.DateSubmitted,
						DateReviewed = first.DateReviewed,
						IsApproved = first.IsApproved,
						AuthorName = first.AuthorName,
						AuthorEmail = first.AuthorEmail,
						AuthorWebsite = first.AuthorWebsite,
						MarkdownContent = first.CommentMarkdownContent,
						HtmlContent = first.CommentHtmlContent
					}
				}
			};

			while (iter.MoveNext()) {
				var cur = iter.Current;
				entry.Comments.Add(new Conceptual.BlogComment {
					CommentId = cur.CommentId,
					EntryId = cur.EntryId,
					DateSubmitted = cur.DateSubmitted,
					DateReviewed = cur.DateReviewed,
					IsApproved = cur.IsApproved,
					AuthorName = cur.AuthorName,
					AuthorEmail = cur.AuthorEmail,
					AuthorWebsite = cur.AuthorWebsite,
					MarkdownContent = cur.CommentMarkdownContent,
					HtmlContent = cur.CommentHtmlContent
				});
			}

			return entry;
		}

		public IEnumerable<Conceptual.BlogEntry> GetAll() {
			var results =
				from be in _dbContext.BlogEntries
				join bc in _dbContext.BlogComments on be.EntryId equals bc.EntryId
				orderby be.DatePublished descending, bc.DateSubmitted descending
				select new {
					be.EntryId,
					be.DatePublished,
					be.DateCreated,
					be.DateLastModified,
					be.Title,
					be.Slug,
					EntryMarkdownContent = be.MarkdownContent,
					EntryHtmlContent = be.HtmlContent,

					bc.CommentId,
					bc.DateSubmitted,
					bc.DateReviewed,
					bc.IsApproved,
					bc.AuthorName,
					bc.AuthorEmail,
					bc.AuthorWebsite,
					CommentMarkdownContent = bc.MarkdownContent,
					CommentHtmlContent = bc.HtmlContent,
				}
			;

			using (var iter = results.GetEnumerator()) {
				if (!iter.MoveNext()) yield break;

				var cur = iter.Current;
				var entry = new Conceptual.BlogEntry {
					EntryId = cur.EntryId,
					DatePublished = cur.DatePublished,
					DateCreated = cur.DateCreated,
					DateLastModified = cur.DateLastModified,
					Title = cur.Title,
					Slug = cur.Slug,
					MarkdownContent = cur.EntryMarkdownContent,
					HtmlContent = cur.CommentMarkdownContent,

					Comments = new List<Conceptual.BlogComment> {
						new Conceptual.BlogComment {
							CommentId = cur.CommentId,
							EntryId = cur.EntryId,
							DateSubmitted = cur.DateSubmitted,
							DateReviewed = cur.DateReviewed,
							IsApproved = cur.IsApproved,
							AuthorName = cur.AuthorName,
							AuthorEmail = cur.AuthorEmail,
							AuthorWebsite = cur.AuthorWebsite,
							MarkdownContent = cur.CommentMarkdownContent,
							HtmlContent = cur.CommentHtmlContent
						}
					}
				};

				while (iter.MoveNext()) {
					cur = iter.Current;
					if (cur.EntryId != entry.EntryId) {
						yield return entry;
						entry = new Conceptual.BlogEntry {
							EntryId = cur.EntryId,
							DatePublished = cur.DatePublished,
							DateCreated = cur.DateCreated,
							DateLastModified = cur.DateLastModified,
							Title = cur.Title,
							Slug = cur.Slug,
							MarkdownContent = cur.EntryMarkdownContent,
							HtmlContent = cur.CommentMarkdownContent,

							Comments = new List<Conceptual.BlogComment>()
						};
					}

					entry.Comments.Add(new Conceptual.BlogComment {
						CommentId = cur.CommentId,
						EntryId = cur.EntryId,
						DateSubmitted = cur.DateSubmitted,
						DateReviewed = cur.DateReviewed,
						IsApproved = cur.IsApproved,
						AuthorName = cur.AuthorName,
						AuthorEmail = cur.AuthorEmail,
						AuthorWebsite = cur.AuthorWebsite,
						MarkdownContent = cur.CommentMarkdownContent,
						HtmlContent = cur.CommentHtmlContent
					});
				}
				yield return entry;
			}
		}
	}
}