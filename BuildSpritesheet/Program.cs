#if DNX451
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace BuildSpritesheet {
	class Program {
		private const int DesiredSize = 60;

		static void Main(string[] args) {
			var outputPath = GetOutputPath();
			Directory.CreateDirectory(outputPath);
			var outputDirectory = new DirectoryInfo(outputPath);
			outputDirectory.Empty();

			var inputFilePaths = GetInputFilePaths();
			foreach (var inputFilePath in inputFilePaths) {
				var ext = Path.GetExtension(inputFilePath)?.ToLower();
				var outputFilePath = Path.ChangeExtension(CombinePath(GetOutputPath(), Path.GetFileName(inputFilePath)), "png");
				switch (ext) {
					case ".jpg":
						ProcessJpg(inputFilePath, outputFilePath);
						break;
					case ".png":
						ProcessPng(inputFilePath, outputFilePath);
						break;
					case ".svg":
						ProcessSvg(inputFilePath, outputFilePath);
						break;
					default:
						throw new NotImplementedException($"Cannot process file with extension ${ext}");
				}
			}

			Console.WriteLine();
			Console.WriteLine("Done processing individual files");

			var outputPngFilePaths = GetOutputPngFilePaths().ToList();
			var num = outputPngFilePaths.Count;
			var extents = GetSpritesheetExtents(num);

			var x = 0;
			var y = 0;
			var css = new StringBuilder();
			css.AppendLine(".icon { background: url(/skills-spritesheet.png) no-repeat; }");
			using (var newImg = new Bitmap(extents.Width, extents.Height)) {
				using (var graphics = Graphics.FromImage(newImg)) {
					graphics.SmoothingMode = SmoothingMode.HighQuality;
					graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
					graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
					foreach (var outputPngFilePath in outputPngFilePaths) {
						using (var srcImg = Image.FromFile(outputPngFilePath)) {
							var dest = new Rectangle(x, y, DesiredSize, DesiredSize);
							graphics.DrawImage(srcImg, dest);
							srcImg.Dispose();
							var fileNoExt = Path.GetFileNameWithoutExtension(outputPngFilePath);
							var xCss = x == 0 ? "0" : $"-{x}px";
							var yCss = y == 0 ? "0" : $"-{y}px";
							css.AppendLine($".icon-{fileNoExt} {{ background-position: {xCss} {yCss}; }}");
							x += 64;
							if (x < extents.Width) continue;
							x = 0;
							y += 64;
						}
					}
				}
				newImg.Save(CombinePath(outputPath, "../skills-spritesheet.png"), ImageFormat.Png);
			}

			css.Length--; // Remove trailing newline
			File.WriteAllText(CombinePath(outputPath, "../css/skills-spritesheet.css"), css.ToString());

			Console.WriteLine("Done building spritesheet");

			Console.ReadKey();
		}

		private static Rectangle GetSpritesheetExtents(int num) {
			var leastWastefulExtents = new Rectangle(0, 0, num * 64, 64);
			if (num <= 8) return leastWastefulExtents;
			var leastNumWastedSpots = int.MaxValue;
			var lowerBound = (int)Math.Floor(Math.Sqrt(num));
			for (var x = lowerBound; x < num; x++) {
				var y = num / x;
				if (y * x < num) y++;
				var waste = y * x - num;
				// Don't replace if equal; lower-valued x is more square (preferred)
				if (waste >= leastNumWastedSpots) continue;
				leastNumWastedSpots = waste;
				// Easier to reason about rows that are multiples of 2
				if (y % 2 == 0 && x % 2 != 0) {
					leastWastefulExtents.Width = y * 64;
					leastWastefulExtents.Height = x * 64;
				} else {
					leastWastefulExtents.Width = x * 64;
					leastWastefulExtents.Height = y * 64;
				}
			}
			return leastWastefulExtents;
		}

		private static void ProcessJpg(string inputFilePath, string outputFilePath) {
			Console.WriteLine($"Processing JPG: {Path.GetFileName(inputFilePath)}");
			ProcessImage(inputFilePath, outputFilePath);
		}

		private static void ProcessPng(string inputFilePath, string outputFilePath) {
			Console.WriteLine($"Processing PNG: {Path.GetFileName(inputFilePath)}");
			ProcessImage(inputFilePath, outputFilePath);
		}

		private static void ProcessImage(string inputFilePath, string outputFilePath) {
			using (var srcImg = Image.FromFile(inputFilePath)) {
				using (var newImg = new Bitmap(DesiredSize, DesiredSize)) {
					using (var graphics = Graphics.FromImage(newImg)) {
						graphics.SmoothingMode = SmoothingMode.HighQuality;
						graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
						graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

						var dest = new Rectangle(0, 0, DesiredSize, DesiredSize);
						if (srcImg.Width < srcImg.Height) {
							dest.Width = (int)(DesiredSize * (double)srcImg.Width / srcImg.Height);
							dest.X = (DesiredSize - dest.Width) / 2;
						} else {
							dest.Height = (int)(DesiredSize * (double)srcImg.Height / srcImg.Width);
							dest.Y = (DesiredSize - dest.Height) / 2;
						}

						graphics.DrawImage(srcImg, dest);
						srcImg.Dispose();
						newImg.Save(outputFilePath, ImageFormat.Png);
					}
				}
			}
		}

		private const string InkscapePath = @"C:\Storage\Inkscape\inkscape.exe";

		private static void ProcessSvg(string inputFilePath, string outputFilePath) {
			Console.WriteLine($"Processing SVG: {Path.GetFileName(inputFilePath)}");
			var args = $"-z -f {inputFilePath} -e {outputFilePath}";
			var inkscape = new Process {
				StartInfo = {
					UseShellExecute = false,
					RedirectStandardOutput = true,
					FileName = InkscapePath,
					Arguments = args
				}
			};
			inkscape.Start();
			inkscape.WaitForExit();
			ProcessImage(outputFilePath, outputFilePath);
		}

		private static IEnumerable<string> GetInputFilePaths() {
			var sourcePath = GetSourcePath();
			return DirectoryExtensions.GetFilesByExtensionList(sourcePath, ".jpg,.png,.svg");
		}

		private static IEnumerable<string> GetOutputPngFilePaths() {
			var outputPath = GetOutputPath();
			return DirectoryExtensions.GetFilesByExtensionList(outputPath, ".png");
		}

		static string GetBasePath() {
			// e.g. C:\Storage\Projects\PersonalWebApp\BuildSpritesheet\bin\Debug\BuildSpritesheet.exe
			var asmLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
			// e.g. C:\Storage\Projects\PersonalWebApp\
			return CombinePath(asmLocation, "../../../../");
		}

		static string GetSourcePath() {
			var basePath = GetBasePath();
			return CombinePath(basePath, "Client/skills/");
		}

		static string GetOutputPath() {
			var basePath = GetBasePath();
			return CombinePath(basePath, "wwwroot/skills/");
		}

		static string CombinePath(params string[] paths) {
			// Use GetFullPath to normalize with Path.DirectorySeparatorChar
			return Path.GetFullPath(Path.Combine(paths));
		}
	}
}
#endif