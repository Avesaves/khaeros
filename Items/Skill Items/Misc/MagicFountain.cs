/**************************************
*   Bloody+Enhanced Bandage System    *
*      Distro files: Bandage.cs       *
*                                     *
*     Made by Demortris AKA Joeku     *
*             10/11/2005              *
*                                     *
* Anyone can modify/redistribute this *
*  DO NOT REMOVE/CHANGE THIS HEADER!  *
**************************************/

using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Accounting;

namespace Server.Items
{
	[FlipableAttribute( 0x2AC0, 0x2AC3 )] 
	public class MagicalFountain : Item
	{
		public MagicalFountain() : base( 0x2AC0 )
		{
			Name = "a magical fountain";
		}

		public MagicalFountain ( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public override void OnDoubleClick ( Mobile from )
		{
			if ( from.InRange( GetWorldLocation(), Core.AOS ? 2 : 1 ) )
			{
				from.RevealingAction();

				Item bandage = from.Backpack.FindItemByType( typeof( Bandage ) );

				if(bandage != null)
				{
					int amount = bandage.Amount;
					from.SendMessage("You enhance all of your bandages and drop them into your pack.");
					from.Backpack.ConsumeTotal( typeof(Bandage),amount );
					from.AddToBackpack( new EnhancedBandage( amount ) );
				}
				return;
			}
			else
			{
				from.SendLocalizedMessage ( 500295 ); // You are too far away to do that.
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( from.Backpack != null )
			{
				if( dropped is Bandage )
         			{
					int amount = dropped.Amount;
					from.SendMessage("You enhance all of your bandages and drop them into your pack.");
					dropped.Amount = 1;
					dropped.Consume();
					from.AddToBackpack( new EnhancedBandage( amount ) );
				}
				else if( dropped is EnhancedBandage )
         			{
					from.SendMessage("Those have already been enhanced.");
				}
				else
				{
					from.SendMessage("You can only enhance bandages.");
				}
				return false;
			}
			else
			{
				from.SendMessage("You don't have a backpack to hold the enhanced bandages! Begone!");
				return false;
			}
		}
	}
}
