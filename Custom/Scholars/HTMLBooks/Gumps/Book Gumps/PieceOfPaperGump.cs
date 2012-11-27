using System;
using Server;
using Server.Items;
using System.Text;

namespace Server.Gumps
{
	public class PieceOfPaperGump : Gump, IHTMLBookGump
	{
		public PieceOfPaperGump( HTMLBook book ) : base(0, 0)
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
			return new Rectangle2D( 0, 0, 291, 333 );
		}
		
		void IHTMLBookGump.DrawToSurface(Gump gump, int x, int y, string page1Text, string page2Text, int page)
		{
			gump.AddBackground( x, y, 291, 333, 9350 );
			gump.AddHtml( 10+x, 10+y, 274, 167*2, page1Text, (bool)false, (bool)false);
		}
	}
}
