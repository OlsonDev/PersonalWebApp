using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BuildSpritesheet {
	public static class DirectoryExtensions {
		/// <summary>
		/// Helper method for when you need to get files with varied extensions
		/// </summary>
		/// <param name="path">The directory path</param>
		/// <param name="extensionList">A list of extensions. Example: ".jpg,.png,.svg"</param>
		/// <param name="searchOption">TopDirectoryOnly or AllDirectories</param>
		/// <returns></returns>
		public static IEnumerable<string> GetFilesByExtensionList(string path, string extensionList, SearchOption searchOption = SearchOption.TopDirectoryOnly) {
			var regexPattern = $"({extensionList.Replace(',', '|')})$";
      return GetFilesByRegex(path, regexPattern, searchOption);
		}

		public static IEnumerable<string> GetFilesByRegex(string path, string regexPattern = "", SearchOption searchOption = SearchOption.TopDirectoryOnly) {
			var reSearchPattern = new Regex(regexPattern, RegexOptions.IgnoreCase);
			return Directory.EnumerateFiles(path, "*", searchOption).Where(file => reSearchPattern.IsMatch(Path.GetFileName(file)));
		}
	}
}