using System;
using System.Text.RegularExpressions;

namespace PersonalWebApp.Extensions {
	public static class StringExtensions {
		public static string Right(this string str, int maxLength) {
			return str.Substring(0, Math.Min(str.Length, maxLength));
		}

		public static string Capitalize(this string str, bool lowercaseTail = false) {
			return str.Length > 1
				? char.ToUpper(str[0]) + (lowercaseTail ? str.Substring(1).ToLower() : str.Substring(1))
				: str.ToUpper()
			;
		}

		public static string PascalToSlug(this string pascalString) {
			return Regex.Replace(pascalString, "([a-z])([A-Z])", "$1-$2").ToLower();
		}

		public static string Slugify(this string phrase, int maxLength = 45) {
			if (string.IsNullOrEmpty(phrase)) return "";
			var slug = phrase.Trim().RemoveAccent().ToLower();                    // trim, remove accents, lowercase
			slug = slug.Replace("@", "-at-");                                     // @ => -at-
			slug = slug.Replace("&", "-and-");                                    // & => -and-
			slug = Regex.Replace(slug, @"[!#$%^*(){}[\]+=_<>?,./\\|`~;:]+", "-"); // convert specific characters to hyphens
			slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");                      // remove invalid chars
			slug = Regex.Replace(slug, @"\s+", " ");                              // collapse whitespace
			slug = slug.Replace(' ', '-');                                        // spaces => hyphens
			slug = Regex.Replace(slug, "-+", "-");                                // collapse hyphens
			slug = Regex.Replace(slug, "^-", "");                                 // remove leading hyphen
			slug = slug.Right(maxLength);                                         // cut to max length
			slug = slug.TrimEnd('-');                                             // remove possible trailing hyphen
			return slug;
		}

		public static string RemoveAccent(this string str) {
			// TODO: Look into better methods, ex: http://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
			var bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(str);
			return System.Text.Encoding.ASCII.GetString(bytes);
		}

		public static string FirstCharacterToLower(this string str) {
			return string.IsNullOrEmpty(str) || char.IsLower(str, 0)
				? str
				: char.ToLowerInvariant(str[0]) + str.Substring(1)
			;
		}
	}
}