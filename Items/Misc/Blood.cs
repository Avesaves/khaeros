using System;
using Server;

namespace Server.Items
{
	public class Blood : Item
	{
		[Constructable]
		public Blood() : this( 0x1645 )
		{
		Hue = Utility.RandomList(32,0,2771,2652,2600,2604);
		
		
			switch ( Utility.Random( 8 ))
			{
				case 0: ItemID = 4651; break;
				case 1: ItemID = 4652; break;
				case 2: ItemID = 4654; break;
				case 3: ItemID = 5701; break;
				case 4: ItemID = 5701; break;
				case 5: ItemID = 5701; break;
				case 6: ItemID = 5701; break;
				case 7: ItemID = 7406; break;
				
				
			}
		}

		[Constructable]
		public Blood( int itemID ) : base( itemID )
		{
			Movable = false;

			new InternalTimer( this ).Start();
		}

		public Blood( Serial serial ) : base( serial )
		{
			new InternalTimer( this ).Start();
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

		private class InternalTimer : Timer
		{
			private Item m_Blood;

			public InternalTimer( Item blood ) : base( TimeSpan.FromSeconds( 10 ) )
			{
				Priority = TimerPriority.OneSecond;

				m_Blood = blood;
			}

			protected override void OnTick()
			{
				m_Blood.Delete();
			}
		}
	}
}