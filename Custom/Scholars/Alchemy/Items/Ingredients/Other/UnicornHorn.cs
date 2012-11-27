using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class UnicornHorn : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Cure, 60),
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
		public UnicornHorn( int amount ) : base( 0x315C, amount )
		{
			Name = "unicorn horn";
			Hue = 2101;
		}

		[Constructable]
		public UnicornHorn() : this( 1 )
		{
		}

		public UnicornHorn( Serial serial ) : base( serial )
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
