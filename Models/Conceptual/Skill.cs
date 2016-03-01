using System;
using System.Collections.Generic;

namespace PersonalWebApp.Models.Conceptual {
	public class Skill {
		public Guid SkillId { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public int Rating { get; set; }
		public List<string> Tags { get; set; }
	}
}