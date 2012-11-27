using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class HornedBeetleHorn : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRegeneration, 10),
				new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRegeneration, 10),
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRegeneration, 10)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public HornedBeetleHorn( int amount ) : base( 0x3D02, amount )
		{
			Name = "horned beetle horn";
			Hue = 2588;
		}

		[Constructable]
		public HornedBeetleHorn() : this( 1 )
		{
		}

		public HornedBeetleHorn( Serial serial ) : base( serial )
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
