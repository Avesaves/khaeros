using System;
using Server;

namespace Server.Gumps
{
	interface IHTMLBookGump
	{
		Rectangle2D GetDimensions( string page1Text, string page2Text ); // get gump dimensions for these pages
		void DrawToSurface(Gump gump, int x, int y, string page1Text, string page2Text, int page); // draw this gump to another surface (i.e. gump)
	}
}
