using System;
using Server;

namespace Server.Items
{
	public class BoldTag : HTMLTag
	{
		public override Tag Name { get { return Tag.Bold; } }
		public override TagType Type { get { return TagType.Bold; } }
		public override int CharacterCost { get { return 2; } }
		public override string OpenTag()
		{
			return "<B>";
		}

		public override string CloseTag()
		{
			return "</B>";
		}
	}
}
