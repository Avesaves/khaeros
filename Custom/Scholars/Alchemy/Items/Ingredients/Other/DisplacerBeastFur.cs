using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class DisplacerBeastFur : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Smoke, 50)
			}; 
		} }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 2; } }
		bool IBombIngredient.InstantEffect { get { return true; } }

		[Constructable]
		public DisplacerBeastFur( int amount ) : base( 0x11F5, amount )
		{
			Name = "displacer beast fur";
			Hue = 2884;
		}

		[Constructable]
		public DisplacerBeastFur() : this( 1 )
		{
		}

		public DisplacerBeastFur( Serial serial ) : base( serial )
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
