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
			Console.ReadKey();
		}

		private static void PrepareOutputDirectory() {
			Directory.CreateDirectory(Config.OutputPath);
			var outputDirectory = new DirectoryInfo(Config.OutputPath);
			outputDirectory.Empty();
		}
	}
}
#endif