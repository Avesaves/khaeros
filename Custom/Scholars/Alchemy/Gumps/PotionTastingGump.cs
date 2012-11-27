using Server;
using Server.Items;
using System;
using System.Collections.Generic;

namespace Server.Gumps
{
	public class PotionTastingGump : Gump
	{
		public PotionTastingGump( DrinkPotion potion, Mobile taster ) : base(0, 0)
		{
			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;

			string htmlText = "<CENTER><BASEFONT COLOR=#5BAE0A>";

			List<string> effects = new List<string>();
			List<string> sideEffects = new List<string>();

			foreach ( KeyValuePair<CustomEffect, int> kvp in potion.Effects )
			{
				CustomPotionEffect effect = CustomPotionEffect.GetEffect( kvp.Key );
				if ( effect != null )
				{
					if ( kvp.Value > 0 )
						effects.Add( effect.Name );
					else
						sideEffects.Add( effect.Name );
				}
			}

			foreach ( string name in effects )
				htmlText += "+ " + name + "<BR>";

			htmlText += "<BASEFONT COLOR=#BF2121>";

			foreach ( string name in sideEffects )
				htmlText += "- " + name + "<BR>";

			this.AddPage(0);
			this.AddBackground(198, 119, 249, 265, 9270);
			this.AddBackground(208, 192, 228, 186, 83);
			this.AddHtml( 222, 209, 199, 152, htmlText, (bool)false, (bool)true);
			this.AddImage(190, 114, 30079);
			this.AddImage(193, 332, 30080);
			this.AddImage(396, 292, 30081);
			this.AddHtml( 220, 136, 207, 22, "<CENTER><BASEFONT COLOR=#AAAA11>" + potion.Name, (bool)false, (bool)false);

			if ( Utility.RandomDouble() < 0.05 )
				this.AddLabel(243, 169, 39, "Hmm, tastes like chicken.");
			else
				this.AddImage(278, 175, 30061);
		}
	}
}
