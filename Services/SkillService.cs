using System;
using System.Collections.Generic;
using System.Linq;
using PersonalWebApp.Models.Db;
using PersonalWebApp.Models.Entity;

namespace PersonalWebApp.Services {
	public class SkillService : BaseService {
		private readonly ApplicationDbContext _dbContext;

		public SkillService(ApplicationDbContext dbContext) {
			_dbContext = dbContext;
		}

		public Skill GetById(string skillId) => GetById(Guid.Parse(skillId));

		public Skill GetById(Guid skillId) {
			var skill = _dbContext.Skills.FirstOrDefault(s => s.SkillId == skillId);
			if (skill == null) {
				throw new InvalidOperationException($"Skill with SkillId {skillId} does not exist.");
			}
			return skill;
		}

		public IEnumerable<Skill> GetAll() => _dbContext.Skills.ToList();
	}
}