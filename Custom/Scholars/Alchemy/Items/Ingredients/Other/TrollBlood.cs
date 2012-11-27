using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class TrollBlood : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRegeneration, 60),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Hunger, -40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Thirst, -40)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public TrollBlood( int amount ) : base( 0xE24, amount )
		{
			Name = "troll blood";
			Hue = 2977;
		}

		[Constructable]
		public TrollBlood() : this( 1 )
		{
		}

		public TrollBlood( Serial serial ) : base( serial )
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
