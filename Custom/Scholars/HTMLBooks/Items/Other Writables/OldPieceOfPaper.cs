using System;
using Server;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
	public class OldPieceOfPaper : HTMLBook
	{
		public override Type Gump { get { return typeof( OldPieceOfPaperGump ); } }
		public override int CharactersPerLineMax { get { return base.CharactersPerLineMax*2; } }
		public override int MaxLines { get { return 16; } }

		[Constructable]
		public OldPieceOfPaper() : base( 5357, 3 )
		{
			Name = "old piece of paper";
			Hue = 47;
		}

		public OldPieceOfPaper( Serial serial ) : base( serial )
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
