using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class DevourersTeeth : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Shrapnel, 20),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRegeneration, -20)
			}; 
		} }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return -1; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public DevourersTeeth( int amount ) : base( 0x1B1A, amount )
		{
			Name = "devourer's teeth";
		}

		[Constructable]
		public DevourersTeeth() : this( 1 )
		{
		}

		public DevourersTeeth( Serial serial ) : base( serial )
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
