using System;
using System.Collections.Generic;
using Microsoft.AspNet.Builder;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PersonalWebApp.Extensions;
using PersonalWebApp.Models.Entity;

namespace PersonalWebApp.Models.Db {
	public static class DbSeeder {
		private static ApplicationDbContext _context;

		// implemented as an extension method on IApplicationBuilder that we can call from Startup.cs
		public static void EnsureSampleData(this IApplicationBuilder app) {
			_context = app.ApplicationServices.GetService<ApplicationDbContext>();

			if (_context.Skills.Any()) return;

			BuildSkillWithCode(5, "C#", "c-sharp", "language");
			BuildSkill(3, "Visual Basic .NET", "language");
			BuildSkill(2, "Visual Basic 6", "language");
			BuildSkill(3, "ASP.NET", "server", "framework");
			BuildSkill(4, "ASP.NET MVC", "server", "framework");
			BuildSkill(4, "Entity Framework", "server", "framework", "database");
			BuildSkill(3, "NHibernate", "server", "framework", "database");
			BuildSkill(3, "Java", "language");
			BuildSkill(5, "Railo", "framework", "server");
			BuildSkill(5, "Railo CFML", "language");
			BuildSkill(4, "Adobe ColdFusion", "framework", "server");
			BuildSkill(5, "Adobe CFML", "language");
			BuildSkill(4, "Apache Tomcat", "server");
			BuildSkill(5, "HTML5", "language", "web");
			BuildSkill(5, "Markdown", "language");
			BuildSkill(5, "CSS3", "language", "web");
			BuildSkill(5, "LESS", "language", "web");
			BuildSkill(4, "SCSS", "language", "web");
			BuildSkill(5, "JavaScript", "language", "web", "server");
			BuildSkill(3, "TypeScript", "language", "web", "server");
			BuildSkill(2, "Dart", "language", "web", "server");
			BuildSkill(4, "lodash", "library", "web", "server");
			BuildSkill(5, "jQuery", "library", "web");
			BuildSkill(5, "jQuery UI", "library", "web");
			BuildSkill(5, "jQuery Mobile", "language", "web");
			BuildSkill(5, "KnockoutJS", "framework", "web");
			BuildSkill(4, "AngularJS 1.x", "framework", "web");
			BuildSkill(4, "Angular 2.x", "framework", "web");
			BuildSkill(3, "Polymer", "framework", "web");
			BuildSkill(4, "NodeJS", "framework", "web", "server");
			BuildSkill(4, "npm", "tool");
			BuildSkill(4, "gulp", "tool");
			BuildSkill(4, "grunt", "tool");
			BuildSkill(4, "bower", "tool");
			BuildSkill(4, "typings", "tool");
			BuildSkill(5, "svn", "tool");
			BuildSkill(4, "git", "tool");
			BuildSkill(3, "Mercurial", "tool");
			BuildSkill(4, "Electron", "framework", "web", "server");
			BuildSkill(3, "ActionScript 3.0", "language", "web", "desktop");
			BuildSkill(3, "Flex Framework 4", "framework", "web", "desktop");
			BuildSkillWithCode(4, "Microsoft SQL Server", "sql-server", "database");
			BuildSkill(3, "MySQL", "database", "language");
			BuildSkill(4, "T-SQL", "language");
			BuildSkill(2, "PL/SQL", "language");
			BuildSkill(3, "MongoDB", "database");
			BuildSkillWithCode(2, "C++14", "c-plus-plus", "language");
			BuildSkillWithCode(2, "C++ CLR", "c-plus-plus-clr", "language", "framework");
			BuildSkill(4, "Tessel", "hardware");
			BuildSkill(3, "Teensy", "hardware");
			BuildSkill(3, "Arduino", "hardware");
			BuildSkill(2, "Perl", "language");
			BuildSkill(3, "Python 3", "language");
			BuildSkill(5, "Regex", "language");
			BuildSkill(4, "Windows Forms", "framework", "desktop");
			BuildSkill(4, "WPF", "framework", "desktop");
			BuildSkill(4, "XAML", "language", "web", "desktop");
			BuildSkill(1, "DirectX", "library", "desktop");
			BuildSkill(3, "WebGL", "library", "web");
			BuildSkill(3, "OpenGL", "library", "desktop");
			BuildSkill(2, "GLSL", "language", "desktop");
			BuildSkill(1, "HLSL", "language", "desktop");
			BuildSkill(3, "XNA", "library", "framework", "desktop");
			BuildSkill(3, "Unreal Engine", "framework", "web", "desktop", "app");
			BuildSkill(3, "Unity", "framework", "web", "desktop", "app");

			BuildSkill(3, "Bash", "tool", "language");
			BuildSkill(2, "cmd", "tool", "language");

			BuildSkill(4, "Eclipse", "tool", "app");
			BuildSkill(4, "ColdFusion Builder", "tool", "app");
			BuildSkill(4, "Dreamweaver", "tool", "app");
			BuildSkill(4, "Flash Builder 4", "tool", "app");
			BuildSkill(4, "Flash Professional", "tool", "app");
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
			BuildSkill(2, "Adobe PhotoShop", "tool", "app");

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

		// ReSharper disable once UnusedMethodReturnValue.Local
		private static Skill BuildSkill(int rating, string name, params string[] tagCodes) {
			return BuildSkillWithCode(rating, name, name.Slugify(), tagCodes);
		}

		private static Skill BuildSkillWithCode(int rating, string name, string code, params string[] tagCodes) {
			var skillId = Guid.NewGuid(); // TODO: Get Comb from library or find my old implementation
			var skill = new Skill {
				SkillId = skillId,
				Name = name,
				Code = code,
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