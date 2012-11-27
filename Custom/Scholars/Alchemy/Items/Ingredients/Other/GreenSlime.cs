using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class GreenSlime : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.StickyGoo, 40)
			}; 
		} }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 0; } }
		bool IBombIngredient.InstantEffect { get { return true; } }

		[Constructable]
		public GreenSlime( int amount ) : base( 0x122C, amount )
		{
			Name = "green slime";
			Hue = 2703;
		}

		[Constructable]
		public GreenSlime() : this( 1 )
		{
		}

		public GreenSlime( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
