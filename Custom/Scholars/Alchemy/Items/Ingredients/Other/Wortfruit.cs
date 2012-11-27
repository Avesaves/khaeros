using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class Wortfruit : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Adhesive, 40)
			}; 
		} }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 0; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public Wortfruit( int amount ) : base( 0x1727, amount )
		{
			Name = "wortfruit";
			Hue = 2962;
		}

		[Constructable]
		public Wortfruit() : this( 1 )
		{
		}

		public Wortfruit( Serial serial ) : base( serial )
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
