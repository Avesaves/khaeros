using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class SheathedItem : Container
	{
		public static void Sheathe( Mobile mobile, bool rightHand, bool onBack )
		{
			if ( onBack && mobile.FindItemOnLayer( Layer.Talisman ) != null )
			{
				mobile.SendMessage( "You have something sheathed behind your back already." );
				return;
			}
			
			else if ( !onBack && mobile.FindItemOnLayer( Layer.Waist ) != null )
			{
				mobile.SendMessage( "You have something sheathed on your waist already." );
				return;
			}
			
			Item item;
			if ( rightHand )
				item = mobile.FindItemOnLayer( Layer.OneHanded );
			else
				item = mobile.FindItemOnLayer( Layer.TwoHanded );
			
			BaseWeapon weapon = item as BaseWeapon;
			BaseShield shield = item as BaseShield;
			if ( item == null )
			{
				mobile.SendMessage( "You aren't holding anything in that hand." );
				return;
			}
			
			if ( !item.Movable )
				return;
			
			int itemID=-1;
			if ( weapon == null )
			{
				if ( shield == null )
				{
					mobile.SendMessage( "You can't sheathe that." );
					return;
				}
				else
				{
					if (!onBack)
					{
						mobile.SendMessage( "That cannot be sheathed there." );
						return;
					}
					itemID = (mobile.Female ? shield.SheathedFemaleBackID : shield.SheathedMaleBackID);
					if ( itemID == -1 )
					{
						mobile.SendMessage( "You can't sheathe that." );
						return;
					}
				}
			}
			else if ( ( onBack && (itemID=(mobile.Female ? weapon.SheathedFemaleBackID : weapon.SheathedMaleBackID)) == -1 ) || ( !onBack && (itemID=(mobile.Female ? weapon.SheathedFemaleWaistID : weapon.SheathedMaleWaistID)) == -1 ) )
			{
				mobile.SendMessage( "That weapon cannot be sheathed there." );
				return;
			}
			
			if ( onBack && !(mobile.Backpack.ConsumeTotal( typeof(EmptyBackSheath), 1 )) )
			{
				mobile.SendMessage( "You need an empty back sheath." );
				return;
			}
			else if ( !onBack && !(mobile.Backpack.ConsumeTotal( typeof(EmptyWaistSheath), 1 )) )
			{
				mobile.SendMessage( "You need an empty waist sheath." );
				return;
			}
			
			SheathedItem sheathed = new SheathedItem( itemID, onBack );
			sheathed.Hue = item.Hue;
			sheathed.AddItem( item );
			item.Movable = false;
			if ( shield != null )
				sheathed.Name = "sheathed shield";
			mobile.EquipItem( sheathed );
			mobile.PlaySound( 89 );
		}
		[Constructable]
		public SheathedItem( int itemID, bool onback ) : base( itemID )
		{
			Weight = 1.0;
			if ( onback )
			{
				Layer = Layer.Talisman;
				Name = "back sheath";
			}
			else
			{
				Layer = Layer.Waist;
				Name = "waist sheath";
			}
		}

		public SheathedItem( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick ( Mobile from )
		{
			if ( RootParent == from )
				Unsheathe( from );
		}
		
		public void Unsheathe( Mobile from )
		{
			Item item = FindItemByType( typeof( Item ) );
			if ( item == null )
			{
				Delete();
				return;
			}
			if ( from.FindItemOnLayer( item.Layer ) != null )
			{
				from.SendMessage( "Your hands are full!" );
				return;
			}
			else
			{
				if ( Layer == Layer.Waist )
					from.AddToBackpack( new EmptyWaistSheath() );
				else
					from.AddToBackpack( new EmptyBackSheath() );
				item.Movable = true;
				from.AddToBackpack( item );
				from.EquipItem( item );
				if ( item is BaseWeapon )
				{
					if ( item is BaseSword || item is BaseKnife )
						from.PlaySound( 86 ); // metal
					else
						from.PlaySound( 89 ); // leathery
					from.SendMessage( "You unsheathe your weapon." );
				}
				else
				{
					from.PlaySound( 89 );
					from.SendMessage( "You unsheathe your shield" );
				}
				Delete();
			}
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
