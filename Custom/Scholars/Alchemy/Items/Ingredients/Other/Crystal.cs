using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class Crystal : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Flash, 40)
			}; 
		} }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 0; } }
		bool IBombIngredient.InstantEffect { get { return true; } }

		[Constructable]
		public Crystal( int amount ) : base( 0x35DA, amount )
		{
			Name = "crystal";
			Hue = 2973;
		}

		[Constructable]
		public Crystal() : this( 1 )
		{
		}

		public Crystal( Serial serial ) : base( serial )
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
