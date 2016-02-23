using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalWebApp.Models.Entity {
	[Table("Tag", Schema = "skill")]
	public class SkillTag {
		[Key]
		public Guid TagId { get; set; }

		[Required, MaxLength(100)]
		public string Name { get; set; }

		[Required, MaxLength(100), RegularExpression("[a-z][a-z0-9-]*", ErrorMessage = "Code must be a lowercase slug.")]
		public string Code { get; set; }

		public ICollection<SkillToTag> Taggings { get; set; }
	}
}