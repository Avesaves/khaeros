using System;
using System.Collections;
using Server;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0x2815, 0x2816 )]
	public class TallCabinet : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public TallCabinet() : base( 0x2815 )
		{
			Weight = 1.0;
		}

		public TallCabinet( Serial serial ) : base( serial )
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

	[Furniture]
	[Flipable( 0x2817, 0x2818 )]
	public class ShortCabinet : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public ShortCabinet() : base( 0x2817 )
		{
			Weight = 1.0;
		}

		public ShortCabinet( Serial serial ) : base( serial )
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


	[Furniture]
	[Flipable( 0x2857, 0x2858 )]
	public class RedArmoire : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public RedArmoire() : base( 0x2857 )
		{
			Weight = 1.0;
		}

		public RedArmoire( Serial serial ) : base( serial )
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

	[Furniture]
	[Flipable( 0x285D, 0x285E )]
	public class CherryArmoire : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public CherryArmoire() : base( 0x285D )
		{
			Weight = 1.0;
		}

		public CherryArmoire( Serial serial ) : base( serial )
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

	[Furniture]
	[Flipable( 0x285B, 0x285C )]
	public class MapleArmoire : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public MapleArmoire() : base( 0x285B )
		{
			Weight = 1.0;
		}

		public MapleArmoire( Serial serial ) : base( serial )
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

	[Furniture]
	[Flipable( 0x2859, 0x285A )]
	public class ElegantArmoire : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public ElegantArmoire() : base( 0x2859 )
		{
			Weight = 1.0;
		}

		public ElegantArmoire( Serial serial ) : base( serial )
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

	[Furniture]
	[Flipable( 0xa97, 0xa99, 0xa98, 0xa9a, 0xa9b, 0xa9c )]
	public class FullBookcase : BaseContainer, ICanBeStained
	{
		[Constructable]
		public FullBookcase() : base( 0xA97 )
		{
			Weight = 1.0;
		}

		public FullBookcase( Serial serial ) : base( serial )
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

	[Furniture]
	[Flipable( 0xa9d, 0xa9e )]
	public class EmptyBookcase : BaseContainer, ICanBeStained
	{
		[Constructable]
		public EmptyBookcase() : base( 0xA9D )
		{
		}

		public EmptyBookcase( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( version == 0 && Weight == 1.0 )
				Weight = -1;
		}
	}

	[Furniture]
	[Flipable( 0xa2c, 0xa34 )]
	public class Drawer : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public Drawer() : base( 0xA2C )
		{
			Weight = 1.0;
		}

		public Drawer( Serial serial ) : base( serial )
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

	[Furniture]
	[Flipable( 0xa30, 0xa38 )]
	public class FancyDrawer : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public FancyDrawer() : base( 0xA30 )
		{
			Weight = 1.0;
		}

		public FancyDrawer( Serial serial ) : base( serial )
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

	[Furniture]
	[Flipable( 0xa4f, 0xa53 )]
	public class Armoire : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public Armoire() : base( 0xA4F )
		{
			Weight = 1.0;
		}

		public override void DisplayTo( Mobile m )
		{
			if ( DynamicFurniture.Open( this, m ) )
				base.DisplayTo( m );
		}

		public Armoire( Serial serial ) : base( serial )
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

			DynamicFurniture.Close( this );
		}
	}

	[Furniture]
	[Flipable( 0xa4d, 0xa51 )]
	public class FancyArmoire : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public FancyArmoire() : base( 0xA4D )
		{
			Weight = 1.0;
		}

		public override void DisplayTo( Mobile m )
		{
			if ( DynamicFurniture.Open( this, m ) )
				base.DisplayTo( m );
		}

		public FancyArmoire( Serial serial ) : base( serial )
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

			DynamicFurniture.Close( this );
		}
	}

	public class DynamicFurniture
	{
		private static Hashtable m_Table = new Hashtable();

		public static bool Open( Container c, Mobile m )
		{
			if ( m_Table.Contains( c ) )
			{
				c.SendRemovePacket();
				Close( c );
				c.Delta( ItemDelta.Update );
				c.ProcessDelta();
				return false;
			}

			if ( c is Armoire || c is FancyArmoire )
			{
				Timer t = new FurnitureTimer( c, m );
				t.Start();
				m_Table[c] = t;

				switch ( c.ItemID )
				{
					case 0xA4D: c.ItemID = 0xA4C; break;
					case 0xA4F: c.ItemID = 0xA4E; break;
					case 0xA51: c.ItemID = 0xA50; break;
					case 0xA53: c.ItemID = 0xA52; break;
				}
			}

			return true;
		}

		public static void Close( Container c )
		{
			Timer t = (Timer)m_Table[c];

			if ( t != null )
			{
				t.Stop();
				m_Table.Remove( c );
			}

			if ( c is Armoire || c is FancyArmoire )
			{
				switch ( c.ItemID )
				{
					case 0xA4C: c.ItemID = 0xA4D; break;
					case 0xA4E: c.ItemID = 0xA4F; break;
					case 0xA50: c.ItemID = 0xA51; break;
					case 0xA52: c.ItemID = 0xA53; break;
				}
			}
		}
	}

	public class FurnitureTimer : Timer
	{
		private Container m_Container;
		private Mobile m_Mobile;

		public FurnitureTimer( Container c, Mobile m ) : base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 0.5 ) )
		{
			Priority = TimerPriority.TwoFiftyMS;

			m_Container = c;
			m_Mobile = m;
		}

		protected override void OnTick()
		{
			if ( m_Mobile.Map != m_Container.Map || !m_Mobile.InRange( m_Container.GetWorldLocation(), 3 ) )
				DynamicFurniture.Close( m_Container );
		}
	}
	[Furniture]
	[Flipable( 0x17B5, 0x17B8 )]
	public class SideCabinet : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public SideCabinet() : base( 0x17B5 )
		{
			Weight = 1.0;
		}

		public SideCabinet( Serial serial ) : base( serial )
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
	[Flipable( 0x301, 0x302, 0x303, 0x304, 0x305, 0x306, 0x309, 0x30A, 0x30B, 0x30C, 0x30D, 0x30E, 0x30F, 0x310, 0x311, 0x312, 0x366, 0x367, 0x3A1, 0x3A2 )]
	public class WorkersBookcase : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
	[Constructable]
		public WorkersBookcase() : base( 0x301 )
		{
			Weight = 1.0;
		}

		public WorkersBookcase( Serial serial ) : base( serial )
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
