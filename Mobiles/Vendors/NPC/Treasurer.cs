using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Engines.BulkOrders;

namespace Server.Mobiles
{
	public class Treasurer : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		private Container m_Treasury;
		public override bool IsActiveBuyer{ get{ return true; } }
		public override bool IsActiveSeller{ get{ return true; } }
		private VendorEntry m_NewBuyInfo = new VendorEntry();
		private VendorEntry m_NewSellInfo = new VendorEntry();
        Nation obsoleteNation = Nation.None; // obselete as of version 4
		private ArrayList m_CustomSellList = new ArrayList();
		private ArrayList m_CustomBuyList = new ArrayList();
		
		[CommandProperty( AccessLevel.GameMaster )]
		public ArrayList CustomSellList{ get{ return m_CustomSellList; } set{ m_CustomSellList = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public ArrayList CustomBuyList{ get{ return m_CustomBuyList; } set{ m_CustomBuyList = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public VendorEntry NewBuyInfo{ get{ return m_NewBuyInfo; } set{ m_NewBuyInfo = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public VendorEntry NewSellInfo{ get{ return m_NewSellInfo; } set{ m_NewSellInfo = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool UpdateSellList{ get{ return false; } set{ UpdateList( m_NewSellInfo, m_CustomSellList ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool UpdateBuyList{ get{ return false; } set{ UpdateList( m_NewBuyInfo, m_CustomBuyList ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool CheckSellList{ get{ return false; } set{ CheckList( m_CustomSellList ); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool CheckBuyList{ get{ return false; } set{ CheckList( m_CustomBuyList ); } }
		
		public Container Treasury
		{
			get{ return m_Treasury; }
			set{ m_Treasury = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Treasurer SendInfosTo
		{
			get{ return null; }
			set
			{
				if( value != null && value is Treasurer )
				{
					Treasurer treasurer = value as Treasurer;
					treasurer.CustomBuyList.Clear();
					treasurer.CustomSellList.Clear();

					for( int i = 0; i < CustomBuyList.Count; i++ )
					{
						VendorEntry entry = (VendorEntry)CustomBuyList[i];
						treasurer.CustomBuyList.Add( new VendorEntry( entry.ItemName, entry.TypeName, entry.Price, entry.Type ) );
					}
					
					for( int i = 0; i < CustomSellList.Count; i++ )
					{
						VendorEntry entry = (VendorEntry)CustomSellList[i];
						treasurer.CustomSellList.Add( new VendorEntry( entry.ItemName, entry.TypeName, entry.Price, entry.Type ) );
					}
				}
			}
		}
		
		protected override ArrayList SBInfos{ get { return m_SBInfos; } }

		[Constructable]
		public Treasurer() : base( "the treasurer" )
		{
			FixBackpack();
		}
		
		private void CheckList( ArrayList list )
		{
			for( int i = 0; i < list.Count; i++ )
			{
				VendorEntry entry = (VendorEntry)list[i];
				string toSay = "" + entry.ItemName + " (" + entry.TypeName.ToLower() + ") for " + entry.Price.ToString() + " copper coins.";
				this.Say( toSay );
			}
		}
		
		private void UpdateList( VendorEntry entry, ArrayList list )
		{
			if( entry.TypeName == null || entry.TypeName.Length < 1 || ( !entry.Remove && (entry.Price < 1 || entry.ItemName == null || entry.ItemName.Length < 1) ) )
			{
				this.Say( "Insufficient information." );
				return;
			}

			entry.Type = StringToType( entry.TypeName );
			
			if( entry.Type == null )
			{
				this.Say( "Invalid type." );
				return;
			}
			
			bool found = false;
			
			for( int i = 0; i < list.Count; i++ )
			{
				VendorEntry oldEntry = (VendorEntry)list[i];
				
				if( oldEntry.Type == entry.Type )
				{
					list.Remove( oldEntry );
					
					if( entry.Remove )
						this.Say( "Entry removed successfully." );
					
					else
					{
						list.Add( new VendorEntry( entry.ItemName, entry.TypeName, entry.Price, entry.Type ) );
						this.Say( "Entry replaced successfully." );
					}
					
					found = true;
					break;
				}
			}
			
			if( !found )
			{
				if( entry.Remove )
					this.Say( "Chosen type could not be removed because it was not in the list." );
				
				else
				{
					list.Add( new VendorEntry( entry.ItemName, entry.TypeName, entry.Price, entry.Type ) );
					this.Say( "New entry added successfully." );
				}
			}
		}
		
		private void FixBackpack()
		{
			if( this.Backpack != null )
				this.Backpack.Delete();
			
			BackpackOfHolding pack = new BackpackOfHolding();
			this.AddItem( pack );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBEmpty() );
		}
		
		public override void VendorSell( Mobile from )
		{
			if ( !IsActiveBuyer )
				return;

			if ( !from.CheckAlive() )
				return;

			if ( !CheckVendorAccess( from ) )
			{
				Say( 501522 ); //I shall not treat with scum like thee!
				return;
			}

			Container pack = from.Backpack;

			if ( pack != null )
			{
				VendorEntry[] info = (VendorEntry[])m_CustomSellList.ToArray( typeof( VendorEntry ) );

				Hashtable table = new Hashtable();

				foreach ( VendorEntry entry in info )
				{
					Item[] items = pack.FindItemsByType( entry.Type );

					foreach ( Item item in items )
					{
						if ( item is Container && ((Container)item).Items.Count != 0 )
							continue;

						if ( item.IsStandardLoot() && item.Movable )
							table[item] = new SellItemState( item, entry.Price, item.Name );
					}
				}

				if ( table.Count > 0 )
				{
					SendPacksTo( from );

					from.Send( new VendorSellList( this, table ) );
				}
				else
				{
					Say( true, "You have nothing I would be interested in." );
				}
			}
		}
		
		public override void VendorBuy( Mobile from )
		{
			if ( !IsActiveSeller )
				return;

			if ( !from.CheckAlive() )
				return;

			if ( !CheckVendorAccess( from ) )
			{
				Say( 501522 ); //I shall not treat with scum like thee!
				return;
			}

			int count = 0;
			ArrayList list;
			VendorEntry[] buyInfo = (VendorEntry[])m_CustomBuyList.ToArray( typeof( VendorEntry ) );

			list = new ArrayList( buyInfo.Length );
			
			Container cont = this.BuyPack;
			
			ArrayList opls = new ArrayList();

			List<Item> playerItems = cont.Items;

			for( int i = 0; i < playerItems.Count; ++i )
			{
				Item item = playerItems[i];

				int price = 0;
				string name = null;

				foreach( VendorEntry entry in buyInfo )
				{
					if ( item.GetType() == entry.Type )
					{
						price = entry.Price;
						name = entry.ItemName;
						break;
					}
				}

				if ( name != null && list.Count < 250 )
				{
					list.Add( new BuyItemState( name, cont.Serial, item.Serial, price, item.Amount, item.ItemID, item.Hue ) );
					count++;

					opls.Add( item.PropertyList );
				}
			}

			if ( list.Count > 0 )
			{
				list.Sort( new BuyItemStateComparer() );

				SendPacksTo( from );
				
				from.Send( new VendorBuyContent( list ) );
				from.Send( new VendorBuyList( this, list ) );
				from.Send( new DisplayBuyList( this ) );
				from.Send( new MobileStatusExtended( from ) ); //make sure their copper amount is sent

				for ( int i = 0; i < opls.Count; ++i )
					from.Send( opls[i] as Packet );

				SayTo( from, "Greetings. Have a look around." );
			}
		}

		public override bool OnBuyItems( Mobile buyer, ArrayList list )
		{
			if ( !IsActiveSeller )
				return false;

			if ( !buyer.CheckAlive() )
				return false;

			if ( !CheckVendorAccess( buyer ) )
			{
				Say( 501522 ); //I shall not treat with scum like thee!
				return false;
			}

			VendorEntry[] buyInfo = (VendorEntry[])m_CustomBuyList.ToArray( typeof( VendorEntry ) );
			int totalCost = 0;
			ArrayList validBuy = new ArrayList( list.Count );
			Container cont;
			bool bought = false;
			bool fromBank = false;
			bool fullPurchase = true;
			int controlSlots = buyer.FollowersMax - buyer.Followers;

			foreach ( BuyItemResponse buy in list )
			{
				Serial ser = buy.Serial;
				int amount = buy.Amount;

				if ( ser.IsItem )
				{
					Item item = World.FindItem( ser );

					if ( item == null )
					{
						this.Say( "I am sorry. I could not find in the stock the item you wish to buy." );
						continue;
					}

					if ( item.RootParent == this )
					{
						if ( amount > item.Amount )
							amount = item.Amount;

						if ( amount <= 0 )
							continue;

						foreach ( VendorEntry entry in buyInfo )
						{
							if ( entry.Type == item.GetType() )
							{
								totalCost += entry.Price * amount;
								validBuy.Add( buy );
								break;
							}
						}
					}
				}
			}

			if ( fullPurchase && validBuy.Count == 0 )
				SayTo( buyer, 500190 ); //Thou hast bought nothing!
			else if ( validBuy.Count == 0 )
				SayTo( buyer, 500187 ); //Your order cannot be fulfilled, please try again.

			if ( validBuy.Count == 0 )
				return false;

			bought = ( buyer.AccessLevel >= AccessLevel.GameMaster );

			cont = buyer.Backpack;
			if ( !bought && cont != null )
			{
				if ( cont.ConsumeTotal( typeof( Copper ), totalCost ) )
					bought = true;
				else if ( totalCost < 2000 )
					SayTo( buyer, 500192 ); //Begging thy pardon, but thou casnt afford that.
			}

			if ( !bought && totalCost >= 2000 )
			{
				cont = buyer.BankBox;
				if ( cont != null && cont.ConsumeTotal( typeof( Copper ), totalCost ) )
				{
					bought = true;
					fromBank = true;
				}
				else
				{
					SayTo( buyer, 500191 ); //Begging thy pardon, but thy bank account lacks these funds.
				}
			}

			if ( !bought )
				return false;
			else
				buyer.PlaySound( 0x32 );

			cont = buyer.Backpack;
			if ( cont == null )
				cont = buyer.BankBox;

			foreach ( BuyItemResponse buy in validBuy )
			{
				Serial ser = buy.Serial;
				int amount = buy.Amount;

				if ( amount < 1 )
					continue;

				if ( ser.IsItem )
				{
					Item item = World.FindItem( ser );

					if ( item == null )
						continue;

					GenericBuyInfo gbi = LookupDisplayObject( item );

					if ( gbi != null )
					{
						ProcessValidPurchase( amount, gbi, buyer, cont );
					}
					else
					{
						if ( amount > item.Amount )
							amount = item.Amount;

						foreach ( VendorEntry entry in buyInfo )
						{
							if ( item.GetType() == entry.Type )
							{
								Item buyItem;
								if ( amount >= item.Amount )
								{
									buyItem = item;
								}
								else
								{
									buyItem = Mobile.LiftItemDupe( item, item.Amount - amount );

									if ( buyItem == null )
										buyItem = item;
								}

								if ( cont == null || !cont.TryDropItem( buyer, buyItem, false ) )
									buyItem.MoveToWorld( buyer.Location, buyer.Map );

								break;
							}
						}
					}
				}
			}
			
			if( buyer.AccessLevel == AccessLevel.Player )
			{
				cont = this.BuyPack;
						
				if( this is Treasurer && ( (Treasurer)this ).Nation != Nation.None && ( ( ( (Treasurer)this ).Treasury != null && !( (Treasurer)this ).Treasury.Deleted ) || ( (Treasurer)this ).AssignTreasury() ) )
				{
					cont = ( (Treasurer)this ).Treasury;
				}
				
				Copper copper = new Copper();
				copper.Amount = totalCost;
				
				if( cont is BaseContainer )
					( (BaseContainer)cont ).DropAndStack( copper );
			}

			if ( fullPurchase )
			{
				if ( buyer.AccessLevel >= AccessLevel.GameMaster )
					SayTo( buyer, true, "I would not presume to charge thee anything.  Here are the goods you requested." );
				else if ( fromBank )
					SayTo( buyer, true, "The total of thy purchase is {0} copper, which has been withdrawn from your bank account.  My thanks for the patronage.", totalCost );
				else
					SayTo( buyer, true, "The total of thy purchase is {0} copper.  My thanks for the patronage.", totalCost );
			}
			else
			{
				if ( buyer.AccessLevel >= AccessLevel.GameMaster )
					SayTo( buyer, true, "I would not presume to charge thee anything.  Unfortunately, I could not sell you all the goods you requested." );
				else if ( fromBank )
					SayTo( buyer, true, "The total of thy purchase is {0} copper, which has been withdrawn from your bank account.  My thanks for the patronage.  Unfortunately, I could not sell you all the goods you requested.", totalCost );
				else
					SayTo( buyer, true, "The total of thy purchase is {0} copper.  My thanks for the patronage.  Unfortunately, I could not sell you all the goods you requested.", totalCost );
			}

			return true;
		}
		
		public override bool OnSellItems( Mobile seller, ArrayList list )
		{
			if ( !IsActiveBuyer )
				return false;

			if ( !seller.CheckAlive() )
				return false;

			if ( !CheckVendorAccess( seller ) )
			{
				Say( 501522 ); // I shall not treat with scum like thee!
				return false;
			}

			VendorEntry[] info = (VendorEntry[])m_CustomSellList.ToArray( typeof( VendorEntry ) );
			int GiveCopper = 0;
			int Sold = 0;
			Container cont;
			ArrayList delete = new ArrayList();
			ArrayList drop = new ArrayList();
			int vendorcopper = 0;
			
			if( this.Backpack != null )
			{
				foreach( Item item in this.Backpack.Items )
				{
					if( item is Copper )
						vendorcopper += item.Amount;
				}
			}
			
			else
			{
				Backpack backpack = new Backpack();
				this.AddItem( backpack );
			}

			foreach ( SellItemResponse resp in list )
			{
				if ( resp.Item.RootParent != seller || resp.Amount <= 0 || !resp.Item.IsStandardLoot() || !resp.Item.Movable || (resp.Item is Container && ((Container)resp.Item).Items.Count != 0) )
					continue;

				foreach( VendorEntry entry in info )
				{
					if ( resp.Item.GetType() == entry.Type )
					{
						Sold++;
						break;
					}
				}
			}

			if ( Sold > MaxSell )
			{
				SayTo( seller, true, "You may only sell {0} items at a time!", MaxSell );
				return false;
			} 
			else if ( Sold == 0 )
			{
				return true;
			}

			foreach ( SellItemResponse resp in list )
			{
				if ( resp.Item.RootParent != seller || resp.Amount <= 0 || !resp.Item.IsStandardLoot() || !resp.Item.Movable || (resp.Item is Container && ((Container)resp.Item).Items.Count != 0) )
					continue;

				foreach( VendorEntry entry in info )
				{
					if ( resp.Item.GetType() == entry.Type )
					{
						int amount = resp.Amount;

						if ( amount > resp.Item.Amount )
							amount = resp.Item.Amount;
						
						GiveCopper += entry.Price * amount;
						break;
					}
				}
			}

			if( GiveCopper > vendorcopper )
			{
				this.SayTo( seller, "I apologize, but I do not have that much copper." );
				return false;
			}
			
			foreach ( SellItemResponse resp in list )
			{
				if ( resp.Item.RootParent != seller || resp.Amount <= 0 || !resp.Item.IsStandardLoot() || !resp.Item.Movable || (resp.Item is Container && ((Container)resp.Item).Items.Count != 0) )
					continue;

				foreach( VendorEntry entry in info )
				{
					if ( resp.Item.GetType() == entry.Type )
					{
						int amount = resp.Amount;

						if ( amount > resp.Item.Amount )
							amount = resp.Item.Amount;

						cont = this.BuyPack;
						
						if( this is Treasurer && ( (Treasurer)this ).Nation != Nation.None && ( ( ( (Treasurer)this ).Treasury != null && !( (Treasurer)this ).Treasury.Deleted ) || ( (Treasurer)this ).AssignTreasury() ) )
							cont = ( (Treasurer)this ).Treasury;

						if ( amount < resp.Item.Amount )
						{
							Item item = Mobile.LiftItemDupe( resp.Item, resp.Item.Amount - amount );

							if ( item != null )
							{
								item.SetLastMoved();
								if( cont is BaseContainer )
									( (BaseContainer)cont ).DropAndStack( item );
							}
							else
							{
								resp.Item.SetLastMoved();
								if( cont is BaseContainer )
									( (BaseContainer)cont ).DropAndStack( resp.Item );
							}
						}
						else
						{
							resp.Item.SetLastMoved();
							if( cont is BaseContainer )
								( (BaseContainer)cont ).DropAndStack( resp.Item );
						}
					}
				}
			}
			
			this.Backpack.ConsumeTotal( typeof( Copper ), GiveCopper );
			
			if ( GiveCopper > 0 )
			{
				while ( GiveCopper > 60000 )
				{
					seller.AddToBackpack( new Copper( 60000 ) );
					GiveCopper -= 60000;
				}

				seller.AddToBackpack( new Copper( GiveCopper ) );
				seller.PlaySound( 0x0037 ); //Copper dropping sound
			}
			
			return true;
		}
		
		#region Serialization
		public Treasurer( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			if( m_CustomBuyList == null )
				m_CustomBuyList = new ArrayList();
			
			if( m_CustomSellList == null )
				m_CustomSellList = new ArrayList();

			writer.Write( (int) 4 ); // version
			writer.Write( (Item) m_Treasury );
			writer.Write( (int) m_CustomSellList.Count );
			writer.Write( (int) m_CustomBuyList.Count );
			
			for( int i = 0; i < m_CustomSellList.Count; i++ )
			{
				VendorEntry entry = m_CustomSellList[i] as VendorEntry;
				VendorEntry.Serialize( writer, entry );
			}
			
			for( int i = 0; i < m_CustomBuyList.Count; i++ )
			{
				VendorEntry entry = m_CustomBuyList[i] as VendorEntry;
				VendorEntry.Serialize( writer, entry );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			m_CustomSellList = new ArrayList();
			m_CustomBuyList = new ArrayList();
			
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Treasury = (Container)reader.ReadItem();
			if ( version < 4) obsoleteNation = (Nation)reader.ReadInt();

			int sellcount = reader.ReadInt();
			int buycount = reader.ReadInt();
			
			for( int i = 0; i < sellcount; i++ )
			{
				VendorEntry entry = new VendorEntry( reader );
				entry.Type = StringToType( entry.TypeName );
				
				if( entry.Type != null )
					m_CustomSellList.Add( entry );
			}
			
			for( int i = 0; i < buycount; i++ )
			{
				VendorEntry entry = new VendorEntry( reader );
				entry.Type = StringToType( entry.TypeName );
				
				if( entry.Type != null )
					m_CustomBuyList.Add( entry );
			}
			
			m_NewSellInfo = new VendorEntry();
			m_NewBuyInfo = new VendorEntry();
		}
		#endregion
		
		#region Utilities
		public Type StringToType( string toType )
		{
			Type t = ScriptCompiler.FindTypeByName( toType );
			
			return t;
		}
		
		public bool AssignTreasury()
		{
			foreach( Item item in World.Items.Values )
			{
                if (item is Treasury && (item as Treasury).Nation == this.Nation)
                {
                    this.Treasury = item as Container;
                    return true;
                }
			}
			
			return false;
		}
		#endregion
	}
}
