using System;
using Server;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
	public class OrnateStoneTablet : HTMLBook
	{
		public override Type Gump { get { return typeof( OrnateStoneTabletGump ); } }
		public override int CharactersPerLineMax { get { return base.CharactersPerLineMax*2; } }
		public override int MaxLines { get { return 16; } }

		[Constructable]
		public OrnateStoneTablet() : base( 9910, 3 )
		{
			Name = "ornate stone tablet";
			Hue = 905;
		}

		public OrnateStoneTablet( Serial serial ) : base( serial )
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
