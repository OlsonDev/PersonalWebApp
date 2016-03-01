using System.Collections.Generic;

namespace PersonalWebApp.Models.ViewModels {
	public class SkillsViewModel {
		public IEnumerable<Conceptual.Skill> Skills { get; set; }
		public IEnumerable<Conceptual.SkillTag> Tags { get; set; }
	}
}