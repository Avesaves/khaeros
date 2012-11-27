using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class PieceOfUmberhulkShell : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Smoke, 60)
			}; 
		} }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 2; } }
		bool IBombIngredient.InstantEffect { get { return true; } }

		[Constructable]
		public PieceOfUmberhulkShell( int amount ) : base( 0x26B2, amount )
		{
			Name = "piece of umberhulk shell";
			Hue = 1809;
		}

		[Constructable]
		public PieceOfUmberhulkShell() : this( 1 )
		{
		}

		public PieceOfUmberhulkShell( Serial serial ) : base( serial )
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
