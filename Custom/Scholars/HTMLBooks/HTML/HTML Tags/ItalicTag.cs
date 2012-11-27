using System;
using Server;

namespace Server.Items
{
	public class ItalicTag : HTMLTag
	{
		public override Tag Name { get { return Tag.Italic; } }
		public override TagType Type { get { return TagType.Italic; } }
		public override int CharacterCost { get { return 1; } }
		public override string OpenTag()
		{
			return "<I>";
		}

		public override string CloseTag()
		{
			return "</I>";
		}
	}
}
