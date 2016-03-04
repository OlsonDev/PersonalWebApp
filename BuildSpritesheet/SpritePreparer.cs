#if DNX451
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace BuildSpritesheet {
	class SpritePreparer {
		public static void ProcessJpg(string inputFilePath, string outputFilePath) {
			Console.WriteLine($"Processing JPG: {Path.GetFileName(inputFilePath)}");
			ProcessImage(inputFilePath, outputFilePath);
		}

		public static void ProcessPng(string inputFilePath, string outputFilePath) {
			Console.WriteLine($"Processing PNG: {Path.GetFileName(inputFilePath)}");
			ProcessImage(inputFilePath, outputFilePath);
		}

		public static void ProcessImage(string inputFilePath, string outputFilePath) {
			using (var srcImage = Image.FromFile(inputFilePath)) {
				var newImageSizeAndDest = GetNewImageSize(srcImage);
				using (var newImage = new Bitmap(newImageSizeAndDest.Width, newImageSizeAndDest.Height)) {
					using (var graphics = Graphics.FromImage(newImage)) {
						graphics.SmoothingMode = SmoothingMode.HighQuality;
						graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
						graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
						// Since some sprites have white backgrounds, 'mise well make them all white
						// so we can optimize the image/resulting spritesheet further by removing the Alpha channel
						var whiteBrush = new SolidBrush(Color.White);
						graphics.FillRectangle(whiteBrush, 0, 0, newImageSizeAndDest.Width, newImageSizeAndDest.Height);
						graphics.DrawImage(srcImage, newImageSizeAndDest);
						srcImage.Dispose();
						newImage.Save(outputFilePath, ImageFormat.Png);
					}
				}
			}
		}

		private static Rectangle GetNewImageSize(Image srcImage) {
			var boundByWidth = new Rectangle(0, 0, Config.DesiredSize.Width, Config.DesiredSize.Width * srcImage.Height / srcImage.Width);
			var boundByHeight = new Rectangle(0, 0, Config.DesiredSize.Height * srcImage.Width / srcImage.Height, Config.DesiredSize.Height);

			if (Config.DesiredSize.Width >= Config.DesiredSize.Height) {
				return boundByWidth.Height <= Config.DesiredSize.Height ? boundByWidth : boundByHeight;
			}
			return boundByHeight.Width <= Config.DesiredSize.Height ? boundByHeight : boundByWidth;
		}
		
		public static void ProcessSvg(string inputFilePath, string outputFilePath) {
			Console.WriteLine($"Processing SVG: {Path.GetFileName(inputFilePath)}");
			var args = $"-z -f {inputFilePath} -e {outputFilePath}";
			var inkscape = new Process {
				StartInfo = {
					UseShellExecute = false,
					RedirectStandardOutput = true,
					FileName = Config.InkscapePath,
					Arguments = args
				}
			};
			inkscape.Start();
			inkscape.WaitForExit();
			ProcessImage(outputFilePath, outputFilePath);
		}

		public static void PrepareSprites() {
			var inputFilePaths = Config.GetInputFilePaths();
			foreach (var inputFilePath in inputFilePaths) {
				var ext = Path.GetExtension(inputFilePath)?.ToLower();
				var outputFilePath = Path.ChangeExtension(Config.CombinePath(Config.WwwRootSkillsPath, Path.GetFileName(inputFilePath)),
					"png");
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
		}
	}
}
#endif