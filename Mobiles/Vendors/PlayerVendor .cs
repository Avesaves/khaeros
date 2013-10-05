using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Prompts;
using Server.Targeting;
using Server.Misc;
using Server.Multis;
using Server.ContextMenus;
using Server.TimeSystem;

namespace Server.Mobiles
{
	[AttributeUsage( AttributeTargets.Class )]
	public class PlayerVendorTargetAttribute : Attribute
	{
		public PlayerVendorTargetAttribute()
		{
		}
	}

	public class VendorItem
	{
		private Item m_Item;
		private int m_Price;
		private string m_Description;
		private DateTime m_Created;

		private bool m_Valid;

		public Item Item{ get{ return m_Item; } }
		public int Price{ get{ return m_Price; } }        

		public string Description
		{
			get{ return m_Description; }
			set
			{
				if ( value != null )
					m_Description = value;
				else
					m_Description = "";

				if ( Valid )
					Item.InvalidateProperties();
			}
		}

		public DateTime Created{ get{ return m_Created; } }

		public bool IsForSale{ get{ return Price >= 0; } }
		public bool IsForFree{ get{ return Price == 0; } }

		public bool Valid{ get{ return m_Valid; } }

		public VendorItem( Item item, int price, string description, DateTime created )
		{
			m_Item = item;
			m_Price = price;

			if ( description != null )
				m_Description = description;
			else
				m_Description = "";

			m_Created = created;

			m_Valid = true;
		}

		public void Invalidate()
		{
			m_Valid = false;
		}
	}

	public class VendorBackpack : Backpack
	{
		public VendorBackpack()
		{
			Layer = Layer.Backpack;
			Weight = 1.0;
		}

		public override int DefaultMaxWeight{ get{ return 0; } }

		public override bool CheckHold( Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			//if ( !base.CheckHold( m, item, message, checkItems, plusItems, plusWeight ) )
				//return false;

			if ( Ethics.Ethic.IsImbued( item, true ) )
			{
				if ( message )
					m.SendMessage( "Imbued items may not be sold here." );

				return false;
			}

			if ( !BaseHouse.NewVendorSystem && Parent is PlayerVendor )
			{
				BaseHouse house = ((PlayerVendor)Parent).House;

				if ( house != null && house.IsAosRules && !house.CheckAosStorage( 1 + item.TotalItems + plusItems ) )
				{
					if ( message )
						m.SendLocalizedMessage( 1061839 ); // This action would exceed the secure storage limit of the house.

					return false;
				}
			}

			return true;
		}

		public override bool IsAccessibleTo( Mobile m )
		{
			return true;
		}

		public override bool CheckItemUse( Mobile from, Item item )
		{
			if ( !base.CheckItemUse( from, item ) )
				return false;

			if ( item is Container || item is Engines.BulkOrders.BulkOrderBook )
				return true;

			from.SendLocalizedMessage( 500447 ); // That is not accessible.
			return false;
		}

		public override bool CheckTarget( Mobile from, Target targ, object targeted )
		{
			if ( !base.CheckTarget( from, targ, targeted ) )
				return false;

			if ( from.AccessLevel >= AccessLevel.GameMaster )
				return true;

			return targ.GetType().IsDefined( typeof( PlayerVendorTargetAttribute ), false );
		}

		public override void GetChildContextMenuEntries( Mobile from, List<ContextMenuEntry> list, Item item )
		{
			base.GetChildContextMenuEntries( from, list, item );

			PlayerVendor pv = RootParent as PlayerVendor;

			if ( pv == null || pv.IsOwner( from ) )
				return;

			VendorItem vi = pv.GetVendorItem( item );

			if ( vi != null )
				list.Add( new BuyEntry( item ) );
		}

		private class BuyEntry : ContextMenuEntry
		{
			private Item m_Item;

			public BuyEntry( Item item ) : base( 6103 )
			{
				m_Item = item;
			}

			public override bool NonLocalUse{ get{ return true; } }

			public override void OnClick()
			{
				if ( m_Item.Deleted )
					return;

				PlayerVendor.TryToBuy( m_Item, Owner.From );
			}
		}

		public override void GetChildNameProperties( ObjectPropertyList list, Item item )
		{
			base.GetChildNameProperties( list, item );

			PlayerVendor pv = RootParent as PlayerVendor;

			if ( pv == null )
				return;

			VendorItem vi = pv.GetVendorItem( item );

			if ( vi == null )
				return;

			if ( !vi.IsForSale )
				list.Add( 1043307 ); // Price: Not for sale.
			else if ( vi.IsForFree )
				list.Add( 1043306 ); // Price: FREE!
			else
				list.Add( 1043304, vi.Price.ToString() ); // Price: ~1_COST~
		}

		public override void GetChildProperties( ObjectPropertyList list, Item item )
		{
			base.GetChildProperties( list, item );

			PlayerVendor pv = RootParent as PlayerVendor;

			if ( pv == null )
				return;

			VendorItem vi = pv.GetVendorItem( item );

			if ( vi != null && vi.Description != null && vi.Description.Length > 0 )
				list.Add( 1043305, vi.Description ); // <br>Seller's Description:<br>"~1_DESC~"
		}

		public override void OnSingleClickContained( Mobile from, Item item )
		{
			if ( RootParent is PlayerVendor )
			{
				PlayerVendor vendor = (PlayerVendor)RootParent;

				VendorItem vi = vendor.GetVendorItem( item );

				if ( vi != null )
				{
					if ( !vi.IsForSale )
						item.LabelTo( from, 1043307 ); // Price: Not for sale.
					else if ( vi.IsForFree )
						item.LabelTo( from, 1043306 ); // Price: FREE!
					else
						item.LabelTo( from, 1043304, vi.Price.ToString() ); // Price: ~1_COST~

					if ( vi.Description != null && vi.Description != "" )
					{
						// The localized message (1043305) is no longer valid - <br>Seller's Description:<br>"~1_DESC~"
						item.LabelTo( from, "Description: {0}", vi.Description );
					}
				}
			}

			base.OnSingleClickContained( from, item );
		}

		public VendorBackpack( Serial serial ) : base( serial )
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

	public class PlayerVendor : Mobile, IKhaerosMobile
	{
		//start IKhaerosMobile mod
		public Mobile ShieldingMobile{ get { return null; } set { } }
		public virtual int RideBonus{ get{ return 0; } }
		public int Intimidated{ get { return 0; } set { } }
		public int Level{ get { return 0; } set { } }
		public int RageFeatLevel{ get { return 0; } set { } }
		public int ManeuverDamageBonus{ get { return 0; } set { } }
		public int ManeuverAccuracyBonus{ get { return 0; } set { } }
		public int TechniqueLevel{ get { return 0; } set { } }
		public double ShieldValue{ get { return 0; } set { } }
		public virtual void RemoveShieldOfSacrifice() {}
		public virtual void DisableManeuver() {}
		public virtual string GetPersonalPronoun() { return ""; }
		public virtual string GetReflexivePronoun() { return ""; }
		public virtual string GetPossessivePronoun() { return ""; }
		public virtual string GetPossessive() { return ""; }
		public string Technique{ get { return null; } set { } }
		public bool Fizzled{ get { return false; } set { } }
		public bool Enthralled{ get { return false; } set { } }
		public bool CanUseMartialPower{ get { return false; } }
		public bool CanUseMartialStance{ get { return false; } }
		public bool CleaveAttack{ get { return false; } set { } }
		public bool CanDodge{ get { return false; } }
		public virtual bool IsAllyOf( Mobile mob ) { return false; }
		public virtual bool Evaded() { return false; }
		public virtual bool Dodged() { return false; }
		public virtual bool Snatched() { return false; }
		public virtual bool DeflectedProjectile() { return false; }
		public virtual bool IsTired() { return false; }
		public virtual bool IsTired( bool message ) { return false; }
		public virtual bool CanSummon() { return false; }
		public Feats Feats{ get { return new Feats(); } set { } }
		public CombatStyles CombatStyles{ get { return new CombatStyles(); } set { } }
		public DateTime NextFeatUse{ get { return DateTime.MinValue; } set { } }
		public FeatList OffensiveFeat{ get { return FeatList.None; } set { } }
		public BaseCombatManeuver CombatManeuver{ get { return null; } set { } }
		public FeatList CurrentSpell{ get { return FeatList.None; } set { } }
		public Timer CrippledTimer{ get { return null; } set { } }
		public Timer DazedTimer{ get { return null; } set { } }
		public Timer TrippedTimer{ get { return null; } set { } }
		public Timer StunnedTimer{ get { return null; } set { } }
		public Timer DismountedTimer{ get { return null; } set { } }
		public Timer BlindnessTimer{ get { return null; } set { } }
		public Timer DeafnessTimer{ get { return null; } set { } }
		public Timer MutenessTimer{ get { return null; } set { } }
		public Timer DisabledLegsTimer{ get { return null; } set { } }
		public Timer DisabledLeftArmTimer{ get { return null; } set { } }
		public Timer DisabledRightArmTimer{ get { return null; } set { } }
		public Timer FeintTimer{ get { return null; } set { } }
		public Timer HealingTimer{ get { return null; } set { } }
		public Timer AuraOfProtection{ get { return null; } set { } }
        public Timer JusticeAura { get { return null; } set { } }
		public Timer Sanctuary{ get { return null; } set { } }
		public Timer RageTimer{ get { return null; } set { } }
		public Timer ManeuverBonusTimer{ get { return null; } set { } }
		public Timer FreezeTimer{ get { return null; } set { } }
		public BaseStance Stance{ get { return null; } set { } }
		public Mobile ShieldedMobile{ get { return null; } set { } }
		public DateTime NextRage{ get { return DateTime.MinValue; } set { } }
        private bool m_Deserialized;
        public bool Deserialized { get { return m_Deserialized; } set { m_Deserialized = value; } }
		//end IKhaerosMobile mod
		private Hashtable m_SellItems;

		private Mobile m_Owner;
		private BaseHouse m_House;

		private int m_BankAccount;
		private int m_HoldCopper;

		private string m_ShopName;

		private Timer m_PayTimer;
		private DateTime m_NextPayTime;

		private PlayerVendorPlaceholder m_Placeholder;
		
		private bool m_SellsToSecondRace = true;
		private List<string> m_Log = new List<string>();
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool SellsToSecondRace{ get{ return m_SellsToSecondRace; } set{ m_SellsToSecondRace = value; } }
		
		public List<string> Log{ get{ return m_Log; } set{ m_Log = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool ClearLog
		{
			get{ return false; }
			set
			{
				if( value == true )
					this.Log.Clear();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool ListLog
		{
			get{ return false; }
			set
			{
				if( value == true )
				{
					foreach( string st in this.Log )
						this.Say( st );
				}
			}
		}

        //Government information private fields
        private GovernmentEntity m_Government;        

        //Public Government Properties
        [CommandProperty(AccessLevel.GameMaster)]
        public GovernmentEntity Government
        {
            get { return m_Government; }
            set { m_Government = value; }
        }
        
        public int Wages 
        { 
            get 
            {
                if (m_Government == null || m_Government.Deleted)
                    return 0;
                return m_Government.TradeInformation.VendorWages; 
            } 
        }
        public int Taxes 
        { 
            get 
            {
                if (m_Government == null || m_Government.Deleted)
                    return 0;
                return m_Government.TradeInformation.Taxes; 
            } 
        }
        public bool FlatTax 
        { 
            get 
            {
                if (m_Government == null || m_Government.Deleted)
                    return false;
                return m_Government.TradeInformation.FlatTax; 
            } 
        }

        public PlayerVendor(Mobile owner, BaseHouse house): this(owner, house, null)
        {
            
        }

		public PlayerVendor( Mobile owner, BaseHouse house, GovernmentEntity government)
		{
			Owner = owner;
			House = house;
            Government = government;
            if (Government != null && !Government.Deleted)
                if(!Government.Employees.Contains(this))
                    Government.Employees.Add(this);

			if ( BaseHouse.NewVendorSystem )
			{
                if (Government != null)
                {
                    m_BankAccount = 0;
                    m_HoldCopper = Wages;
                }
                else
                {
                    m_BankAccount = 0;
                    m_HoldCopper = 4;
                }
			}
			else
			{
				m_BankAccount = 1000;
				m_HoldCopper = 0;
			}

			ShopName = "Shop Not Yet Named";

			m_SellItems = new Hashtable();

			CantWalk = true;

			if ( !Core.AOS )
				NameHue = 0x35;

			InitStats( 75, 75, 75 );
			InitBody();
			InitOutfit();
			RawStr = 100;
			RawDex = 100;
			RawStam = 100;

			TimeSpan delay = PayTimer.GetInterval();

			m_PayTimer = new PayTimer( this, delay );
			m_PayTimer.Start();

			m_NextPayTime = DateTime.Now + delay;
		}

		public PlayerVendor( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 4 ); // version

            writer.Write((GovernmentEntity)m_Government);

			writer.Write( (bool) m_SellsToSecondRace );
			writer.Write( (int) m_Log.Count );
			
			foreach( string st in m_Log )
				writer.Write( (string) st );
			
			writer.Write( (bool) BaseHouse.NewVendorSystem );
			writer.Write( (string) m_ShopName );
			writer.WriteDeltaTime( (DateTime) m_NextPayTime );
			writer.Write( (Item) House );

			writer.Write( (Mobile) m_Owner );
			writer.Write( (int) m_BankAccount );
			writer.Write( (int) m_HoldCopper );

			writer.Write( (int) m_SellItems.Count );
			foreach ( VendorItem vi in m_SellItems.Values )
			{
				writer.Write( (Item) vi.Item );
				writer.Write( (int) vi.Price );
				writer.Write( (string) vi.Description );

				writer.Write( (DateTime) vi.Created );
			}

            
		}

		public override void Deserialize( GenericReader reader )
		{
            
			base.Deserialize( reader );
            
			int version = reader.ReadInt();
            
			bool newVendorSystem = false;

            //if (version == 3)
               // version = 4;

			switch ( version )
			{
                case 4:
                {
                    //Console.WriteLine("Government");
                    m_Government = (GovernmentEntity)reader.ReadItem();
                    goto case 3;
                }
				case 3:
				{
                    //Console.WriteLine("SecondRace");
					m_SellsToSecondRace = reader.ReadBool();
                    //Console.WriteLine("Count");
					int count = reader.ReadInt();
                    //Console.WriteLine("Log");
					for( int i = 0; i < count; i++ )
						m_Log.Add( reader.ReadString() );
					
					goto case 2;
				}
				case 2:
				case 1:
				{
                    //Console.WriteLine("Read NewVendorSystem");
					newVendorSystem = reader.ReadBool();
                    //Console.WriteLine("ShopName");
					m_ShopName = reader.ReadString();
                    //Console.WriteLine("NextPayTime");
					m_NextPayTime = reader.ReadDeltaTime();
                   // Console.WriteLine("House");
					House = (BaseHouse) reader.ReadItem();

					goto case 0;
				}
				case 0:
				{
                    //Console.WriteLine("Owner");
					m_Owner = reader.ReadMobile();
                    //Console.WriteLine("BankAccount");
					m_BankAccount = reader.ReadInt();
                    //Console.WriteLine("HoldCopper");
					m_HoldCopper = reader.ReadInt();
                    //Console.WriteLine("SellItems Hashtable");
					m_SellItems = new Hashtable();
                    //Console.WriteLine("SellItemsLoop");
					int count = reader.ReadInt();
					for ( int i = 0; i < count; i++ )
					{
						Item item = reader.ReadItem();

						int price = reader.ReadInt();
						if ( price > 100000000 )
							price = 100000000;

						string description = reader.ReadString();

						DateTime created = version < 1 ? DateTime.Now : reader.ReadDateTime();

						if ( item != null )
						{
							SetVendorItem( item, version < 1 && price <= 0 ? -1 : price, description, created );
						}
					}

					break;	
				}
			}
            //Console.WriteLine("NewVendorSystemActivateBool");
			bool newVendorSystemActivated = BaseHouse.NewVendorSystem && !newVendorSystem;

			if ( version < 1 || newVendorSystemActivated )
			{
				if ( version < 1 )
				{
					m_ShopName = "Shop Not Yet Named";
					Timer.DelayCall( TimeSpan.Zero, new TimerStateCallback( UpgradeFromVersion0 ), newVendorSystemActivated );
				}
				else
				{
					Timer.DelayCall( TimeSpan.Zero, new TimerCallback( FixDresswear ) );
				}

				m_NextPayTime = DateTime.Now + PayTimer.GetInterval();

				if ( newVendorSystemActivated )
				{
					m_HoldCopper += m_BankAccount;
					m_BankAccount = 0;
				}
			}

			TimeSpan delay = m_NextPayTime - DateTime.Now;

			m_PayTimer = new PayTimer( this, delay > TimeSpan.Zero ? delay : TimeSpan.Zero );
			m_PayTimer.Start();

			Blessed = false;

			if ( Core.AOS && NameHue == 0x35 )
				NameHue = -1;
			
			if( version < 2 )
			{
				RawStr = 100;
				RawDex = 100;
				RawStam = 100;
			}
			
			m_Deserialized = true;
		}

		private void UpgradeFromVersion0( object newVendorSystem )
		{
			ArrayList toRemove = new ArrayList();

			foreach ( VendorItem vi in m_SellItems.Values )
			{
				if ( !CanBeVendorItem( vi.Item ) )
					toRemove.Add( vi.Item );
				else
					vi.Description = Utility.FixHtml( vi.Description );
			}

			foreach ( Item item in toRemove )
			{
				RemoveVendorItem( item );
			}

			House = BaseHouse.FindHouseAt( this );

			if ( (bool) newVendorSystem )
				ActivateNewVendorSystem();
		}

		private void ActivateNewVendorSystem()
		{
			FixDresswear();

			if ( House != null && !House.IsOwner( Owner ) )
				Destroy( false );
		}

		public void InitBody()
		{
			Hue = Utility.RandomSkinHue();
			SpeechHue = 0x3B2;

			if ( !Core.AOS )
				NameHue = 0x35;

			if ( this.Female = Utility.RandomBool() )
			{
				this.Body = 0x191;
				this.Name = NameList.RandomName( "female" );
			}
			else
			{
				this.Body = 0x190;
				this.Name = NameList.RandomName( "male" );
			}
		}

		public virtual void InitOutfit()
		{
			Item item = new FancyShirt( Utility.RandomNeutralHue() );
			item.Layer = Layer.InnerTorso;
			AddItem( item );
			AddItem( new LongPants( Utility.RandomNeutralHue() ) );
			AddItem( new BodySash( Utility.RandomNeutralHue() ) );
			AddItem( new Boots( Utility.RandomNeutralHue() ) );
			AddItem( new Cloak( Utility.RandomNeutralHue() ) );

			Utility.AssignRandomHair( this );

			Container pack = new VendorBackpack();
			pack.Movable = false;
			AddItem( pack );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return m_Owner; }
			set{ m_Owner = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int BankAccount
		{
			get{ return m_BankAccount; }
			set{ m_BankAccount = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int HoldCopper
		{
			get{ return m_HoldCopper; }
			set{ m_HoldCopper = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string ShopName
		{
			get{ return m_ShopName; }
			set
			{
				if ( value == null )
					m_ShopName = "";
				else
					m_ShopName = value;

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextPayTime
		{
			get{ return m_NextPayTime; }
		}

		public PlayerVendorPlaceholder Placeholder
		{
			get{ return m_Placeholder; } 
			set{ m_Placeholder = value; }
		}

		public BaseHouse House
		{
			get{ return m_House; }
			set
			{
				if ( m_House != null )
					m_House.PlayerVendors.Remove( this );

				if ( value != null )
					value.PlayerVendors.Add( this );

				m_House = value;
			}
		}

		public int ChargePerDay
		{
			get
			{ 
				if ( BaseHouse.NewVendorSystem )
				{
                    if (m_Government == null)
                        return ChargePerRealWorldDay;
                    else
                    {
/*                         if (FlatTax && Taxes > 0)
                            return Wages + Taxes;
                        else
                            return Wages; */
							return 10;
                    }
				}
				else
				{
/* 					long total = 0;
					foreach ( VendorItem vi in m_SellItems.Values )
					{
						total += vi.Price;
					}

					total -= 500;

					if ( total < 0 )
						total = 0; */

/* 					return (int)( 5 + (total / 500) ); */
                    return 10;
				}
			}
		}

		public int ChargePerRealWorldDay
		{
			get
			{
				if ( BaseHouse.NewVendorSystem )
				{
					long total = 0;
					foreach ( VendorItem vi in m_SellItems.Values )
					{
						total += vi.Price;
					}

					/* return (int)( 5 + (total / 500) * 3 ); */
					return 10;
				}
				else
				{
					/* return ChargePerDay * 3; */
					return 10;
				}
			}
		}

		public virtual bool IsOwner( Mobile m )
		{
			if ( m.AccessLevel >= AccessLevel.GameMaster )
				return true;

			/*if ( BaseHouse.NewVendorSystem && House != null )
			{
				return House.IsOwner( m );
			}*/
			else
			{
				return m == Owner;
			}
		}

		protected ArrayList GetItems()
		{
			ArrayList list = new ArrayList();

			foreach ( Item item in this.Items )
			{
				if ( item.Movable && item != this.Backpack && item.Layer != Layer.Hair && item.Layer != Layer.FacialHair )
					list.Add( item );
			}

			if ( this.Backpack != null )
			{
				list.AddRange( this.Backpack.Items );
			}

			return list;
		}

		public virtual void Destroy( bool toBackpack )
		{
			Return();

			if ( !BaseHouse.NewVendorSystem )
				FixDresswear();

			/* Possible cases regarding item return:
			 * 
			 * 1. No item must be returned
			 *       -> do nothing.
			 * 2. ( toBackpack is false OR the vendor is in the internal map ) AND the vendor is associated with a AOS house
			 *       -> put the items into the moving crate or a vendor inventory,
			 *          depending on whether the vendor owner is also the house owner.
			 * 3. ( toBackpack is true OR the vendor isn't associated with any AOS house ) AND the vendor isn't in the internal map
			 *       -> put the items into a backpack.
			 * 4. The vendor isn't associated with any house AND it's in the internal map
			 *       -> do nothing (we can't do anything).
			 */

			ArrayList list = GetItems();

			if ( list.Count > 0 || HoldCopper > 0 ) // No case 1
			{
				if ( ( !toBackpack || this.Map == Map.Internal ) && House != null && House.IsAosRules ) // Case 2
				{
					if ( House.IsOwner( Owner ) ) // Move to moving crate
					{
						if ( House.MovingCrate == null )
							House.MovingCrate = new MovingCrate( House );

						if ( HoldCopper > 0 )
							Banker.Deposit( House.MovingCrate, HoldCopper );

						foreach ( Item item in list )
						{
							House.MovingCrate.DropItem( item );
						}
					}
					else // Move to vendor inventory
					{
						VendorInventory inventory = new VendorInventory( House, Owner, Name, ShopName );
						inventory.Copper = HoldCopper;

						foreach ( Item item in list )
						{
							inventory.AddItem( item );
						}

						House.VendorInventories.Add( inventory );
					}
				}
				else if ( ( toBackpack || House == null || !House.IsAosRules ) && this.Map != Map.Internal ) // Case 3 - Move to backpack
				{
					Container backpack = new Backpack();

					if ( HoldCopper > 0 )
						Banker.Deposit( backpack, HoldCopper );

					foreach ( Item item in list )
					{
						backpack.DropItem( item );
					}

                    backpack.AddItem( new ContractOfEmployment() );
					backpack.MoveToWorld( this.Location, this.Map );
				}
			}

			Delete();
		}

		private void FixDresswear()
		{
			for ( int i = 0; i < Items.Count; ++i )
			{
				Item item = Items[i] as Item;

				if ( item is BaseHat )
					item.Layer = Layer.Helm;
				else if ( item is BaseMiddleTorso )
					item.Layer = Layer.MiddleTorso;
				else if ( item is BaseOuterLegs )
					item.Layer = Layer.OuterLegs;
				else if ( item is BaseOuterTorso )
					item.Layer = Layer.OuterTorso;
				else if ( item is BasePants )
					item.Layer = Layer.Pants;
				else if ( item is BaseShirt )
					item.Layer = Layer.Shirt;
				else if ( item is BaseWaist )
					item.Layer = Layer.Waist;
				else if ( item is BaseShoes )
				{
					if ( item is Sandals )
						item.Hue = 0;

					item.Layer = Layer.Shoes;
				}
			}
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			m_PayTimer.Stop();

			House = null;

			if ( Placeholder != null )
				Placeholder.Delete();
		}

		public override bool IsSnoop( Mobile from )
		{
			return false;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( BaseHouse.NewVendorSystem )
			{
				list.Add( 1062449, ShopName ); // Shop Name: ~1_NAME~
			}
		}

		public VendorItem GetVendorItem( Item item )
		{
			return (VendorItem) m_SellItems[item];
		}

		private VendorItem SetVendorItem( Item item, int price, string description )
		{
			return SetVendorItem( item, price, description, DateTime.Now );
		}

		private VendorItem SetVendorItem( Item item, int price, string description, DateTime created )
		{
			RemoveVendorItem( item );

			VendorItem vi = new VendorItem( item, price, description, created );
			m_SellItems[item] = vi;

			item.InvalidateProperties();

			return vi;
		}

		private void RemoveVendorItem( Item item )
		{
			VendorItem vi = GetVendorItem( item );

			if ( vi != null )
			{
				vi.Invalidate();
				m_SellItems.Remove( item );
				item.OnRemovedFromSale( this );

				foreach ( Item subItem in item.Items )
				{
					RemoveVendorItem( subItem );
				}

				item.InvalidateProperties();
			}
		}

		private bool CanBeVendorItem( Item item )
		{
			Item parent = item.Parent as Item;

			if ( parent == this.Backpack )
				return true;

			if ( parent is Container )
			{
				VendorItem parentVI = GetVendorItem( parent );

				if ( parentVI != null )
					return !parentVI.IsForSale;
			}

			return false;
		}

		public override void OnSubItemAdded( Item item )
		{
			base.OnSubItemAdded( item );

			if ( GetVendorItem( item ) == null && CanBeVendorItem( item ) )
			{
				// TODO: default price should be dependent to the type of object
				SetVendorItem( item, 999, "" );
				item.OnPlacedForSale( this );
			}
		}

		public override void OnSubItemRemoved( Item item )
		{
			base.OnSubItemRemoved( item );

			if ( item.GetBounce() == null )
				RemoveVendorItem( item );
		}

		public override void OnSubItemBounceCleared( Item item )
		{
			base.OnSubItemBounceCleared( item );

			if ( !CanBeVendorItem( item ) )
				RemoveVendorItem( item );
		}

		public override void OnItemRemoved( Item item )
		{
			base.OnItemRemoved( item );

			if ( item == this.Backpack )
			{
				foreach ( Item subItem in item.Items )
				{
					RemoveVendorItem( subItem );
				}
			}
		}
		
		public void SendLogToBook( HTMLBook m_Book )
		{
			int oldPageCount = m_Book.Pages.Length;
			m_Book.Writable = true;
			m_Book.SealedBy = null;
			
			string content = "";
			
			foreach( string st in this.Log )
				content += st + "<br>";
			
			List<BookPageInfo> newPages = new List<BookPageInfo>();
			newPages.Add( new BookPageInfo( new string[]{content} ) );
			
			while( newPages.Count < oldPageCount )
				newPages.Add( new BookPageInfo() );
			
			m_Book.Pages = newPages.ToArray();
			m_Book.FixContent();
			m_Book.HTMLContent = new HTMLContent( m_Book.PagesCount, m_Book.MaxLines, m_Book );
			m_Book.FixStyling();
			m_Book.HTMLContent.UpdateCache();
			m_Book.SealedBy = this;
			m_Book.Writable = false;
			m_Book.Author = this.Name;
			
			string day = TimeSystem.Data.Day.ToString();
			string month = TimeSystem.Data.Month.ToString();
			Commands.LevelSystemCommands.FormatDayAndMonth( ref day, ref month );

			m_Book.Title = ( "Vendor Log: " + day + " of " + month + ", " + TimeSystem.Data.Year.ToString() );
		}

		public override bool OnDragDrop( Mobile from, Item item )
		{
			if ( !IsOwner( from ) )
			{
				SayTo( from, 503209 ); // I can only take item from the shop owner.
				return false;
			}
			
			if( item is HTMLBook && from == this.Owner )
			{
				if( ((HTMLBook)item).PagesCount < 50 )
					from.SendMessage( "You will need a larger book for that." );
				
				else if( this.Log.Count > 0 )
				{
					SendLogToBook( (HTMLBook)item );
					from.SendMessage( "You have received the updated log from your vendor." );
					this.Log.Clear();
				}
				
				else
					from.SendMessage( "Your vendor has nothing new to report." );
				
				return false;
			}

			if ( item is Copper )
			{
				if ( BaseHouse.NewVendorSystem )
				{
					if ( this.HoldCopper < 1000000 )
					{
						SayTo( from, 503210 ); // I'll take that to fund my services.

						this.HoldCopper += item.Amount;
						item.Delete();

						return true;
					}
					else
					{
						from.SendLocalizedMessage( 1062493 ); // Your vendor has sufficient funds for operation and cannot accept this copper.

						return false;
					}
				}
				else
				{
					if ( this.BankAccount < 1000000 )
					{
						SayTo( from, 503210 ); // I'll take that to fund my services.

						this.BankAccount += item.Amount;
						item.Delete();

						return true;
					}
					else
					{
						from.SendLocalizedMessage( 1062493 ); // Your vendor has sufficient funds for operation and cannot accept this copper.

						return false;
					}
				}
			}
			else
			{
				bool newItem = ( GetVendorItem( item ) == null );

				if ( this.Backpack != null && this.Backpack.TryDropItem( from, item, false ) )
				{
					if ( newItem )
						OnItemGiven( from, item );

					return true;
				}
				else
				{
					SayTo( from, 503211 ); // I can't carry any more.
					return false;
				}
			}
		}

		public override bool CheckNonlocalDrop( Mobile from, Item item, Item target )
		{
			if ( IsOwner( from ) )
			{
				if ( GetVendorItem( item ) == null )
				{
					// We must wait until the item is added
					Timer.DelayCall( TimeSpan.Zero, new TimerStateCallback( NonLocalDropCallback ), new object[] { from, item } );
				}

				return true;
			}
			else
			{
				SayTo( from, 503209 ); // I can only take item from the shop owner.
				return false;
			}
		}

		private void NonLocalDropCallback( object state )
		{
			object[] aState = (object[]) state;

			Mobile from = (Mobile) aState[0];
			Item item = (Item) aState[1];

			OnItemGiven( from, item );
		}

		private void OnItemGiven( Mobile from, Item item )
		{
			VendorItem vi = GetVendorItem( item );

			if ( vi != null )
			{
				string name;
				if ( item.Name != null && item.Name != "" )
					name = item.Name;
				else
					name = "#" + item.LabelNumber.ToString();

				from.SendLocalizedMessage( 1043303, name ); // Type in a price and description for ~1_ITEM~ (ESC=not for sale)
				from.Prompt = new VendorPricePrompt( this, vi );
			}
		}

		public override bool AllowEquipFrom( Mobile from )
		{
			if ( BaseHouse.NewVendorSystem && IsOwner( from ) )
				return true;

			return base.AllowEquipFrom( from );
		}

		public override bool CheckNonlocalLift( Mobile from, Item item )
		{
			if ( item.IsChildOf( this.Backpack ) )
			{
				if ( IsOwner( from ) )
				{
					return true;
				}
				else
				{
					SayTo( from, 503223 ); // If you'd like to purchase an item, just ask.
					return false;
				}
			}
			else if ( BaseHouse.NewVendorSystem && IsOwner( from ) )
			{
				return true;
			}

			return base.CheckNonlocalLift( from, item );
		}

		public bool CanInteractWith( Mobile from, bool ownerOnly )
		{
			if ( !from.CanSee( this ) || !Utility.InUpdateRange( from, this ) || !from.CheckAlive() )
				return false;

			if ( ownerOnly )
				return IsOwner( from );

			if ( House != null && House.IsBanned( from ) && !IsOwner( from ) )
			{
				from.SendLocalizedMessage( 1062674 ); // You can't shop from this home as you have been banned from this establishment.
				return false;
			}
			
			PlayerVendor vendor = this;
			
			/*if( vendor.Owner != null && vendor.Owner is PlayerMobile && from is PlayerMobile )
			{
				PlayerMobile owner = vendor.Owner as PlayerMobile;
				PlayerMobile buyer = from as PlayerMobile;
				Nation nation = buyer.GetDisguisedNation();
				
				if( owner.Nation != nation && !(this.SellsToSecondRace && owner.RaciallyCompatible(nation)) )
				{
					buyer.SendMessage( "You cannot buy from this foreign vendor." );
					return false;
				}
			}*/

            if (vendor.Government != null && !vendor.Government.Deleted)
            {
                if (vendor.Government.MilitaryPolicies.Exceptions.Contains(from.Name.ToString()))
                    return true;                
                
                if (vendor.Government.MilitaryPolicies.KillIndividualOnSight.Contains(from.Name.ToString())||vendor.Government.MilitaryPolicies.JailIndividualOnSight.Contains(from.Name.ToString()))
                {
                    from.SendMessage( "You cannot buy from this vendor because you are a criminal in this nation." );
                    return false;
                }

                PlayerMobile m = from as PlayerMobile;

                if (vendor.Government.MilitaryPolicies.KillNationOnSight.Contains(m.Nation)||vendor.Government.MilitaryPolicies.JailNationOnSight.Contains(m.Nation))
                {
                    from.SendMessage( "You cannot buy from this vendor because this nation has an embargo against your nation." );
                    return false;
                }               
                
            }
			
			return true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsOwner( from ) )
			{
				SendOwnerGump( from );
			}
			else if ( CanInteractWith( from, false ) )
			{
				OpenBackpack( from );
			}
		}

		public override void DisplayPaperdollTo( Mobile m )
		{
			if ( BaseHouse.NewVendorSystem )
			{
				base.DisplayPaperdollTo( m );
			}
			else if ( CanInteractWith( m, false ) )
			{
				OpenBackpack( m );
			}
		}

		public void SendOwnerGump( Mobile to )
		{
			if ( BaseHouse.NewVendorSystem )
			{
				to.CloseGump( typeof( NewPlayerVendorOwnerGump ) );
				to.CloseGump( typeof( NewPlayerVendorCustomizeGump ) );

				to.SendGump( new NewPlayerVendorOwnerGump( this ) );
			}
			else
			{
				to.CloseGump( typeof( PlayerVendorOwnerGump ) );
				to.CloseGump( typeof( PlayerVendorCustomizeGump ) );

				to.SendGump( new PlayerVendorOwnerGump( this ) );
			}
		}

		public void OpenBackpack( Mobile from )
		{
			if ( this.Backpack != null )
			{
				SayTo( from, IsOwner( from ) ? 1010642 : 503208 ); // Take a look at my/your goods.

				this.Backpack.DisplayTo( from );
			}
		}

		public static void TryToBuy( Item item, Mobile from )
		{
			PlayerVendor vendor = item.RootParent as PlayerVendor;

			if ( vendor == null || !vendor.CanInteractWith( from, false ) )
				return;

			if ( vendor.IsOwner( from ) )
			{
				vendor.SayTo( from, 503212 ); // You own this shop, just take what you want.
				return;
			}

            //Check for Government related issues
            if (vendor.m_Government != null && !vendor.Government.Deleted)
            {
                if (!vendor.m_Government.MilitaryPolicies.Exceptions.Contains(from.Name) && from is PlayerMobile)
                {
                    PlayerMobile m = from as PlayerMobile;
                    if (vendor.m_Government.MilitaryPolicies.KillIndividualOnSight.Contains(m.Name) || vendor.m_Government.MilitaryPolicies.JailIndividualOnSight.Contains(m.Name))
                    {
                        vendor.Say("I will not trade with criminals of the " + vendor.m_Government.Name + ".");
                        return;
                    }
                    if (vendor.m_Government.MilitaryPolicies.KillNationOnSight.Contains(m.Nation) || vendor.m_Government.MilitaryPolicies.JailNationOnSight.Contains(m.Nation))
                    {
                        vendor.Say("I will not trade with your kind.");
                        return;
                    }
                    if (vendor.m_Government.TradeInformation.NoBusinessList.Contains(m.Name) || vendor.m_Government.TradeInformation.NoBusinessNations.Contains(m.Nation))
                    {
                        vendor.Say("You are banned from doing business in the " + vendor.m_Government.Name);
                        return;
                    }
                }

            }

			VendorItem vi = vendor.GetVendorItem( item );

			if ( vi == null )
			{
				vendor.SayTo( from, 503216 ); // You can't buy that.
			}
			else if ( !vi.IsForSale )
			{
				vendor.SayTo( from, 503202 ); // This item is not for sale.
			}
			else if ( vi.Created + TimeSpan.FromMinutes( 1.0 ) > DateTime.Now )
			{
				from.SendMessage( "You cannot buy this item right now.  Please wait one minute and try again." );
			}
			else
			{
				from.CloseGump( typeof( PlayerVendorBuyGump ) );
				from.SendGump( new PlayerVendorBuyGump( vendor, vi ) );
			}
		}

		public void CollectCopper( Mobile to )
		{
			if ( HoldCopper > 0 )
			{
				SayTo( to, "How much of the {0} that I'm holding would you like?", HoldCopper.ToString() );
				to.SendMessage( "Enter the amount of copper you wish to withdraw (ESC = CANCEL):" );

				to.Prompt = new CollectCopperPrompt( this );
			}
			else
			{
				SayTo( to, 503215 ); // I am holding no copper for you.
			}
		}

		public int GiveCopper( Mobile to, int amount )
		{
			if ( amount <= 0 )
				return 0;

			if ( amount > HoldCopper )
			{
				SayTo( to, "I'm sorry, but I'm only holding {0} copper for you.", HoldCopper.ToString() );
				return 0;
			}

			int amountGiven = Banker.DepositUpTo( to, amount );
			HoldCopper -= amountGiven;

			if ( amountGiven > 0 )
			{
				to.SendLocalizedMessage( 1060397, amountGiven.ToString() ); // ~1_AMOUNT~ copper has been deposited into your bank box.
			}

			if ( amountGiven == 0 )
			{
				SayTo( to, 1070755 ); // Your bank box cannot hold the copper you are requesting.  I will keep the copper until you can take it.
			}
			else if ( amount > amountGiven )
			{
				SayTo( to, 1070756 ); // I can only give you part of the copper now, as your bank box is too full to hold the full amount.
			}
			else if ( HoldCopper > 0 )
			{
				SayTo( to, 1042639 ); // Your copper has been transferred.
			}
			else
			{
				SayTo( to, 503234 ); // All the copper I have been carrying for you has been deposited into your bank account.
			}

			return amountGiven;
		}

		public void Dismiss( Mobile from )
		{
			Container pack = this.Backpack;

			if ( pack != null && pack.Items.Count > 0 )
			{
				SayTo( from, 1038325 ); // You cannot dismiss me while I am holding your goods.
				return;
			}

			if ( HoldCopper > 0 )
			{
				GiveCopper( from, HoldCopper );

				if ( HoldCopper > 0 )
					return;
			}

			Destroy( true );
		}

		public void Rename( Mobile from )
		{
			from.SendLocalizedMessage( 1062494 ); // Enter a new name for your vendor (20 characters max):

			from.Prompt = new VendorNamePrompt( this );
		}

		public void RenameShop( Mobile from )
		{
			from.SendLocalizedMessage( 1062433 ); // Enter a new name for your shop (20 chars max):

			from.Prompt = new ShopNamePrompt( this );
		}

		public bool CheckTeleport( Mobile to )
		{
			if ( Deleted || !IsOwner( to ) || House == null || this.Map == Map.Internal )
				return false;

			if ( House.IsInside( to ) || to.Map != House.Map || !House.InRange( to, 5 ) )
				return false;

			if ( Placeholder == null )
			{
				Placeholder = new PlayerVendorPlaceholder( this );
				Placeholder.MoveToWorld( this.Location, this.Map );

				this.MoveToWorld( to.Location, to.Map );

				to.SendLocalizedMessage( 1062431 ); // This vendor has been moved out of the house to your current location temporarily.  The vendor will return home automatically after two minutes have passed once you are done managing its inventory or customizing it.
			}
			else
			{
				Placeholder.RestartTimer();

				to.SendLocalizedMessage( 1062430 ); // This vendor is currently temporarily in a location outside its house.  The vendor will return home automatically after two minutes have passed once you are done managing its inventory or customizing it.
			}

			return true;
		}

		public void Return()
		{
			if ( Placeholder != null )
				Placeholder.Delete();
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			if ( from.Alive && Placeholder != null && IsOwner( from ) )
			{
				list.Add( new ReturnVendorEntry( this ) );
			}

			base.GetContextMenuEntries( from, list );
		}

		private class ReturnVendorEntry : ContextMenuEntry
		{
			private PlayerVendor m_Vendor;

			public ReturnVendorEntry( PlayerVendor vendor ) : base( 6214 )
			{
				m_Vendor = vendor;
			}

			public override void OnClick()
			{
				Mobile from = Owner.From;

				if ( !m_Vendor.Deleted && m_Vendor.IsOwner( from ) && from.CheckAlive() )
					m_Vendor.Return();
			}
		}

		public override bool HandlesOnSpeech( Mobile from )
		{
			return ( from.Alive && from.GetDistanceToSqrt( this ) <= 3 );
		}

		public bool WasNamed( string speech )
		{
			return this.Name != null && Insensitive.StartsWith( speech, this.Name );
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( e.Handled || !from.Alive || from.GetDistanceToSqrt( this ) > 3 )
				return;

			if ( e.HasKeyword( 0x3C ) || (e.HasKeyword( 0x171 ) && WasNamed( e.Speech ))  ) // vendor buy, *buy*
			{
				if ( IsOwner( from ) )
				{
					SayTo( from, 503212 ); // You own this shop, just take what you want.
				}
				else if ( House == null || !House.IsBanned( from ) )
				{
					from.SendLocalizedMessage( 503213 ); // Select the item you wish to buy.
					from.Target = new PVBuyTarget();

					e.Handled = true;
				}
			} 
			else if ( e.HasKeyword( 0x3D ) || (e.HasKeyword( 0x172 ) && WasNamed( e.Speech )) ) // vendor browse, *browse
			{
				if ( House != null && House.IsBanned( from ) && !IsOwner( from ) )
				{
					SayTo( from, 1062674 ); // You can't shop from this home as you have been banned from this establishment.
				}
				else
				{
					OpenBackpack( from );

					e.Handled = true;
				}
			}
			else if ( e.HasKeyword( 0x3E ) || (e.HasKeyword( 0x173 ) && WasNamed( e.Speech )) ) // vendor collect, *collect
			{
				if ( IsOwner( from ) )
				{
					CollectCopper( from );

					e.Handled = true;
				}
			}
			else if ( e.HasKeyword( 0x3F ) || (e.HasKeyword( 0x174 ) && WasNamed( e.Speech )) ) // vendor status, *status
			{
				if ( IsOwner( from ) )
				{
					SendOwnerGump( from );

					e.Handled = true;
				}
				else
				{
					SayTo( from, 503226 ); // What do you care? You don't run this shop.	
				}
			}
			else if ( e.HasKeyword( 0x40 ) || (e.HasKeyword( 0x175 ) && WasNamed( e.Speech )) ) // vendor dismiss, *dismiss
			{
				if ( IsOwner( from ) )
				{
					Dismiss( from );

					e.Handled = true;
				}
			}
			else if ( e.HasKeyword( 0x41 ) || (e.HasKeyword( 0x176 ) && WasNamed( e.Speech )) ) // vendor cycle, *cycle
			{
				if ( IsOwner( from ) )
				{
					this.Direction = this.GetDirectionTo( from );

					e.Handled = true;
				}
			}
		}

		private class PayTimer : Timer
		{
			public static TimeSpan GetInterval()
			{
				return TimeSpan.FromHours( 24 );
			}

			private PlayerVendor m_Vendor;

			public PayTimer( PlayerVendor vendor, TimeSpan delay ) : base( delay, GetInterval() )
			{
				m_Vendor = vendor;

				Priority = TimerPriority.OneMinute;
			}

			protected override void OnTick()
			{
				m_Vendor.m_NextPayTime = DateTime.Now + this.Interval;

				int pay;
				int totalCopper;

				pay = m_Vendor.ChargePerDay;
				totalCopper = m_Vendor.HoldCopper;

                if (m_Vendor.FlatTax && m_Vendor.Taxes > 0 && m_Vendor.Government != null && !m_Vendor.Government.Deleted)
                {
                    pay -= m_Vendor.Taxes;
                    int tax = m_Vendor.Taxes;
                    m_Vendor.HoldCopper -= tax;
                    totalCopper -= tax;
                    m_Vendor.Government.Treasury.DropItem(new Copper(tax));
                }

                pay /= 2;

				if ( pay > totalCopper )
				{
					m_Vendor.Destroy( !BaseHouse.NewVendorSystem );
				}
				else
				{
					if ( !BaseHouse.NewVendorSystem )
					{
						if ( m_Vendor.BankAccount >= pay )
						{
							m_Vendor.BankAccount -= pay;
							pay = 0;
						}
						else
						{
							pay -= m_Vendor.BankAccount;
							m_Vendor.BankAccount = 0;
						}
					}

					m_Vendor.HoldCopper -= pay;
                    if(m_Vendor.Government != null && !m_Vendor.Government.Deleted)
                        m_Vendor.Government.Treasury.ConsumeUpTo(typeof(Copper), pay);                    
				}
			}
		}

		[PlayerVendorTarget]
		private class PVBuyTarget : Target
		{
			public PVBuyTarget() : base( 3, false, TargetFlags.None )
			{
				AllowNonlocal = true;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Item )
				{
					TryToBuy( (Item) targeted, from );
				}
			}
		}

		private class VendorPricePrompt : Prompt
		{
			private PlayerVendor m_Vendor;
			private VendorItem m_VI;

			public VendorPricePrompt( PlayerVendor vendor, VendorItem vi )
			{
				m_Vendor = vendor;
				m_VI = vi;
			}

			public override void OnResponse( Mobile from, string text )
			{
				if ( !m_VI.Valid || !m_Vendor.CanInteractWith( from, true ) )
					return;

				string firstWord;

				int sep = text.IndexOfAny( new char[] { ' ', ',' } );
				if ( sep >= 0 )
					firstWord = text.Substring( 0, sep );
				else
					firstWord = text;

				int price;
				string description;

				try
				{
					price = Convert.ToInt32( firstWord );

					if ( sep >= 0 )
						description = text.Substring( sep + 1 ).Trim();
					else
						description = "";
				}
				catch
				{
					price = -1;
					description = text.Trim();
				}

				SetInfo( from, price, Utility.FixHtml( description ) );
			}

			public override void OnCancel( Mobile from )
			{
				if ( !m_VI.Valid || !m_Vendor.CanInteractWith( from, true ) )
					return;

				SetInfo( from, -1, "" );
			}

			private void SetInfo( Mobile from, int price, string description )
			{
				Item item = m_VI.Item;

				bool setPrice = false;

				if ( price < 0 ) // Not for sale
				{
					price = -1;

					if ( item is Container )
					{
						if ( item is LockableContainer && ((LockableContainer)item).Locked )
							m_Vendor.SayTo( from, 1043298 ); // Locked items may not be made not-for-sale.
						else if ( item.Items.Count > 0 )
							m_Vendor.SayTo( from, 1043299 ); // To be not for sale, all items in a container must be for sale.
						else
							setPrice = true;
					}
					else if ( item is BaseBook || item is Engines.BulkOrders.BulkOrderBook )
					{
						setPrice = true;
					}
					else
					{
						m_Vendor.SayTo( from, 1043301 ); // Only the following may be made not-for-sale: books, containers, keyrings, and items in for-sale containers.
					}
				}
				else
				{
					if ( price > 100000000 )
					{
						price = 100000000;
						from.SendMessage( "You cannot price items above 100,000,000 copper.  The price has been adjusted." );
					}
                    if (m_Vendor.Government != null)
                        if (price > (m_Vendor.Wages * 1000))
                        {
                            price = m_Vendor.Wages * 1000;
                            from.SendMessage( "You cannot price items above " + price.ToString() + ". The price has been adjusted." );
                        }

					setPrice = true;
				}

				if ( setPrice )
				{
					m_Vendor.SetVendorItem( item, price, description );
				}
				else
				{
					m_VI.Description = description;
				}
			}
		}

		private class CollectCopperPrompt : Prompt
		{
			private PlayerVendor m_Vendor;

			public CollectCopperPrompt( PlayerVendor vendor )
			{
				m_Vendor = vendor;
			}

			public override void OnResponse( Mobile from, string text )
			{
				if ( !m_Vendor.CanInteractWith( from, true ) )
					return;

				text = text.Trim();

				int amount;
				try
				{
					amount = Convert.ToInt32( text );
				}
				catch
				{
					amount = 0;
				}

				GiveCopper( from, amount );
			}

			public override void OnCancel( Mobile from )
			{
				if ( !m_Vendor.CanInteractWith( from, true ) )
					return;

				GiveCopper( from, 0 );
			}

			private void GiveCopper( Mobile to, int amount )
			{
				if ( amount <= 0 )
				{
					m_Vendor.SayTo( to, "Very well. I will hold on to the money for now then." );
				}
				else
				{
					m_Vendor.GiveCopper( to, amount );
				}
			}
		}

		private class VendorNamePrompt : Prompt
		{
			private PlayerVendor m_Vendor;

			public VendorNamePrompt( PlayerVendor vendor )
			{
				m_Vendor = vendor;
			}

			public override void OnResponse( Mobile from, string text )
			{
				if ( !m_Vendor.CanInteractWith( from, true ) )
					return;

				string name = text.Trim();

				if ( !NameVerification.Validate( name, 1, 20, true, true, true, 0, NameVerification.Empty ) )
				{
					m_Vendor.SayTo( from, "That name is unacceptable." );
					return;
				}

				m_Vendor.Name = Utility.FixHtml( name );

				from.SendLocalizedMessage( 1062496 ); // Your vendor has been renamed.

				from.SendGump( new NewPlayerVendorOwnerGump( m_Vendor ) );
			}
		}

		private class ShopNamePrompt : Prompt
		{
			private PlayerVendor m_Vendor;

			public ShopNamePrompt( PlayerVendor vendor )
			{
				m_Vendor = vendor;
			}

			public override void OnResponse( Mobile from, string text )
			{
				if ( !m_Vendor.CanInteractWith( from, true ) )
					return;

				string name = text.Trim();

				if ( !NameVerification.Validate( name, 1, 20, true, true, true, 0, NameVerification.Empty ) )
				{
					m_Vendor.SayTo( from, "That name is unacceptable." );
					return;
				}

				m_Vendor.ShopName = Utility.FixHtml( name );

				from.SendGump( new NewPlayerVendorOwnerGump( m_Vendor ) );
			}
		}

		public override bool CanBeDamaged()
		{
			return false;
		}

	}

	public class PlayerVendorPlaceholder : Item
	{
		private PlayerVendor m_Vendor;
		private ExpireTimer m_Timer;

		[CommandProperty( AccessLevel.GameMaster )]
		public PlayerVendor Vendor{ get{ return m_Vendor; } }

		public PlayerVendorPlaceholder( PlayerVendor vendor ) : base( 0x1F28 )
		{
			Hue = 0x672;
			Movable = false;

			m_Vendor = vendor;

			m_Timer = new ExpireTimer( this );
			m_Timer.Start();
		}

		public PlayerVendorPlaceholder( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Vendor != null )
				list.Add( 1062498, m_Vendor.Name ); // reserved for vendor ~1_NAME~
		}

		public void RestartTimer()
		{
			m_Timer.Stop();
			m_Timer.Start();
		}

		private class ExpireTimer : Timer
		{
			private PlayerVendorPlaceholder m_Placeholder;

			public ExpireTimer( PlayerVendorPlaceholder placeholder ) : base( TimeSpan.FromMinutes( 2.0 ) )
			{
				m_Placeholder = placeholder;

				Priority = TimerPriority.FiveSeconds;
			}

			protected override void OnTick()
			{
				m_Placeholder.Delete();
			}
		}

		public override void OnDelete()
		{
			if ( m_Vendor != null && !m_Vendor.Deleted )
			{
				m_Vendor.MoveToWorld( this.Location, this.Map );
				m_Vendor.Placeholder = null;
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 );

			writer.Write( (Mobile) m_Vendor );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			m_Vendor = (PlayerVendor) reader.ReadMobile();

			Timer.DelayCall( TimeSpan.Zero, new TimerCallback( Delete ) );
		}
	}    
}
