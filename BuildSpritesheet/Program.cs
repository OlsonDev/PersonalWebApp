#if DNX451
using System;
using System.IO;

namespace BuildSpritesheet {
	class Program {
		static void Main(string[] args) {
			PrepareOutputDirectory();
			SpritePreparer.PrepareSprites();

			Console.WriteLine();
			Console.WriteLine("Done processing individual files");

			Console.WriteLine();
			SpritesheetBuilder.BuildSpritesheet();

			Console.WriteLine();
			Console.WriteLine("Done building spritesheet");

			Console.ReadKey();
		}

		private static void PrepareOutputDirectory() {
			Directory.CreateDirectory(Config.WwwRootSkillsPath);
			var outputDirectory = new DirectoryInfo(Config.WwwRootSkillsPath);
			outputDirectory.Empty();
		}
	}
}
#endif