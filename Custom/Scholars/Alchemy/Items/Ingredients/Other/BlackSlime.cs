using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class BlackSlime : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Adhesive, 20)
			}; 
		} }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 0; } }
		bool IBombIngredient.InstantEffect { get { return true; } }

		[Constructable]
		public BlackSlime( int amount ) : base( 0x122C, amount )
		{
			Name = "black slime";
			Hue = 2989;
		}

		[Constructable]
		public BlackSlime() : this( 1 )
		{
		}

		public BlackSlime( Serial serial ) : base( serial )
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
