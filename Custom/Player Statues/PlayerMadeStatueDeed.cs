using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Regions;
using Server.Multis;

namespace Server.Items
{

	[Flipable( 0x14F0, 0x14EF )]
	public class PlayerMadeStatueDeed : Item
	{
		[Constructable]
		public PlayerMadeStatueDeed() : base( 0x14F0 )
		{
			Name = "A Character Statue Deed";
			LootType = LootType.Blessed;
		}

		public PlayerMadeStatueDeed( Serial serial ) : base( serial )
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
		
		private class StatueTarget : Target
        {
			private PlayerMadeStatueDeed m_deed;
			
            public StatueTarget( PlayerMobile pm, PlayerMadeStatueDeed deed )
                : base( 8, false, TargetFlags.None )
            {
            	m_deed = deed;
                pm.SendMessage( 60, "Whom do you wish to create a statue of?" );
            }

            protected override void OnTarget( Mobile from, object obj )
            {
                PlayerMobile model = obj as PlayerMobile;
                Mobile creature = obj as Mobile;
                
                BaseHouse house = BaseHouse.FindHouseAt(from);
                BaseHouse house2 = null;
                	
            	if( obj is PlayerMobile )
            	{
            		house2 = BaseHouse.FindHouseAt(from);
            	}
            	
            	if( !( obj is Mobile ) )
        	    {
            		from.SendMessage( "Invalid target." );
            		return;
        	    }

            	if( from.AccessLevel >= AccessLevel.GameMaster || 
            	   ( house != null && house.IsOwner( from ) ) || ( house != null && house.IsCoOwner( from ) ) || 
            	   ( obj is PlayerMobile && house2 != null && house2.IsOwner( model ) ) || ( obj is PlayerMobile && house2 != null && house2.IsCoOwner( model ) ) )
				{
					PlayerMadeStatue m = new PlayerMadeStatue( from );
					m.Map = from.Map;
					m.Location = from.Location;
					m.Direction = from.Direction;
					m_deed.Delete();

					m.Material = StatueMaterial.BronzeX1;
					m.HasPlinth = true;

					m.Name = creature.Name;
					m.Body = creature.Body;
					m.Female = creature.Female;
					m.HairItemID = creature.HairItemID;
					m.HairHue = creature.HairHue;
					m.FacialHairItemID = creature.FacialHairItemID;
					m.FacialHairHue = creature.FacialHairHue;
				
              		ArrayList items = new ArrayList( creature.Items );
              		for (int i=0; i<items.Count; i++)
              		{
             			Item item = (Item)items[i]; 
             			if((( item != null ) && ( item.Parent == creature ) && ( item != creature.Backpack ) && (item != creature.BankBox)))
             			//if((( item != null ) && ( item.Parent == from )))
             			{
                			Type type = item.GetType();
                			Item newitem = Loot.Construct( type );
							//newitem.ItemID = item.ItemID;
                        			//CopyProperties( newitem, item );
							if (newitem != null) 
							{
 								//CopyProperties( newitem, item );
 								newitem.LootType = LootType.Blessed;
						
                        				newitem.Parent = null;
                        				m.AddItem( newitem );
							}
             			}
              		}
				}

        		else
        		{
            		from.SendMessage( "You must be in your house or in your model's house to do this." );
        		}
            }
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile m = from as PlayerMobile;
			
			if ( IsChildOf( from.Backpack ) )
			{
				if( from is PlayerMobile && m.Feats.GetFeatLevel(FeatList.Sculptor) > 2 )
				{
                	m.Target = new StatueTarget( m, this );
				}
				else
					from.SendMessage( "You need to reach the third level of the Sculptor feat in order to do that." );
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}
	}
}
