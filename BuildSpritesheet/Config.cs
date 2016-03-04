#if DNX451
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace BuildSpritesheet {
	public static class Config {
		public static readonly Rectangle DesiredSize = new Rectangle(0, 0, 148, 60);
		public const string InkscapePath = @"C:\Storage\Inkscape\inkscape.exe";
		public const string TexturePackerPath = @"C:\Program Files\CodeAndWeb\TexturePacker\bin\texturepacker.exe";

		public static IEnumerable<string> GetInputFilePaths() => DirectoryExtensions.GetFilesByExtensionList(ClientSkillsPath, ".jpg,.png,.svg");

		// e.g. C:\Storage\Projects\PersonalWebApp\BuildSpritesheet\bin\Debug\BuildSpritesheet.exe
		public static string GetAssemblyLocation() => System.Reflection.Assembly.GetExecutingAssembly().Location;

		// e.g. C:\Storage\Projects\PersonalWebApp\
		public static string BasePath => CombinePath(GetAssemblyLocation(), "../../../..");
		
		public static string ClientPath => CombinePath(BasePath, "Client");
		public static string ClientSkillsPath => CombinePath(ClientPath, "skills");
		public static string TexturePackerTpsFilePath => CombinePath(ClientPath, "skills.tps");

		public static string WwwRootPath => CombinePath(BasePath, "wwwroot");
		public static string WwwRootSkillsPath => CombinePath(WwwRootPath, "skills");

		public static string SheetFilePath => CombinePath(WwwRootPath, "skills-spritesheet.png");
		public static string JsonFilePath => CombinePath(WwwRootPath, "skills-spritesheet.json");

		public static string CssFilePath => CombinePath(WwwRootPath, "css/skills-spritesheet.css");
		
		// Use GetFullPath to normalize with Path.DirectorySeparatorChar
		public static string CombinePath(params string[] paths) => Path.GetFullPath(Path.Combine(paths));
	}
}
#endif