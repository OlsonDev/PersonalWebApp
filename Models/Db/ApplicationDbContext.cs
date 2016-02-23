using System.Linq;
using System.Reflection;
using Microsoft.Data.Entity;
using PersonalWebApp.Models.Attributes;
using PersonalWebApp.Models.Entity;

namespace PersonalWebApp.Models.Db {
	public class ApplicationDbContext : DbContext {
		public DbSet<Skill> Skills { get; set; }
		public DbSet<SkillTag> SkillTags { get; set; }
		public DbSet<SkillToTag> SkillTaggings { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			BuildModelSchemaNames(modelBuilder);

			modelBuilder.Entity<Skill>()
				.HasIndex(e => new { e.Code })
				.IsUnique()
			;

			modelBuilder.Entity<SkillTag>()
				.HasIndex(e => new { e.Code })
				.IsUnique()
			;

			modelBuilder.Entity<SkillToTag>()
				.HasKey(e => new { e.SkillId, e.TagId })
			;

			base.OnModelCreating(modelBuilder);
		}

		private static void BuildModelSchemaNames(ModelBuilder modelBuilder) {
			var asm = typeof(ApplicationDbContext).GetTypeInfo().Assembly;
			var typesWithSchemas = asm
				.GetTypes()
				.Select(t => new { Type = t, Attr = t.GetTypeInfo().GetCustomAttribute<SchemaNameAttribute>() })
				.Where(o => o.Attr != null)
			;
			foreach (var typeAndAttr in typesWithSchemas) {
				modelBuilder.Entity(typeAndAttr.Type).ToTable(typeAndAttr.Type.Name, typeAndAttr.Attr.SchemaName);
			}
		}
	}
}