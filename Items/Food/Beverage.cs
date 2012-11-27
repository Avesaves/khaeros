// Includes "drinking" messages. That's it.  - Alari

using System;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Engines.Plants;
using System.Collections;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Engines.XmlSpawner2;
using Server.Misc;


namespace Server.Items
{
	public enum BeverageType
	{
		Ale,
		Cider,
		Liquor,
		Milk,
		Wine,
		Water,
		Juice,
		Mead
	}

	public interface IHasQuantity
	{
		int Quantity{ get; set; }
	}

	public interface IWaterSource : IHasQuantity
	{
	}

	// TODO: Flipable attributes

	[TypeAlias( "Server.Items.BottleAle", "Server.Items.BottleLiquor", "Server.Items.BottleWine" )]
	public class BeverageBottle : BaseBeverage
	{
		public override int BaseLabelNumber{ get{ return 1042959; } } // a bottle of Ale
		public override int MaxQuantity{ get{ return 5; } }
		public override bool Fillable{ get{ return false; } }

		public override int ComputeItemID()
		{
			if ( !IsEmpty )
			{
				return 0x99F; // equippable graphic
				/*switch ( Content )
				{
					case BeverageType.Ale: return 0x99F;
					case BeverageType.Cider: return 0x99F;
					case BeverageType.Liquor: return 0x99B;
					case BeverageType.Milk: return 0x99B;
					case BeverageType.Wine: return 0x9C7;
					case BeverageType.Water: return 0x99B;
					case BeverageType.Juice: return 0x99B;
					case BeverageType.Mead: return 0x9C7;
				}*/
			}

			return 0;
		}

		[Constructable]
		public BeverageBottle( BeverageType type ) : base( type )
		{
			Weight = 1.0;
			Layer = Layer.OneHanded;
			
			switch ( (int)type )
			{
				case (int)BeverageType.Wine:
				{
					Hue = 2950;
					break;
				}
				
				default:
				{
					Hue = 0;
					break;
				}
			}
		}

		public BeverageBottle( Serial serial ) : base( serial )
		{
		}
		
		public override bool CanEquip( Mobile from )
		{
			if ( ItemID == 0x99F )
				return true;
			else
				return false;
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

			switch ( version )
			{
				case 0:
				{
					if ( CheckType( "BottleAle" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Ale;
					}
					else if ( CheckType( "BottleLiquor" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Liquor;
					}
					else if ( CheckType( "BottleWine" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Wine;
					}
					else
					{
						throw new Exception( World.LoadingType );
					}

					break;
				}
			}
		}
	}

	public class Jug : BaseBeverage
	{
		public override int BaseLabelNumber{ get{ return 1042965; } } // a jug of Ale
		public override int MaxQuantity{ get{ return 10; } }
		public override bool Fillable{ get{ return false; } }

		public override int ComputeItemID()
		{
			if ( !IsEmpty )
				return 0x9C8;

			return 0;
		}

		[Constructable]
		public Jug( BeverageType type ) : base( type )
		{
			Weight = 1.0;
		}

		public Jug( Serial serial ) : base( serial )
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
		}
	}

	public class CeramicMug : BaseBeverage
	{
		public override int BaseLabelNumber{ get{ return 1042982; } } // a ceramic mug of Ale
		public override int MaxQuantity{ get{ return 1; } }

		public override int ComputeItemID()
		{
			if ( ItemID >= 0x995 && ItemID <= 0x999 )
				return ItemID;
			else if ( ItemID == 0x9CA )
				return ItemID;

			return 0x995;
		}

		[Constructable]
		public CeramicMug()
		{
			Weight = 1.0;
		}

		[Constructable]
		public CeramicMug( BeverageType type ) : base( type )
		{
			Weight = 1.0;
		}

		public CeramicMug( Serial serial ) : base( serial )
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
		}
	}

	public class PewterMug : BaseBeverage
	{
		public override int BaseLabelNumber{ get{ return 1042994; } } // a pewter mug with Ale
		public override int MaxQuantity{ get{ return 1; } }

		public override int ComputeItemID()
		{
			if ( ItemID >= 0xFFF && ItemID <= 0x1002 )
				return ItemID;

			return 0xFFF;
		}

		[Constructable]
		public PewterMug()
		{
			Weight = 1.0;
		}

		[Constructable]
		public PewterMug( BeverageType type ) : base( type )
		{
			Weight = 1.0;
		}

		public PewterMug( Serial serial ) : base( serial )
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
		}
	}

	public class Goblet : BaseBeverage
	{
		public override int BaseLabelNumber{ get{ return 1043000; } } // a goblet of Ale
		public override int MaxQuantity{ get{ return 1; } }

		public override int ComputeItemID()
		{
			if ( ItemID == 0x99A || ItemID == 0x9B3 || ItemID == 0x9BF || ItemID == 0x9CB )
				return ItemID;

			return 0x99A;
		}

		[Constructable]
		public Goblet()
		{
			Weight = 1.0;
			Layer = Layer.OneHanded;
		}

		[Constructable]
		public Goblet( BeverageType type ) : base( type )
		{
			Weight = 1.0;
			Layer = Layer.OneHanded;
		}

		public Goblet( Serial serial ) : base( serial )
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
		}
	}

	[TypeAlias( "Server.Items.MugAle", "Server.Items.GlassCider", "Server.Items.GlassLiquor", "Server.Items.GlassMilk",
		  "Server.Items.GlassWine", "Server.Items.GlassWater", "Server.Items.GlassJuice", "Server.Items.GlassMead" )]
	public class GlassMug : BaseBeverage
	{
		public override int EmptyLabelNumber{ get{ return 1022456; } } // mug
		public override int BaseLabelNumber{ get{ return 1042976; } } // a mug of Ale
		public override int MaxQuantity{ get{ return 5; } }

		public override int ComputeItemID()
		{
			if ( IsEmpty )
				return (ItemID >= 0x1F81 && ItemID <= 0x1F84 ? ItemID : 0x1F81);

			switch ( Content )
			{
				case BeverageType.Ale: return (ItemID == 0x9EF ? 0x9EF : 0x9EE);
				case BeverageType.Cider: return (ItemID >= 0x1F7D && ItemID <= 0x1F80 ? ItemID : 0x1F7D);
				case BeverageType.Liquor: return (ItemID >= 0x1F85 && ItemID <= 0x1F88 ? ItemID : 0x1F85);
				case BeverageType.Milk: return (ItemID >= 0x1F89 && ItemID <= 0x1F8C ? ItemID : 0x1F89);
				case BeverageType.Wine: return (ItemID >= 0x1F8D && ItemID <= 0x1F90 ? ItemID : 0x1F8D);
				case BeverageType.Water: return (ItemID >= 0x1F91 && ItemID <= 0x1F94 ? ItemID : 0x1F91);
				case BeverageType.Juice: return (ItemID >= 0x1F91 && ItemID <= 0x1F94 ? ItemID : 0x1F91);
				case BeverageType.Mead: return (ItemID >= 0x1F8D && ItemID <= 0x1F90 ? ItemID : 0x1F8D);
			}

			return 0;
		}

		[Constructable]
		public GlassMug()
		{
			Weight = 1.0;
		}

		[Constructable]
		public GlassMug( BeverageType type ) : base( type )
		{
			Weight = 1.0;
		}

		public GlassMug( Serial serial ) : base( serial )
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

			switch ( version )
			{
				case 0:
				{
					if ( CheckType( "MugAle" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Ale;
					}
					else if ( CheckType( "GlassCider" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Cider;
					}
					else if ( CheckType( "GlassLiquor" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Liquor;
					}
					else if ( CheckType( "GlassMilk" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Milk;
					}
					else if ( CheckType( "GlassWine" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Wine;
					}
					else if ( CheckType( "GlassWater" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Water;
					}
					else if ( CheckType( "GlassJuice" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Juice;
					}
					else if ( CheckType( "GlassMead" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Mead;
					}
					else
					{
						throw new Exception( World.LoadingType );
					}

					break;
				}
			}
		}
	}

	[TypeAlias( "Server.Items.PitcherAle", "Server.Items.PitcherCider", "Server.Items.PitcherLiquor",
		 		"Server.Items.PitcherMilk", "Server.Items.PitcherWine","Server.Items.PitcherWater", 
				"Server.Items.PitcherJuice", "Server.Items.PitcherMead", "Server.Items.GlassPitcher" )]
	public class Pitcher : BaseBeverage
	{
		public override int BaseLabelNumber{ get{ return 1048128; } } // a Pitcher of Ale
		public override int MaxQuantity{ get{ return 5; } }

		public override int ComputeItemID()
		{
			if ( IsEmpty )
			{
				if ( ItemID == 0x9A7 || ItemID == 0xFF7 )
					return ItemID;

				return 0xFF6;
			}

			switch ( Content )
			{
				case BeverageType.Ale:
				{
					if ( ItemID == 0x1F96 )
						return ItemID;

					return 0x1F95;
				}
				case BeverageType.Cider:
				{
					if ( ItemID == 0x1F98 )
						return ItemID;

					return 0x1F97;
				}
				case BeverageType.Liquor:
				{
					if ( ItemID == 0x1F9A )
						return ItemID;

					return 0x1F99;
				}
				case BeverageType.Milk:
				{
					if ( ItemID == 0x9AD )
						return ItemID;

					return 0x9F0;
				}
				case BeverageType.Wine:
				{
					if ( ItemID == 0x1F9C )
						return ItemID;

					return 0x1F9B;
				}
				case BeverageType.Water:
				{
					if ( ItemID == 0xFF8 || ItemID == 0xFF9 || ItemID == 0x1F9E )
						return ItemID;

					return 0x1F9D;
				}
				case BeverageType.Juice:
				{
					if ( ItemID == 0x1F9C )
						return ItemID;

					return 0x1F9B;
				}
				case BeverageType.Mead:
				{
					if ( ItemID == 0x1F9C )
						return ItemID;

					return 0x1F97;
				}
			}

			return 0;
		}

		[Constructable]
		public Pitcher()
		{
			Weight = 2.0;
		}

		[Constructable]
		public Pitcher( BeverageType type ) : base( type )
		{
			Weight = 2.0;
		}

		public Pitcher( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			if ( CheckType( "PitcherWater" ) || CheckType( "GlassPitcher" ) )
				base.Deserialize( reader, false );
			else
				base.Deserialize( reader, true );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					if ( CheckType( "PitcherAle" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Ale;
					}
					else if ( CheckType( "PitcherCider" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Cider;
					}
					else if ( CheckType( "PitcherLiquor" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Liquor;
					}
					else if ( CheckType( "PitcherMilk" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Milk;
					}
					else if ( CheckType( "PitcherWine" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Wine;
					}
					else if ( CheckType( "PitcherWater" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Water;
					}
					else if ( CheckType( "GlassPitcher" ) )
					{
						Quantity = 0;
						Content = BeverageType.Water;
					}
					else if ( CheckType( "PitcherJuice" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Juice;
					}
					else if ( CheckType( "PitcherMead" ) )
					{
						Quantity = MaxQuantity;
						Content = BeverageType.Mead;
					}
					else
					{
						throw new Exception( World.LoadingType );
					}

					break;
				}
			}
		}
	}

	public abstract class BaseBeverage : Item, IHasQuantity
	{
		private BeverageType m_Content;
		private int m_Quantity;
		private Mobile m_Poisoner;
		private Poison m_Poison;
        private Disease m_Disease;

		public override int LabelNumber
		{
			get
			{
				int num = BaseLabelNumber;

				if ( IsEmpty || num == 0 )
					return EmptyLabelNumber;

				return BaseLabelNumber + (int)m_Content;
			}
		}

		public virtual bool ShowQuantity{ get{ return ( MaxQuantity > 1 ); } }
		public virtual bool Fillable{ get{ return true; } }
		public virtual bool Pourable{ get{ return true; } }

		public virtual  int EmptyLabelNumber{ get{ return base.LabelNumber; } }
		public virtual  int BaseLabelNumber{ get{ return 0; } }

		public abstract int MaxQuantity{ get; }
		public abstract int ComputeItemID();

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsEmpty
		{
			get{ return ( m_Quantity <= 0 ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ContainsAlchohol
		{
			get{ return ( !IsEmpty && m_Content != BeverageType.Milk && m_Content != BeverageType.Water ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsFull
		{
			get{ return ( m_Quantity >= MaxQuantity ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Poison Poison
		{
			get{ return m_Poison; }
			set{ m_Poison = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Poisoner
		{
			get{ return m_Poisoner; }
			set{ m_Poisoner = value; }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public Disease Disease
        {
            get { return m_Disease; }
            set { m_Disease = value; }
        }

		[CommandProperty( AccessLevel.GameMaster )]
		public BeverageType Content
		{
			get{ return m_Content; }
			set
			{
				m_Content = value;

				InvalidateProperties();

				int itemID = ComputeItemID();

				if ( itemID > 0 )
					ItemID = itemID;
				else
					Delete();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Quantity
		{
			get{ return m_Quantity; }
			set
			{
				if ( value < 0 )
					value = 0;
				else if ( value > MaxQuantity )
					value = MaxQuantity;

				m_Quantity = value;

				InvalidateProperties();

				int itemID = ComputeItemID();

				if ( itemID > 0 )
					ItemID = itemID;
				else
					Delete();
			}
		}

		public virtual int GetQuantityDescription()
		{
			int perc = ( m_Quantity * 100 ) / MaxQuantity;

			if ( perc <= 0 )
				return 1042975; // It's empty.
			else if ( perc <= 33 )
				return 1042974; // It's nearly empty.
			else if ( perc <= 66 )
				return 1042973; // It's half full.
			else
				return 1042972; // It's full.
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( ShowQuantity )
				list.Add( GetQuantityDescription() );
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );

			if ( ShowQuantity )
				LabelTo( from, GetQuantityDescription() );
		}

		public virtual bool ValidateUse( Mobile from, bool message )
		{
			if ( Deleted )
				return false;

			if ( !Movable && !Fillable )
			{
				Multis.BaseHouse house = Multis.BaseHouse.FindHouseAt( this );

				if ( house == null || !house.IsLockedDown( this ) )
				{
					if ( message )
						from.SendLocalizedMessage( 502946, "", 0x59 ); // That belongs to someone else.

					return false;
				}
			}

			if ( from.Map != Map || !from.InRange( GetWorldLocation(), 2 ) || !from.InLOS( this ) )
			{
				if ( message )
					from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.

				return false;
			}

			return true;
		}

		public virtual void Fill_OnTarget( Mobile from, object targ )
		{
			if ( !IsEmpty || !Fillable || !ValidateUse( from, false ) )
				return;

			if ( targ is BaseBeverage )
			{
				BaseBeverage bev = (BaseBeverage)targ;

				if ( bev.IsEmpty || !bev.ValidateUse( from, true ) )
					return;

				this.Content = bev.Content;
				this.Poison = bev.Poison;
				this.Poisoner = bev.Poisoner;

				if ( bev.Quantity > this.MaxQuantity )
				{
					this.Quantity = this.MaxQuantity;
					bev.Quantity -= this.MaxQuantity;
				}
				else
				{
					this.Quantity += bev.Quantity;
					bev.Quantity = 0;
				}

                if(bev.Disease != Disease.None)
                    this.Disease = bev.Disease;
			}
			
			else if ( targ is StaticTarget )
			{
				StaticTarget stat = targ as StaticTarget;
				
				if( stat.ItemID == 3707 || stat.ItemID == 5453 || stat.ItemID == 4104 || 
				   ( stat.ItemID > 2880 && stat.ItemID < 2885 ) || ( stat.ItemID > 6038 && stat.ItemID < 6067 ) ||
				   stat.ItemID == 13459 || stat.ItemID == 13465 || stat.ItemID == 13471 || stat.ItemID == 13477 )
				{
					if ( !from.InRange( stat.Location, 2 ) || !from.InLOS( stat.Location ) )
					{
						from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
						return;
					}
					
					this.Content = BeverageType.Water;
					this.Poison = null;
					this.Poisoner = null;
                    this.Disease = Disease.None;
	
					this.Quantity = this.MaxQuantity;
	
					from.SendLocalizedMessage( 1010089 ); // You fill the container with water.
				}
			}
			
			else if ( targ is LandTarget )
			{
				LandTarget land = targ as LandTarget;
				
				if( land.TileID > 167 && land.TileID < 172 )
				{
					if ( !from.InRange( land.Location, 2 ) || !from.InLOS( land.Location ) )
					{
						from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
						return;
					}
					
					this.Content = BeverageType.Water;
					this.Poison = null;
					this.Poisoner = null;
                    this.Disease = Disease.None;
	
					this.Quantity = this.MaxQuantity;
	
					from.SendLocalizedMessage( 1010089 ); // You fill the container with water.
				}
			}
			
			else if ( targ is Item )
			{
				Item item = (Item)targ;
				IWaterSource src;

				src = ( item as IWaterSource );

				if ( src == null && item is AddonComponent )
					src = ( ((AddonComponent)item).Addon as IWaterSource );

				if ( src == null || src.Quantity <= 0 )
					return;

				if ( from.Map != item.Map || !from.InRange( item.GetWorldLocation(), 2 ) || !from.InLOS( item ) )
				{
					from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
					return;
				}

				this.Content = BeverageType.Water;
				this.Poison = null;
				this.Poisoner = null;
                this.Disease = Disease.None;

				if ( src.Quantity > this.MaxQuantity )
				{
					this.Quantity = this.MaxQuantity;
					src.Quantity -= this.MaxQuantity;
				}
				else
				{
					this.Quantity += src.Quantity;
					src.Quantity = 0;
				}

				from.SendLocalizedMessage( 1010089 ); // You fill the container with water.
			}
//			else if ( targ is LandTarget )
//			{
//				int tileID = ((LandTarget)targ).TileID;
//
//				PlayerMobile player = from as PlayerMobile;
//
//				if ( player != null )
//				{
//					QuestSystem qs = player.Quest;
//
//					if ( qs is WitchApprenticeQuest )
//					{
//						FindIngredientObjective obj = qs.FindObjective( typeof( FindIngredientObjective ) ) as FindIngredientObjective;
//
//						if ( obj != null && !obj.Completed && obj.Ingredient == Ingredient.SwampWater )
//						{
//							bool contains = false;
//
//							for ( int i = 0; !contains && i < m_SwampTiles.Length; i += 2 )
//								contains = ( tileID >= m_SwampTiles[i] && tileID <= m_SwampTiles[i + 1] );
//
//							if ( contains )
//							{
//								Delete();
// WITCHCRAFT
//								player.SendLocalizedMessage( 1055035 ); // You dip the container into the disgusting swamp water, collecting enough for the Hag's vile stew.
//								obj.Complete();
//							}
//						}
//					}
//				}
//			}
			
			else if ( targ is Cow )
			{
				Cow cow = targ as Cow;
				
				if ( DateTime.Compare( DateTime.Now, ( cow.LastMilking + TimeSpan.FromHours( 2 ) ) ) > 0 )
				{
					this.Quantity = this.MaxQuantity;
					this.Content = BeverageType.Milk;
					cow.LastMilking = DateTime.Now;
                    HealthAttachment.TryContaminate(cow, this);
					from.SendMessage( "You milk the cow and get yourself some fresh, warm milk." );
				}
				
				else
				{
					from.SendMessage( "It is too early to milk this cow again." );
				}
			}
		}

		private static int[] m_SwampTiles = new int[]
			{
				0x9C4, 0x9EB,
				0x3D65, 0x3D65,
				0x3DC0, 0x3DD9,
				0x3DDB, 0x3DDC,
				0x3DDE, 0x3EF0,
				0x3FF6, 0x3FF6,
				0x3FFC, 0x3FFE,
			};

		#region Effects of achohol
		private static Hashtable m_Table = new Hashtable();

		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( EventSink_Login );
		}

		private static void EventSink_Login( LoginEventArgs e )
		{
			CheckHeaveTimer( e.Mobile );
		}

		public static void CheckHeaveTimer( Mobile from )
		{
			if ( from.BAC > 0 && from.Map != Map.Internal && !from.Deleted )
			{
				Timer t = (Timer)m_Table[from];

				if ( t == null )
				{
					if ( from.BAC > 60 )
						from.BAC = 60;

					t = new HeaveTimer( from );
					t.Start();

					m_Table[from] = t;
				}
			}
			else
			{
				Timer t = (Timer)m_Table[from];

				if ( t != null )
				{
					t.Stop();
					m_Table.Remove( from );

					from.SendLocalizedMessage( 500850 ); // You feel sober.
				}
			}
		}

		private class HeaveTimer : Timer
		{
			private Mobile m_Drunk;

			public HeaveTimer( Mobile drunk ) : base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
			{
				m_Drunk = drunk;

				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if ( m_Drunk.Deleted || m_Drunk.Map == Map.Internal )
				{
					Stop();
					m_Table.Remove( m_Drunk );
				}
				else if ( m_Drunk.Alive )
				{
					if ( m_Drunk.BAC > 60 )
						m_Drunk.BAC = 60;

					// chance to get sober
					if ( 10 > Utility.Random( 100 ) )
					--m_Drunk.BAC;

					// lose some stats
					m_Drunk.Stam -= 1;
					m_Drunk.Mana -= 1;

					if ( Utility.Random( 1, 4 ) == 1 )
					{
						//if ( !m_Drunk.Mounted )
						//{
						//	turn in a random direction
						//	m_Drunk.Direction = (Direction)Utility.Random( 8 );
						//
						//	heave
						//	m_Drunk.Animate( 32, 5, 1, true, false, 0 );
						//}

						// *hic*
						m_Drunk.PublicOverheadMessage( Network.MessageType.Regular, 0x3B2, 500849 );

						//if ( m_Drunk.Female = True )
						//{
						//	m_Drunk.PlaySound( 0x31E );
						//}
						//else
						//{
						//	m_Drunk.PlaySound( 0xF2E );
						//}

					}

					if ( m_Drunk.BAC <= 0 )
					{
						Stop();
						m_Table.Remove( m_Drunk );

						m_Drunk.SendLocalizedMessage( 500850 ); // You feel sober.
					}
				}
			}
		}
		#endregion

		public virtual void Pour_OnTarget( Mobile from, object targ )
		{
			if ( IsEmpty || !Pourable || !ValidateUse( from, false ) )
				return;

			if ( targ is BaseBeverage )
			{
				BaseBeverage bev = (BaseBeverage)targ;

				if ( !bev.ValidateUse( from, true ) )
					return;

				if ( bev.IsFull && bev.Content == this.Content )
				{
					from.SendLocalizedMessage( 500848 ); // Couldn't pour it there.  It was already full.
				}
				else if ( !bev.IsEmpty )
				{
					from.SendLocalizedMessage( 500846 ); // Can't pour it there.
				}
				else
				{
					bev.Content = this.Content;
					bev.Poison = this.Poison;
					bev.Poisoner = this.Poisoner;
                    if(this.Disease != Disease.None)
                        bev.Disease = this.Disease;

					if ( this.Quantity > bev.MaxQuantity )
					{
						bev.Quantity = bev.MaxQuantity;
						this.Quantity -= bev.MaxQuantity;
					}
					else
					{
						bev.Quantity += this.Quantity;
						this.Quantity = 0;
					}

					from.PlaySound( 0x4E );
				}
			}
			else if ( from == targ )
			{
				DamageEntry de = from.FindMostRecentDamageEntry( false );
				if ( de != null && DateTime.Compare( DateTime.Now, de.LastDamage + TimeSpan.FromMinutes( 5 ) ) < 0 )
				{
					from.SendMessage( "Your heart is still pounding from the battle. You can't bring yourself to eat right now." );
					return;
				}

// new additions by Alari: fill messages for thirst and no drinking when full.

				/*if ( from.Thirst >= 20 )
				{
					from.SendMessage( "You are simply too full to drink any more!" );
					return;
				}*/

				int fillFactor = 4;	// how much each 'drink' should fill your thirst.

				int iThirst = from.Thirst + fillFactor;

				if ( !(from is PlayerMobile && ((PlayerMobile)from).IsVampire) )
				{
					if( from.Thirst < 20 && from.Stam < from.StamMax )
						from.Stam += Utility.Random( 6, 3 ) + fillFactor / 5; // restore some stamina
	
					if ( iThirst >= 20 )
					{
						from.Thirst = 20;
						from.SendMessage( "You manage to drink the beverage, but you are full!" );
					}
					else
					{
						from.Thirst = iThirst;
	
						if ( iThirst < 5 )
							from.SendMessage( "You drink the beverage, but are still extremely thirsty." );
						else if ( iThirst < 10 )
							from.SendMessage( "You drink the beverage, and begin to feel more satiated." );
						else if ( iThirst < 15 )
							from.SendMessage( "After drinking the beverage, you feel much less thirsty." );
						else
							from.SendMessage( "You feel quite full after consuming the beverage." );
					}
					
					FoodDecayTimer.CalculatePenalty( from );
					
					PoisonedFoodAttachment attachment = XmlAttach.FindAttachment( this, typeof( PoisonedFoodAttachment ) ) as PoisonedFoodAttachment;
					
					if ( attachment != null )
						attachment.OnConsumed( from );
	// end modifications
	
					if ( ContainsAlchohol )
					{
						int bac = 0;
	
						switch ( this.Content )
						{
							case BeverageType.Ale: bac = 1; break;
							case BeverageType.Wine: bac = 2; break;
							case BeverageType.Cider: bac = 3; break;
							case BeverageType.Liquor: bac = 4; break;
						}
	
						from.BAC += bac;
	
						if ( from.BAC > 60 )
							from.BAC = 60;
	
						CheckHeaveTimer( from );
					}
				}

				from.PlaySound( Utility.RandomList( 0x30, 0x2D6 ) );

				if ( m_Poison != null )
					from.ApplyPoison( m_Poisoner, m_Poison );
                if (Disease != Disease.None)
                    HealthAttachment.TrySpreadDiease(Disease, this);

				--Quantity;
			}
			else if ( targ is PlantItem )
            {
				((PlantItem)targ).Pour( from, this );
			}
            else if (targ is FarmSoil)
            {
                (targ as FarmSoil).RefreshSoil(from, 4);
            }
            else if (BaseCrop.IsCrop(targ.GetType().Name))
            {
                IPooledEnumerable eable = (targ as Item).Map.GetItemsInRange((targ as Item).Location, 0);
                foreach (Item i in eable)
                {
                    if (i is FarmSoil && (i as FarmSoil).Plant != null && !(i as FarmSoil).Plant.Deleted)
                    {
                        if ((i as FarmSoil).Plant == targ)
                        {
                            (i as FarmSoil).RefreshSoil(from, 4);
                            continue;
                        }
                    }
                }
                eable.Free();
            }
            else if (BaseCrop.IsPlant(targ.GetType().Name))
            {
                if ((targ as BasePlant).Planted && (targ as BasePlant).Soil != null && !(targ as BasePlant).Soil.Deleted)
                    (targ as BasePlant).Soil.RefreshSoil(from, 4);
            }
            else if ((targ is Cloth || targ is UncutCloth || targ is BoltOfCloth) && !(targ is WetCloth))
            {
                if (targ is Cloth)
                {
                    int hue = (targ as Cloth).Hue;
                    WetCloth wc = new WetCloth();
                    wc.Hue = hue;
                    (targ as Cloth).Consume(1);
                    from.AddToBackpack(wc);
                    from.SendMessage("You wet the cloth, preparing it for use.");
                    from.PlaySound(0x027);
                }
                else if (targ is UncutCloth)
                {
                    int hue = (targ as UncutCloth).Hue;
                    WetCloth wc = new WetCloth();
                    wc.Hue = hue;
                    (targ as UncutCloth).Consume(1);
                    from.AddToBackpack(wc);
                    from.SendMessage("You wet the cloth, preparing it for use.");
                    from.PlaySound(0x027);
                }
                else if (targ is BoltOfCloth)
                {
                    int hue = (targ as Cloth).Hue;
                    WetCloth wc = new WetCloth();
                    wc.Hue = hue;
                    (targ as BoltOfCloth).Consume(1);
                    from.AddToBackpack(wc);
                    from.SendMessage("You wet the cloth, preparing it for use.");
                    from.PlaySound(0x027);
                }

            }
            else
            {
                bool success = false;
                /*if ( targ is ICanPourOn )
                {
                    ICanPourOn pourable = targ as ICanPourOn;
                    success = pourable.OnPoured( from, this );
                }*/

                if (!success)
                    from.SendLocalizedMessage(500846); // Can't pour it there.
            }
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsEmpty )
			{
				if ( !Fillable || !ValidateUse( from, true ) )
					return;

				from.BeginTarget( -1, true, TargetFlags.None, new TargetCallback( Fill_OnTarget ) );
				SendLocalizedMessageTo( from, 500837 ); // Fill from what?
			}
			else if ( Pourable && ValidateUse( from, true ) )
			{
				from.BeginTarget( -1, true, TargetFlags.None, new TargetCallback( Pour_OnTarget ) );
				from.SendLocalizedMessage( 1010086 ); // What do you want to use this on?
			}
		}

		public static bool ConsumeTotal( Container pack, BeverageType content, int quantity )
		{
			return ConsumeTotal( pack, typeof( BaseBeverage ), content, quantity );
		}

		public static bool ConsumeTotal( Container pack, Type itemType, BeverageType content, int quantity )
		{
			Item[] items = pack.FindItemsByType( itemType );

			// First pass, compute total
			int total = 0;

			for ( int i = 0; i < items.Length; ++i )
			{
				BaseBeverage bev = items[i] as BaseBeverage;

				if ( bev != null && bev.Content == content && !bev.IsEmpty )
					total += bev.Quantity;
			}

			if ( total >= quantity )
			{
				// We've enough, so consume it

				int need = quantity;

				for ( int i = 0; i < items.Length; ++i )
				{
					BaseBeverage bev = items[i] as BaseBeverage;

					if ( bev == null || bev.Content != content || bev.IsEmpty )
						continue;

					int theirQuantity = bev.Quantity;

					if ( theirQuantity < need )
					{
						bev.Quantity = 0;
						need -= theirQuantity;
					}
					else
					{
						bev.Quantity -= need;
						return true;
					}
				}
			}

			return false;
		}

		public BaseBeverage()
		{
			ItemID = ComputeItemID();
		}

		public BaseBeverage( BeverageType type )
		{
			m_Content = type;
			m_Quantity = MaxQuantity;
			ItemID = ComputeItemID();
		}

		public BaseBeverage( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version

            writer.Write((int)m_Disease);

			writer.Write( (Mobile) m_Poisoner );

			Poison.Serialize( m_Poison, writer );
			writer.Write( (int) m_Content );
			writer.Write( (int) m_Quantity );
		}

		protected bool CheckType( string name )
		{
			return ( World.LoadingType == String.Format( "Server.Items.{0}", name ) );
		}

		public override void Deserialize( GenericReader reader )
		{
			Deserialize( reader, true );
		}

		public void Deserialize( GenericReader reader, bool read )
		{
			base.Deserialize( reader );

			if ( !read )
				return;

			int version = reader.ReadInt();

			switch ( version )
			{
                case 2:
                {
                    m_Disease = (Disease)reader.ReadInt();
                    goto case 1;
                }
				case 1:
				{
					m_Poisoner = reader.ReadMobile();
					goto case 0;
				}
				case 0:
				{
					m_Poison = Poison.Deserialize( reader );
					m_Content = (BeverageType)reader.ReadInt();
					m_Quantity = reader.ReadInt();
					break;
				}
			}
		}
	}
}
