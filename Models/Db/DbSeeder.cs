using System;
using System.Collections.Generic;
using Microsoft.AspNet.Builder;
using System.Linq;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using PersonalWebApp.Extensions;
using PersonalWebApp.Models.Entity;

namespace PersonalWebApp.Models.Db {
	public static class DbSeeder {
		private static ApplicationDbContext _context;

		// implemented as an extension method on IApplicationBuilder that we can call from Startup.cs
		public static void EnsureSampleData(this IApplicationBuilder app) {
			_context = app.ApplicationServices.GetService<ApplicationDbContext>();

			_context.Database.ExecuteSqlCommand(@"
				delete from skill.SkillToTag;
				delete from skill.Tag;
				delete from skill.Skill;
			");

			if (_context.Skills.Any()) return;

			BuildSkillWithCode(5, "C#", "c-sharp", "language", "desktop", "server");
			BuildSkillWithIconClass(3, "Visual Basic .NET", "visual-basic", "language", "desktop", "server");
			BuildSkillWithIconClass(2, "Visual Basic 6", "visual-basic", "language", "desktop", "server");
			BuildSkill(3, "ASP.NET", "server", "framework");
			BuildSkillWithIconClass(4, "ASP.NET MVC", "asp-net", "server", "framework");
			BuildSkill(3, "IIS", "server");
			BuildSkillWithIconClass(4, "Entity Framework", "asp-net", "server", "framework", "database");
			BuildSkill(3, "NHibernate", "server", "framework", "database");
			BuildSkill(3, "Java", "language", "desktop", "server", "web");
			BuildSkillWithIconClass(5, "Railo Server", "railo", "framework", "server");
			BuildSkillWithIconClass(5, "Railo CFML", "railo", "language", "server");
			BuildSkillWithIconClass(4, "Adobe ColdFusion Server", "adobe-coldfusion", "framework", "server");
			BuildSkillWithIconClass(5, "Adobe CFML", "adobe-coldfusion", "language", "server");
			BuildSkillWithIconClass(4, "Apache Tomcat", "tomcat", "server");
			BuildSkill(5, "HTML5", "language", "web");
			BuildSkill(5, "Markdown", "language", "web", "desktop");
			BuildSkill(5, "CSS3", "language", "web");
			BuildSkill(5, "LESS", "language", "web");
			BuildSkillWithIconClass(4, "SCSS", "sass", "language", "web");
			BuildSkill(5, "JavaScript", "language", "web", "server", "desktop");
			BuildSkill(3, "TypeScript", "language", "web", "server", "desktop");
			BuildSkill(2, "Dart", "language", "web", "server", "desktop");
			BuildSkill(4, "lodash", "library", "web", "server");
			BuildSkill(5, "jQuery", "library", "web", "server");
			BuildSkill(5, "jQuery UI", "library", "web");
			BuildSkill(5, "jQuery Mobile", "framework", "web");
			BuildSkill(5, "KnockoutJS", "framework", "web");
			BuildSkillWithIconClass(4, "AngularJS 1.x", "angularjs", "framework", "web");
			BuildSkillWithIconClass(4, "Angular 2.x", "angularjs", "framework", "web", "server");
			BuildSkill(3, "Polymer", "framework", "web");
			BuildSkill(4, "NodeJS", "framework", "web", "server", "hardware");
			BuildSkill(4, "npm", "tool");
			BuildSkill(4, "gulp", "tool");
			BuildSkill(4, "grunt", "tool");
			BuildSkill(4, "bower", "tool");
			BuildSkill(4, "typings", "tool");
			BuildSkill(5, "svn", "tool");
			BuildSkill(4, "git", "tool");
			BuildSkill(3, "Mercurial", "tool");
			BuildSkill(4, "Electron", "framework", "desktop", "web", "server");
			BuildSkillWithIconClass(3, "ActionScript 3.0", "adobe-flash", "language", "web", "desktop");
			BuildSkill(3, "Flex Framework 4", "framework", "web", "desktop");
			BuildSkillWithIconClass(4, "Microsoft SQL Server", "sql-server", "database", "server");
			BuildSkill(3, "MySQL", "database", "language", "server");
			BuildSkill(2, "SQLite", "database", "language", "desktop", "server");
			BuildSkillWithIconClass(4, "T-SQL", "sql-server", "language");
			BuildSkillWithIconClass(2, "PL/SQL", "oracle", "language");
			BuildSkill(3, "MongoDB", "database", "server");
			BuildSkillWithCodeAndIconClass(2, "C++14", "c-plus-plus-14", "c-plus-plus", "language", "desktop", "server");
			BuildSkillWithCode(2, "C++ CLR", "c-plus-plus-clr", "language", "framework", "desktop", "server");
			BuildSkill(4, "Tessel", "hardware", "framework");
			BuildSkill(3, "Teensy", "hardware", "framework");
			BuildSkill(3, "Arduino", "hardware", "framework");
			BuildSkill(2, "Perl", "language", "server", "desktop");
			BuildSkillWithIconClass(3, "Python 3", "python", "language", "server", "desktop");
			BuildSkill(5, "Regex", "language", "server", "web", "desktop");
			BuildSkill(4, "Windows Forms", "framework", "desktop");
			BuildSkill(4, "WPF", "framework", "desktop");
			BuildSkill(4, "XAML", "language", "web", "desktop");
			BuildSkill(1, "DirectX", "library", "desktop");
			BuildSkill(3, "WebGL", "library", "web");
			BuildSkill(3, "OpenGL", "library", "desktop");
			BuildSkillWithIconClass(2, "GLSL", "opengl", "language", "desktop");
			BuildSkillWithIconClass(1, "HLSL", "directx", "language", "desktop");
			BuildSkill(3, "XNA", "library", "framework", "desktop");
			BuildSkill(3, "Unreal Engine", "framework", "web", "desktop", "app");
			BuildSkill(3, "Unity", "framework", "web", "desktop", "app");
			BuildSkill(3, "Bash", "tool", "language");
			BuildSkill(2, "cmd", "tool", "language");
			BuildSkill(4, "Eclipse", "tool", "app");
			BuildSkillWithIconClass(4, "ColdFusion Builder", "adobe-coldfusion", "tool", "app");
			BuildSkill(4, "Dreamweaver", "tool", "app");
			BuildSkillWithIconClass(4, "Flash Builder 4", "flash-builder", "tool", "app");
			BuildSkillWithIconClass(4, "Flash Professional", "adobe-flash", "tool", "app");
			BuildSkill(4, "Arduino IDE", "tool", "app");
			BuildSkill(4, "Atom", "tool", "app");
			BuildSkill(5, "Visual Studio Code", "tool", "app");
			BuildSkill(5, "Visual Studio", "tool", "app");
			BuildSkill(5, "ReSharper", "tool", "app");
			BuildSkill(5, "Sublime Text", "tool", "app");
			BuildSkill(5, "SQL Server Management Studio", "tool", "app");
			BuildSkill(2, "MySQL Workbench", "tool", "app");
			BuildSkill(2, "Android Studio", "tool", "app");
			BuildSkill(3, "Xcode", "tool", "app");
			BuildSkill(3, "Inkscape", "tool", "app");
			BuildSkill(3, "Paint.NET", "tool", "app");
			BuildSkillWithIconClass(2, "Adobe Photoshop", "photoshop", "tool", "app");
			BuildSkill(5, "GitHub Desktop", "tool", "app");
			BuildSkill(4, "SourceTree", "tool", "app");
			BuildSkill(2, "clang", "tool");
			BuildSkill(5, "Microsoft Excel", "tool", "app");
			BuildSkill(4, "Microsoft Word", "tool", "app");
			BuildSkill(4, "Microsoft Outlook", "tool", "app");

			_context.SaveChanges();
		}

		private static readonly Dictionary<string, SkillTag> Tags = new Dictionary<string, SkillTag>();

		private static SkillTag BuildOrGetTag(string code) {
			SkillTag tag;
			if (Tags.TryGetValue(code, out tag)) return tag;
			var tagId = Guid.NewGuid(); // TODO: Get Comb from library or find my old implementation
			tag = new SkillTag {
				TagId = tagId,
				Name = code.Capitalize(),
				Code = code
			};
			_context.SkillTags.Add(tag);
			return Tags[code] = tag;
		}

		// ReSharper disable UnusedMethodReturnValue.Local
		private static Skill BuildSkill(int rating, string name, params string[] tagCodes) {
			var slug = name.Slugify();
			return BuildSkillWithCodeAndIconClass(rating, name, slug, slug, tagCodes);
		}

		private static Skill BuildSkillWithCode(int rating, string name, string code, params string[] tagCodes) {
			return BuildSkillWithCodeAndIconClass(rating, name, code, code, tagCodes);
		}
		
		private static Skill BuildSkillWithIconClass(int rating, string name, string iconClass, params string[] tagCodes) {
			return BuildSkillWithCodeAndIconClass(rating, name, name.Slugify(), iconClass, tagCodes);
		}
		// ReSharper enable UnusedMethodReturnValue.Local

		private static Skill BuildSkillWithCodeAndIconClass(int rating, string name, string code, string iconClass, params string[] tagCodes) {
			var skillId = Guid.NewGuid(); // TODO: Get Comb from library or find my old implementation
			var skill = new Skill {
				SkillId = skillId,
				Name = name,
				Code = code,
				IconClass = iconClass,
				Rating = rating
			};

			_context.Skills.Add(skill);

			foreach (var tagCode in tagCodes) {
				var tag = BuildOrGetTag(tagCode);
				var tagging = new SkillToTag {
					SkillId = skillId,
					TagId = tag.TagId
				};
				_context.SkillTaggings.Add(tagging);
			}

			return skill;
		}
	}
}