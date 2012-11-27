using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.Quests;

namespace Server.Items
{
	public class BankCheck : Item
	{
		private int m_Worth;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Worth
		{
			get{ return m_Worth; }
			set{ m_Worth = value; InvalidateProperties(); }
		}

		public BankCheck( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Worth );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			LootType = LootType.Blessed;

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Worth = reader.ReadInt();
					break;
				}
			}
		}

		[Constructable]
		public BankCheck( int worth ) : base( 0x14F0 )
		{
			Weight = 1.0;
			Hue = 0x34;

			m_Worth = worth;
		}

		public override bool DisplayLootType{ get{ return Core.AOS; } }

		public override int LabelNumber{ get{ return 1041361; } } // A bank check

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties( list );

			list.Add( 1060738, m_Worth.ToString() ); // value: ~1_val~
		}

		public override void OnSingleClick( Mobile from )
		{
			from.Send( new MessageLocalizedAffix( Serial, ItemID, MessageType.Label, 0x3B2, 3, 1041361, "", AffixType.Append, String.Concat( " ", m_Worth.ToString() ), "" ) ); // A bank check:
		}

		public override void OnDoubleClick( Mobile from )
		{
			BankBox box = from.BankBox;

			if ( box != null && IsChildOf( box ) )
			{
				Delete();

				int deposited = 0;

				int toAdd = m_Worth;

				Copper gold;

				while ( toAdd > 60000 )
				{
					gold = new Copper( 60000 );

					if ( box.TryDropItem( from, gold, false ) )
					{
						toAdd -= 60000;
						deposited += 60000;
					}
					else
					{
						gold.Delete();

						from.AddToBackpack( new BankCheck( toAdd ) );
						toAdd = 0;

						break;
					}
				}

				if ( toAdd > 0 )
				{
					gold = new Copper( toAdd );

					if ( box.TryDropItem( from, gold, false ) )
					{
						deposited += toAdd;
					}
					else
					{
						gold.Delete();

						from.AddToBackpack( new BankCheck( toAdd ) );
					}
				}

				// Gold was deposited in your account:
				from.SendMessage( "A total of " + deposited + " copper coins was deposited in your account." );

			}
			else
			{
				from.SendLocalizedMessage( 1047026 ); // That must be in your bank box to use it.
			}
		}
	}
}
