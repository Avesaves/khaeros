using System;
using Server;

namespace Server.Items
{
	public class LeftAlignTag : HTMLTag
	{
		public override Tag Name { get { return Tag.LeftAlign; } }
		public override TagType Type { get { return TagType.Alignment; } }
		public override string OpenTag()
		{
			return "<LEFT>";
		}

		public override string CloseTag()
		{
			return "</LEFT>";
		}
	}

	public class CenterAlignTag : HTMLTag
	{
		public override Tag Name { get { return Tag.CenterAlign; } }
		public override TagType Type { get { return TagType.Alignment; } }
		public override string OpenTag()
		{
			return "<CENTER>";
		}

		public override string CloseTag()
		{
			return "</CENTER>";
		}
	}
}
