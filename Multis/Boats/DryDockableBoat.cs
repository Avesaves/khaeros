using System;
using Server;
using Server.Items;

namespace Server.Multis
{
    public class DryDockableBoat : BaseBoat
	{
        public override bool CanBeDryDocked() { return true; }

		public override int NorthID{ get{ return 0x4014; } }
		public override int  EastID{ get{ return 0x4015; } }
		public override int SouthID{ get{ return 0x4016; } }
		public override int  WestID{ get{ return 0x4017; } }

		public override int HoldDistance{ get{ return 5; } }
		public override int TillerManDistance{ get{ return -5; } }

		public override Point2D StarboardOffset{ get{ return new Point2D(  2, -1 ); } }
		public override Point2D      PortOffset{ get{ return new Point2D( -2, -1 ); } }

		public override Point3D MarkOffset{ get{ return new Point3D( 0, 0, 3 ); } }

		public override BaseDockedBoat DockedBoat{ get{ return new DryDockableDockedBoat ( this ); } }

		[Constructable]
		public DryDockableBoat()
		{
		}

		public DryDockableBoat( Serial serial ) : base( serial )
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

			writer.Write( (int)0 );
		}

	}

	public class DryDockableBoatDeed : BaseBoatDeed
	{
		public override int LabelNumber{ get{ return 1041210; } }// magical large dragon ship
		public override BaseBoat Boat{ get{ return new DryDockableBoat(); } }

		[Constructable]
		public DryDockableBoatDeed() : base( 0x4014, new Point3D( 0, -1, 0 ) )
		{
		}

		public DryDockableBoatDeed( Serial serial ) : base( serial )
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

			writer.Write( (int)0 );
		}
	}

	public class DryDockableDockedBoat : BaseDockedBoat
	{
		public override BaseBoat Boat{ get{ return new DryDockableBoat(); } }

		public DryDockableDockedBoat( BaseBoat boat ) : base( 0x4014, new Point3D( 0, -1, 0 ), boat )
		{
		}

		public DryDockableDockedBoat( Serial serial ) : base( serial )
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

			writer.Write( (int)0 );
		}
	}
}
