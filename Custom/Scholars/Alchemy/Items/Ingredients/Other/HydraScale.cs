using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class HydraScale : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Intelligence, 60),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, -30),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPoints, -30)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public HydraScale( int amount ) : base( 0x26B4, amount )
		{
			Name = "hydra scale";
			Hue = 361;
		}

		[Constructable]
		public HydraScale() : this( 1 )
		{
		}

		public HydraScale( Serial serial ) : base( serial )
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
