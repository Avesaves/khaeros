using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EttercapSilk : BaseIngredient, IOilIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] {
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRegeneration, -10)
			}; 
		} }

		bool IOilIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IOilIngredient.Corrosivity { get { return 1; } }
		int IOilIngredient.Duration { get { return 0; } }

		[Constructable]
		public EttercapSilk( int amount ) : base( 0xF8D, amount )
		{
			Name = "ettercap silk";
			Hue = 2965;
		}

		[Constructable]
		public EttercapSilk() : this( 1 )
		{
		}

		public EttercapSilk( Serial serial ) : base( serial )
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
