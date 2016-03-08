using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PersonalWebApp.Models.Attributes;

namespace PersonalWebApp.Models.Entity {
	[SchemaName("skill")]
	public class Skill {
		public Guid SkillId { get; set; }

		[Required, MaxLength(100)]
		public string Name { get; set; }

		[Required, MaxLength(100), RegularExpression("[a-z][a-z0-9-]*", ErrorMessage = "Code must be a lowercase slug.")]
		public string Code { get; set; }

		[Required, MaxLength(100), RegularExpression("[a-z][a-z0-9-]*", ErrorMessage = "IconClass must be a lowercase slug.")]
		public string IconClass { get; set; }

		[Required, Range(1, 5)]
		public int Rating { get; set; }

		public ICollection<SkillToTag> Taggings { get; set; }
		
	}
}