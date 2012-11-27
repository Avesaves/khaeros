using System;
using Server;
using Server.Items;
using System.Text;

namespace Server.Gumps
{
	public class RoughScrollGump : Gump, IHTMLBookGump
	{
		public RoughScrollGump( HTMLBook book ) : base(0, 0)
		{
			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);

			string text1 = "";

			if ( book.HTMLContent.CachedHTMLContent[0].Lines != null )
				for (int i = 0; i < book.HTMLContent.CachedHTMLContent[0].Lines.Length; i++)
					text1 += book.HTMLContent.CachedHTMLContent[0].Lines[i];
				
			(this as IHTMLBookGump).DrawToSurface( this, 200, 95, text1, "", 0 );
		}
		
		Rectangle2D IHTMLBookGump.GetDimensions( string page1Text, string page2Text )
		{
			return new Rectangle2D( 0, 0, 313, 362 );
		}
		
		void IHTMLBookGump.DrawToSurface(Gump gump, int x, int y, string page1Text, string page2Text, int page)
		{
			gump.AddBackground(x, y, 313, 362, 9380);
			gump.AddHtml( 23+x, 36+y, 274, 167*2, page1Text, (bool)false, (bool)false);
		}
	}
}
