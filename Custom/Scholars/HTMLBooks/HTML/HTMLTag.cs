using System;
using Server;

namespace Server.Items
{
	public enum Tag
	{
		None = 0,
		Bold,
		Italic,
		LeftAlign,
		CenterAlign,
		LargeFont,
		SmallFont,
		MediumFont,
		Color
	}
	
	public enum TagType
	{
		Bold = 0,
		Italic = 1,
		Color = 2,
		Alignment = 3,
		Font = 4,
		None = 5
	}

	public abstract class HTMLTag
	{
		private string m_Value = "";
		public virtual Tag Name { get { return Tag.None; } }
		public virtual TagType Type { get { return TagType.None; } }
		public virtual int CharacterCost { get { return 0; } }
		public virtual string Value { get { return m_Value; } set { m_Value = value; } }
		public virtual string OpenTag()
		{
			return "";
		}

		public virtual string CloseTag()
		{
			return "";
		}

		public static bool CompareTags( HTMLTag tag1, HTMLTag tag2 )
		{
			if ( tag1.Name == tag2.Name && tag1.Value == tag2.Value )
				return true;
			else
				return false;
		}

		public static HTMLTag CreateTagInstance( Tag tag )
		{
			switch( (int)tag )
			{
				case (int)Tag.Bold: return new BoldTag();
				case (int)Tag.Italic: return new ItalicTag();
				case (int)Tag.LeftAlign: return new LeftAlignTag();
				case (int)Tag.CenterAlign: return new CenterAlignTag();
				case (int)Tag.LargeFont: return new LargeFontTag();
				case (int)Tag.SmallFont: return new SmallFontTag();
				case (int)Tag.MediumFont: return new MediumFontTag();
				case (int)Tag.Color: return new ColorTag();
				default: return null;
			}
		}
	}
}
