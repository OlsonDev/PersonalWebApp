#if DNX461
using System.Collections.Generic;

namespace BuildSpritesheet.TexturePacker {
	public class Spritesheet {
		public List<SpriteFrame> Frames { get; set; }
		public Meta Meta { get; set; }
	}
}
#endif