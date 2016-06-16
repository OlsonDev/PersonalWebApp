#if DNX461
using System.Drawing;

namespace BuildSpritesheet.TexturePacker {
	public class Rect {
		public int X { get; set; }
		public int Y { get; set; }
		public int W { get; set; }
		public int H { get; set; }

		public Rectangle ToRectangle() {
			return new Rectangle(X, Y, W, H);
		}
	}
}
#endif