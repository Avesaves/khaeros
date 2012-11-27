using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
	public class DonationBox : LockableContainer
	{
		[Constructable]
		public DonationBox() : base( 0xE7D )
		{
			Weight = 4.0;
			Movable = false;
			Locked = true;
			LockLevel = 150;
			RequiredSkill = 150;
			Key key = new Key();
			uint keyvalue = Key.RandomValue();
			key.KeyValue = keyvalue;
			KeyValue = keyvalue;
			DropItem( key );
			Name = "Donation Box";
		}

		public DonationBox( Serial serial ) : base( serial )
		{
		}
		
		public override bool CheckHold( Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			return true;
		}
		
		public override bool TryDropItem( Mobile from, Item dropped, bool sendFullMessage )
		{
			bool success = false;
			bool notEnough = false;
			if( from is PlayerMobile )
			{
				PlayerMobile m = from as PlayerMobile;
				if ( dropped is Copper )
				{
					success = true;
					if ( dropped.Amount < 1000 )
						notEnough = true;
				}
				else if ( dropped is Silver )
				{
					success = true;
					if ( dropped.Amount < 100 )
						notEnough = true;
				}
				else if ( dropped is Gold )
				{
					success = true;
					if ( dropped.Amount < 10 )
						notEnough = true;
				}
				
				if ( success )
				{
					if( m.Class == Class.Mage )
						m.SendMessage( "Your class cannot regenerate lost life points." );
					else if( m.IsVampire )
						m.SendMessage( "Vampires cannot regenerate lost life points." );
					else if ( notEnough )
						m.SendMessage( "You donate the coins, but the measly amount does not grant you any benefits." );
					else
					{
						if( DateTime.Compare( DateTime.Now, ( m.LastDonationLife + TimeSpan.FromDays( 30 ) ) ) > 0 )
						{
							m.LastDonationLife = DateTime.Now;
							
							if( m.Age < m.MaxAge )
							{
								if ( m.Level <= m.Lives )
									m.SendMessage( "You donate the coins, but since your level exceeds your life points, you cannot gain any." );
								else
								{
									m.Lives++;
									m.SendMessage( "You donate the coins and gain a life point in return." );
								}
							}
							else
								m.SendMessage( "You donate the coins, but you are too old to gain any more life points." );
						}
						else
							m.SendMessage( "You donate the coins, but you cannot gain any more life points at the moment." );
					}
				}
				else
					m.SendMessage( "It would be very inappropriate to leave this junk inside a donation box." );
			}

			if ( success )
			{
				dropped.Delete();
				return true;
			}
			else
				return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
			{
				Locked = true;
				LockLevel = 150;
				RequiredSkill = 150;
			}
		}
	}
}
