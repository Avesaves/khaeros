using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RustGland : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Rust, 50)
			}; 
		} }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 0; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public RustGland( int amount ) : base( 0x3189, amount )
		{
			Name = "rust gland";
			Hue = 2964;
		}

		[Constructable]
		public RustGland() : this( 1 )
		{
		}

		public RustGland( Serial serial ) : base( serial )
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
