using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class MinotaurHorn : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, 60),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Dexterity, -30),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Stamina, -30)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public MinotaurHorn( int amount ) : base( 0x3BCB, amount )
		{
			Name = "minotaur horn";
			Hue = 2424;
		}

		[Constructable]
		public MinotaurHorn() : this( 1 )
		{
		}

		public MinotaurHorn( Serial serial ) : base( serial )
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
