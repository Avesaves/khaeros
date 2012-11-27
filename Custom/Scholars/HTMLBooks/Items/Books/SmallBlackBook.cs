using System;
using Server;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
	public class SmallBlackBook : OffsetableBook
	{
		public override int CharactersPerLineMax { get { return base.CharactersPerLineMax*2; } }
		public override int MaxLines { get { return 16; } }

		[Constructable]
		public SmallBlackBook() : base( 9120, 100 )
		{
			Name = "small black book";
			Weight = 3.0;
			Layer = Layer.OneHanded;
		}

		public SmallBlackBook( Serial serial ) : base( serial )
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}
	}
}
