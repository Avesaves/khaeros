using System;
using Server;

namespace Server.Items
{
	public class LargeFontTag : HTMLTag
	{
		public override Tag Name { get { return Tag.LargeFont; } }
		public override TagType Type { get { return TagType.Font; } }
		public override int CharacterCost { get { return 3; } }
		public override string OpenTag()
		{
			return "<BIG>";
		}

		public override string CloseTag()
		{
			return "</BIG>";
		}
	}

	public class MediumFontTag : HTMLTag
	{
		public override Tag Name { get { return Tag.MediumFont; } }
		public override TagType Type { get { return TagType.Font; } }
		public override int CharacterCost { get { return 1; } }
		public override string OpenTag()
		{
			return "<H6>";
		}

		public override string CloseTag()
		{
			return "</H6>";
		}
	}

	public class SmallFontTag : HTMLTag
	{
		public override Tag Name { get { return Tag.SmallFont; } }
		public override TagType Type { get { return TagType.Font; } }
		public override int CharacterCost { get { return 0; } }
		public override string OpenTag()
		{
			return "<SMALL>";
		}

		public override string CloseTag()
		{
			return "</SMALL>";
		}
	}
}
