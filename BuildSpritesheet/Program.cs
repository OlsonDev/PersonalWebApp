using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BuildSpritesheet {
	class Program {
		static void Main(string[] args) {
			Directory.CreateDirectory(GetOutputPath());

			var logos = GetLogos();

			foreach (var inputFilePath in logos) {
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
			Console.ReadKey();
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
				using (var newImg = new Bitmap(60, 60)) {
					using (var graphics = Graphics.FromImage(newImg)) {
						graphics.SmoothingMode = SmoothingMode.HighQuality;
						graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
						graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

						var dest = new Rectangle(0, 0, 60, 60);
						if (srcImg.Width < srcImg.Height) {
							dest.Width = (int)(60.0d * srcImg.Width / srcImg.Height);
							dest.X = (60 - dest.Width) / 2;
						} else {
							dest.Height = (int)(60.0d * srcImg.Height / srcImg.Width);
							dest.Y = (60 - dest.Height) / 2;
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

		private static IEnumerable<string> GetLogos() {
			var sourcePath = GetSourcePath();
			return DirectoryExtensions.GetFilesByExtensionList(sourcePath, ".jpg,.png,.svg");
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