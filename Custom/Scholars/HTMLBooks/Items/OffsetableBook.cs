using System;
using Server;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
	public abstract class OffsetableBook : HTMLBook
	{
		public override Type Gump { get { return typeof( OffsetableBookGump ); } }
		public virtual int GumpID { get { return 0; } }		// the gump that is displayed
		public virtual Point2D PrevPageButtonOffset { get { return new Point2D( 0, 0 ); } }
		public virtual Point2D NextPageButtonOffset { get { return new Point2D( 0, 0 ); } }
		public virtual Point2D HTMLOffset { get { return new Point2D( 0, 0 ); } }
		public virtual Rectangle2D GumpDimensions()
		{
			return new Rectangle2D(0, 0, 0, 0);
		}

		[Constructable]
		public OffsetableBook( int itemID, int pagenum ) : base( itemID, pagenum )
		{
		}

		public OffsetableBook( Serial serial ) : base( serial )
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
