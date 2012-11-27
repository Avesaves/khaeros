using System;
using Server;

namespace Server.Items
{
	public class ColorTag : HTMLTag
	{
		public override Tag Name { get { return Tag.Color; } }
		public override TagType Type { get { return TagType.Color; } }
		public override string OpenTag()
		{
			return "<BASEFONT COLOR=#" + Value + ">";
		}

		public override string CloseTag() // return "" because we can't close this tag (text vanishes) HTMLContent will handle reopening.
		{
			return "";
		}
	}
}
