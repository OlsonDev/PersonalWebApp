using System;
using System.Collections.Generic;
using System.Linq;
using PersonalWebApp.Models.Db;
using Conceptual = PersonalWebApp.Models.Conceptual;

namespace PersonalWebApp.Services {
	public class SkillService : BaseService {
		private readonly ApplicationDbContext _dbContext;

		public SkillService(ApplicationDbContext dbContext) {
			_dbContext = dbContext;
		}

		public Conceptual.Skill GetById(string skillId) => GetById(Guid.Parse(skillId));

		public Conceptual.Skill GetById(Guid skillId) {
			var results =
				from s in _dbContext.Skills
				join st in _dbContext.SkillTaggings on s.SkillId equals st.SkillId
				join t in _dbContext.SkillTags on st.TagId equals t.TagId
				where s.SkillId == skillId
				select new { s.SkillId, s.Code, s.Name, s.Rating, Tag = t.Code }
			;

			var iter = results.GetEnumerator();
			if (!iter.MoveNext()) {
				throw new InvalidOperationException($"Skill with SkillId {skillId} does not exist");
			}

			var first = iter.Current;
			var skill = new Conceptual.Skill {
				SkillId = first.SkillId,
				Code = first.Code,
				Name = first.Name,
				Rating = first.Rating,
				Tags = new List<string> { first.Tag }
			};

			while (iter.MoveNext()) {
				skill.Tags.Add(iter.Current.Tag);
			}

			return skill;
		}

		public IEnumerable<Conceptual.Skill> GetAll() {
			var results =
				from s in _dbContext.Skills
				join st in _dbContext.SkillTaggings on s.SkillId equals st.SkillId
				join t in _dbContext.SkillTags on st.TagId equals t.TagId
				orderby s.Name
				select new { s.SkillId, s.Code, s.Name, s.Rating, Tag = t.Code }
			;

			using (var iter = results.GetEnumerator()) {
				if (!iter.MoveNext()) yield break;

				var cur = iter.Current;
				var skill = new Conceptual.Skill {
					SkillId = cur.SkillId,
					Code = cur.Code,
					Name = cur.Name,
					Rating = cur.Rating,
					Tags = new List<string> { cur.Tag }
				};

				while (iter.MoveNext()) {
					cur = iter.Current;
					if (cur.SkillId != skill.SkillId) {
						yield return skill;
						skill = new Conceptual.Skill {
							SkillId = cur.SkillId,
							Code = cur.Code,
							Name = cur.Name,
							Rating = cur.Rating,
							Tags = new List<string>()
						};
					}
					skill.Tags.Add(cur.Tag);
				}
				yield return skill;
			}
		}

		public IEnumerable<Conceptual.SkillTag> GetAllSkillTags() => _dbContext.SkillTags.Select(
			st => new Conceptual.SkillTag {
				TagId = st.TagId,
				Code = st.Code,
				Name = st.Name
			}
		);
	}
}