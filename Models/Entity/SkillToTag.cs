using System;
using PersonalWebApp.Models.Attributes;

namespace PersonalWebApp.Models.Entity {
	[SchemaName("skill")]
	public class SkillToTag {
		public Guid SkillId { get; set; }
		public Skill Skill { get; set; }

		public Guid TagId { get; set; }
		public SkillTag Tag { get; set; }
	}
}