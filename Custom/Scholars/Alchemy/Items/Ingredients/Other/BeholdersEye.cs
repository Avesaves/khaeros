using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class BeholdersEye : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Flash, 60)
			}; 
		} }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 1; } }
		bool IBombIngredient.InstantEffect { get { return true; } }

		[Constructable]
		public BeholdersEye( int amount ) : base( 0x1F13, amount )
		{
			Name = "beholder's eye";
			Hue = 2964;
		}

		[Constructable]
		public BeholdersEye() : this( 1 )
		{
		}

		public BeholdersEye( Serial serial ) : base( serial )
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
