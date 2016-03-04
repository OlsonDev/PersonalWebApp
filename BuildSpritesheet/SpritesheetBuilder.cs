#if DNX451
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using BuildSpritesheet.TexturePacker;
using Newtonsoft.Json;

namespace BuildSpritesheet {
	public class SpritesheetBuilder {
		public static void BuildSpritesheet() {
			RunTexturePacker();
			BuildCleanSpritesheetAndCssFiles();
		}

		private static void RunTexturePacker() {
			var texturePacker = new Process {
				StartInfo = {
					UseShellExecute = false,
					RedirectStandardOutput = true,
					FileName = Config.TexturePackerPath,
					Arguments = Config.TexturePackerTpsFilePath
				}
			};
			texturePacker.Start();
			texturePacker.WaitForExit();
		}

		private static void BuildCleanSpritesheetAndCssFiles() {
			var css = new StringBuilder();
			var iconRule = ".icon { background-image: url(/skills-spritesheet.png); ";
			
			using (var file = File.OpenText(Config.JsonFilePath)) {
				using (var reader = new JsonTextReader(file)) {
					var serializer = new JsonSerializer();
					var sheet = serializer.Deserialize<Spritesheet>(reader);
					var w = sheet.Meta.Size.W;
					var h = sheet.Meta.Size.H;
					using (var sheetBmp = new Bitmap(w, h)) {
						using (var graphics = Graphics.FromImage(sheetBmp)) {
							var white = new SolidBrush(Color.White);
							graphics.FillRectangle(white, 0, 0, w, h);
							foreach (var frame in sheet.Frames) {
								var filename = Config.CombinePath(Config.WwwRootPath, frame.Filename);
								using (var spriteImg = Image.FromFile(filename)) {
									var dest = frame.Frame.ToRectangle();
									graphics.DrawImage(spriteImg, dest);
								}
								var slug = Path.GetFileNameWithoutExtension(filename);
								var xPos = frame.Frame.X == 0 ? "0" : $"-{frame.Frame.X}px";
								var yPos = frame.Frame.Y == 0 ? "0" : $"-{frame.Frame.Y}px";
								var rules = $"width: {frame.Frame.W}px; height: {frame.Frame.H}px; background-position: {xPos} {yPos};";
								if (slug == "aaaa-missing") {
									iconRule += rules + " }" + Environment.NewLine;
								} else {
									css.AppendLine($".icon-{slug} {{ {rules} }}");
								}
							}
							sheetBmp.Save(Config.SheetFilePath, ImageFormat.Png);
						}
					}
				}
			}

			css.Insert(0, iconRule);
			// Remove trailing newline
			css.Length -= 2;
			File.WriteAllText(Config.CssFilePath, css.ToString());
		}
	}
}
#endif