#if DNX461
namespace BuildSpritesheet.TexturePacker {
	public class SpriteFrame {
		public string Filename { get; set; }
		public Rect Frame { get; set; }
		public bool Rotated { get; set; }
		public bool Trimmed { get; set; }
		public Rect SpriteSourceSize { get; set; }
		public Size SourceSize { get; set; }
		public Point Pivot { get; set; }
	}
}
#endif