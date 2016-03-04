#if DNX451
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace BuildSpritesheet {
	public static class Config {
		public static readonly Rectangle DesiredSize = new Rectangle(0, 0, 148, 60);
		public const string InkscapePath = @"C:\Storage\Inkscape\inkscape.exe";

		public static IEnumerable<string> GetInputFilePaths() => DirectoryExtensions.GetFilesByExtensionList(SourcePath, ".jpg,.png,.svg");

		// e.g. C:\Storage\Projects\PersonalWebApp\BuildSpritesheet\bin\Debug\BuildSpritesheet.exe
		public static string GetAssemblyLocation() => System.Reflection.Assembly.GetExecutingAssembly().Location;

		// e.g. C:\Storage\Projects\PersonalWebApp\
		public static string BasePath => CombinePath(GetAssemblyLocation(), "../../../../");
		public static string SourcePath => CombinePath(BasePath, "Client/skills/");
		public static string OutputPath => CombinePath(BasePath, "wwwroot/skills/");

		// Use GetFullPath to normalize with Path.DirectorySeparatorChar
		public static string CombinePath(params string[] paths) => Path.GetFullPath(Path.Combine(paths));
	}
}
#endif