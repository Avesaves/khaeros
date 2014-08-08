using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
	public abstract class BaseContainer : Container
	{
	
		private string m_LabeledText;
		
		public string LabeledText { get { return m_LabeledText; } set { m_LabeledText = value; InvalidateProperties(); } }
	
		public override int DefaultMaxWeight
		{
			get
			{
				if ( IsSecure )
					return 0;

				return base.DefaultMaxWeight;
			}
		}
		
		public BaseContainer( int itemID ) : base( itemID )
		{
		}
		
		public void DropAndStack( Item dropped )
		{
			List<Item> list = this.Items;

			for ( int i = 0; i < list.Count; ++i )
			{
				Item item = list[i];

				if ( !(item is Container) && item.StackWith( null, dropped, false ) )
					return;
			}

			this.DropItem( dropped );
		}

		public override bool IsAccessibleTo( Mobile m )
		{
			return base.IsAccessibleTo( m );
		}

		public override bool CheckHold( Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			if ( this.IsSecure && !BaseHouse.CheckHold( m, this, item, message, checkItems, plusItems, plusWeight ) )
				return false;

			return base.CheckHold( m, item, message, checkItems, plusItems, plusWeight );
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			if ( !String.IsNullOrEmpty( m_LabeledText ) )
				list.Add( 1060847, "\t<BASEFONT COLOR=#FFFF00>\"{0}\"<BASEFONT COLOR=#FFFFFF> ", m_LabeledText ); // ~1_val~ ~2_val~
			base.GetProperties( list );
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );
			SetSecureLevelEntry.AddTo( from, this, list );
			
			if( ContainerRenamePrompt.HasAccess( from, this ) )
				list.Add( new ContainerRenameEntry( from, this ));
		}

		public override bool TryDropItem( Mobile from, Item dropped, bool sendFullMessage )
		{
			if ( !CheckHold( from, dropped, sendFullMessage, true ) )
				return false;

			BaseHouse house = BaseHouse.FindHouseAt( this );

			if ( house != null && house.IsLockedDown( this ) )
			{
				if ( dropped is VendorRentalContract || ( dropped is Container && ((Container)dropped).FindItemByType( typeof( VendorRentalContract ) ) != null ) )
				{
					from.SendLocalizedMessage( 1062492 ); // You cannot place a rental contract in a locked down container.
					return false;
				}

				if ( !house.LockDown( from, dropped, false ) )
					return false;
			}

			List<Item> list = this.Items;

			for ( int i = 0; i < list.Count; ++i )
			{
				Item item = list[i];

				if ( !(item is Container) && item.StackWith( from, dropped, false ) )
					return true;
			}

			DropItem( dropped );

			return true;
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			if ( !CheckHold( from, item, true, true ) )
				return false;

			BaseHouse house = BaseHouse.FindHouseAt( this );

			/*if ( house != null && house.IsLockedDown( this ) )
			{
				if ( item is VendorRentalContract || ( item is Container && ((Container)item).FindItemByType( typeof( VendorRentalContract ) ) != null ) )
				{
					from.SendLocalizedMessage( 1062492 ); // You cannot place a rental contract in a locked down container.
					return false;
				}

				if ( !house.LockDown( from, item, false ) )
					return false;
			}*/

			item.Location = new Point3D( p.X, p.Y, 0 );
			AddItem( item );

			from.SendSound( GetDroppedSound( item ), GetWorldLocation() );

			return true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel > AccessLevel.Player || from.InRange( this.GetWorldLocation(), 2 ) || this.RootParent is PlayerVendor )
			{
				if( this is ArmourBackpack || this is StrongBackpack || this is Backpack || from.InLOS( this ) )
				{
					Open( from );
				}
			}
			
			else
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
		}

		public virtual void Open( Mobile from )
		{
			DisplayTo( from );
		}

		public BaseContainer( Serial serial ) : base( serial )
		{
		}

		/* Note: base class insertion; we cannot serialize anything here */
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 ); // version
			writer.Write( m_LabeledText );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 0:
				{
					m_LabeledText = reader.ReadString();
					break;
				}
			}
		}
	}

	public class StrongBackpack : Backpack	//Used on Pack animals
	{
        private int m_MaximumWeight;

		[Constructable]
		public StrongBackpack() : this(1600)
		{ }

        [Constructable]
        public StrongBackpack(int x)
        {
            Layer = Layer.Backpack;
            Weight = 3.0;
            m_MaximumWeight = x;
        }

		public override bool CheckHold( Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			return base.CheckHold( m, item, false, checkItems, plusItems, plusWeight );
		}

		public override int DefaultMaxWeight{ get{ return m_MaximumWeight; } }

		public override bool CheckContentDisplay( Mobile from )
		{
			object root = this.RootParent;

			if ( root is BaseCreature && ((BaseCreature)root).Controlled && ((BaseCreature)root).ControlMaster == from )
				return true;

			return base.CheckContentDisplay( from );
		}

		public StrongBackpack( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
            writer.Write((int)m_MaximumWeight);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            switch (version)
            {
                case 1: m_MaximumWeight = reader.ReadInt(); break;
                case 0: m_MaximumWeight = 1600; break;
            }

		}
	}
	
	public class BackpackOfHolding : Backpack	//Used on Treasurers
	{
		[Constructable]
		public BackpackOfHolding()
		{
			Layer = Layer.Backpack;
			Weight = 3.0;
		}

		public override bool CheckHold( Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			return true;
		}

		public BackpackOfHolding( Serial serial ) : base( serial )
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

	public class Backpack : BaseContainer, IDyable
	{
		[Constructable]
		public Backpack() : base( 0xE75 )
		{
			Layer = Layer.Backpack;
			Weight = 3.0;
		}

		public Backpack( Serial serial ) : base( serial )
		{
		}

		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted ) return false;

			Hue = sender.DyedHue;

			return true;
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

			if ( version == 0 && ItemID == 0x9B2 )
				ItemID = 0xE75;
		}
	}

	public class Pouch : TrapableContainer
	{
		[Constructable]
		public Pouch() : base( 0xE79 )
		{
			Weight = 1.0;
		}

		public Pouch( Serial serial ) : base( serial )
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

	public abstract class BaseBagBall : BaseContainer, IDyable
	{
		public BaseBagBall( int itemID ) : base( itemID )
		{
			Weight = 1.0;
		}

		public BaseBagBall( Serial serial ) : base( serial )
		{
		}

		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;

			Hue = sender.DyedHue;

			return true;
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

	public class SmallBagBall : BaseBagBall
	{
		[Constructable]
		public SmallBagBall() : base( 0x2256 )
		{
		}

		public SmallBagBall( Serial serial ) : base( serial )
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

	public class LargeBagBall : BaseBagBall
	{
		[Constructable]
		public LargeBagBall() : base( 0x2257 )
		{
		}

		public LargeBagBall( Serial serial ) : base( serial )
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

	public class Bag : BaseContainer, IDyable
	{
		[Constructable]
		public Bag() : base( 0xE76 )
		{
			Weight = 2.0;
		}

		public Bag( Serial serial ) : base( serial )
		{
		}

		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted ) return false;

			Hue = sender.DyedHue;

			return true;
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
	
	public class BagOfHolding : BaseContainer
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public BagOfHolding() : base( 0xE76 )
		{
			Weight = 2.0;
			Name = "Bag of Holding";
			MaxItems = 50;
			Hue = 2939;
		}

		public BagOfHolding( Serial serial ) : base( serial )
		{
		}
		
		public override bool TryDropItem( Mobile from, Item dropped, bool sendFullMessage )
		{
			if( this.ParentEntity != null )
			{
				from.SendMessage( "Place this container on the ground before adding items to it or removing items from it." );
				return false;
			}
			
			return base.TryDropItem( from, dropped, sendFullMessage );
		}
		
		public override bool CheckLift( Mobile from, Item item, ref LRReason reject )
		{
			if( item != this && this.ParentEntity != null )
			{
				from.SendMessage( "Place this container on the ground before adding items to it or removing items from it." );
				return false;
			}

			return base.CheckLift( from, item, ref reject );
		}
		
		public override int GetTotal( TotalType type )
		{
			if( type == TotalType.Weight )
				return 0;
			
			return base.GetTotal( type );
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
	
	public class PouchOfHolding : TrapableContainer
	{
		public override int MaxWeight{ get{ return 500; } }
		
		[Constructable]
		public PouchOfHolding() : base( 0xE79 )
		{
			Weight = 1.0;
			Name = "Pouch of Holding";
			MaxItems = 25;
			Hue = 2939;
		}

		public PouchOfHolding( Serial serial ) : base( serial )
		{
		}
		
		public override bool TryDropItem( Mobile from, Item dropped, bool sendFullMessage )
		{
			if( this.ParentEntity != null )
			{
				from.SendMessage( "Place this container on the ground before adding items to it or removing items from it." );
				return false;
			}
			
			return base.TryDropItem( from, dropped, sendFullMessage );
		}
		
		public override bool CheckLift( Mobile from, Item item, ref LRReason reject )
		{
			if( item != this && this.ParentEntity != null )
			{
				from.SendMessage( "Place this container on the ground before adding items to it or removing items from it." );
				return false;
			}

			return base.CheckLift( from, item, ref reject );
		}
		
		public override int GetTotal( TotalType type )
		{
			if( type == TotalType.Weight )
				return 0;
			
			return base.GetTotal( type );
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

	public class Barrel : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public Barrel() : base( 0xE77 )
		{
			Weight = 25.0;
		}

		public Barrel( Serial serial ) : base( serial )
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

			if ( Weight == 0.0 )
				Weight = 25.0;
		}
	}

	public class Keg : BaseContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 500; } }
		
		[Constructable]
		public Keg() : base( 0xE7F )
		{
			Weight = 15.0;
		}

		public Keg( Serial serial ) : base( serial )
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

	public class PicnicBasket : BaseContainer, ICanBeStained
	{
		[Constructable]
		public PicnicBasket() : base( 0xE7A )
		{
			Weight = 2.0; // Stratics doesn't know weight
		}

		public PicnicBasket( Serial serial ) : base( serial )
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

	public class Basket : BaseContainer, ICanBeStained
	{
		[Constructable]
		public Basket() : base( 0x990 )
		{
			Weight = 1.0; // Stratics doesn't know weight
		}

		public Basket( Serial serial ) : base( serial )
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
	
	public class BasketWithHandles : BaseContainer, ICanBeStained
	{
		[Constructable]
		public BasketWithHandles() : base( 2476 )
		{
			Weight = 1.0; // Stratics doesn't know weight
		}

		public BasketWithHandles( Serial serial ) : base( serial )
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
	
	public class SmallBasket : BaseContainer, ICanBeStained
	{
		[Constructable]
		public SmallBasket() : base( 2481 )
		{
			Weight = 1.0; // Stratics doesn't know weight
		}

		public SmallBasket( Serial serial ) : base( serial )
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
	
	[FlipableAttribute( 9430, 9429 )]
	public class SquarishBasket : BaseContainer, ICanBeStained
	{
		[Constructable]
		public SquarishBasket() : base( 9429 )
		{
			Weight = 1.0; // Stratics doesn't know weight
		}

		public SquarishBasket( Serial serial ) : base( serial )
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
	
	[FlipableAttribute( 9433, 9434 )]
	public class MediumPicnicBasket : BaseContainer, ICanBeStained
	{
		[Constructable]
		public MediumPicnicBasket() : base( 9433 )
		{
			Weight = 1.0; // Stratics doesn't know weight
		}

		public MediumPicnicBasket( Serial serial ) : base( serial )
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
	
	[FlipableAttribute( 9435, 9436 )]
	public class BambooBasket : BaseContainer, ICanBeStained
	{
		[Constructable]
		public BambooBasket() : base( 9435 )
		{
			Weight = 2.0; // Stratics doesn't know weight
		}

		public BambooBasket( Serial serial ) : base( serial )
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
	[Flipable( 0x9AA, 0xE7D )]
	public class WoodenBox : LockableContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public WoodenBox() : base( 0x9AA )
		{
			Weight = 4.0;
		}

		public WoodenBox( Serial serial ) : base( serial )
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
	[Flipable( 0x9A9, 0xE7E )]
	public class SmallCrate : LockableContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public SmallCrate() : base( 0x9A9 )
		{
			Weight = 2.0;
		}

		public SmallCrate( Serial serial ) : base( serial )
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

			if ( Weight == 4.0 )
				Weight = 2.0;
		}
	}

	[Furniture]
	[Flipable( 0xE3F, 0xE3E )]
	public class MediumCrate : LockableContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 1500; } }
		
		[Constructable]
		public MediumCrate() : base( 0xE3F )
		{
			Weight = 2.0;
		}

		public MediumCrate( Serial serial ) : base( serial )
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

			if ( Weight == 6.0 )
				Weight = 2.0;
		}
	}

	[Furniture]
	[Flipable( 0xE3D, 0xE3C )]
	public class LargeCrate : LockableContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 2000; } }
		
		[Constructable]
		public LargeCrate() : base( 0xE3D )
		{
			Weight = 1.0;
		}

		public LargeCrate( Serial serial ) : base( serial )
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

			if ( Weight == 8.0 )
				Weight = 1.0;
		}
	}

	[DynamicFliping]
	[Flipable( 0x9A8, 0xE80 )]
	public class MetalBox : LockableContainer
	{
		public override int MaxWeight{ get{ return 1000; } }
		
		[Constructable]
		public MetalBox() : base( 0x9A8 )
		{
		}

		public MetalBox( Serial serial ) : base( serial )
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

			if ( version == 0 && Weight == 3 )
				Weight = -1;
		}
	}
	
	[DynamicFliping]
	[Flipable( 0x9AB, 0xE7C )]
	public class Safe : LockableContainer
	{
		public override int MaxWeight{ get{ return 10000; } }
		
		[Constructable]
		public Safe() : base( 0x9AB )
		{
			Name = "Safe";
			LockLevel = 250;
			MaxLockLevel = 250;
			RequiredSkill = 250;
			MaxItems = 25;
			Weight = 50;
		}

		public Safe( Serial serial ) : base( serial )
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

			if ( version < 1 )
			{
				Weight = 50;
				MaxItems = 25;
			}
		}
	}

	[DynamicFliping]
	[Flipable( 0x9AB, 0xE7C )]
	public class MetalChest : LockableContainer
	{
		public override int MaxWeight{ get{ return 2000; } }
		
		[Constructable]
		public MetalChest() : base( 0x9AB )
		{
		}

		public MetalChest( Serial serial ) : base( serial )
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

			if ( version == 0 && Weight == 25 )
				Weight = -1;
		}
	}
    [DynamicFliping]
    [Flipable(0x9AB, 0xE7C)]
    public class tChest : LockableContainer
    {
        public override int MaxWeight { get { return 2000; } }

        [Constructable]
        public tChest()
            : base(0x9AB)
        {
        }

        public tChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && Weight == 25)
                Weight = -1;
        }
    }
	[DynamicFliping]
	[Flipable( 0xE41, 0xE40 )]
	public class MetalGoldenChest : LockableContainer
	{
		public override int MaxWeight{ get{ return 2000; } }
		
		[Constructable]
		public MetalGoldenChest() : base( 0xE41 )
		{
		}

		public MetalGoldenChest( Serial serial ) : base( serial )
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

			if ( version == 0 && Weight == 25 )
				Weight = -1;
		}
	}

	[Furniture]
	[Flipable( 0xe43, 0xe42 )]
	public class WoodenChest : LockableContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 2000; } }
		
		[Constructable]
		public WoodenChest() : base( 0xe43 )
		{
			Weight = 2.0;
		}

		public WoodenChest( Serial serial ) : base( serial )
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

			if ( Weight == 15.0 )
				Weight = 2.0;
		}
	}

	[Furniture]
	[Flipable( 0x280B, 0x280C )]
	public class PlainWoodenChest : LockableContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 2000; } }
		
		[Constructable]
		public PlainWoodenChest() : base( 0x280B )
		{
		}

		public PlainWoodenChest( Serial serial ) : base( serial )
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

			if ( version == 0 && Weight == 15 )
				Weight = -1;
		}
	}

	[Furniture]
	[Flipable( 0x280D, 0x280E )]
	public class OrnateWoodenChest : LockableContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 2000; } }
		
		[Constructable]
		public OrnateWoodenChest() : base( 0x280D )
		{
		}

		public OrnateWoodenChest( Serial serial ) : base( serial )
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

			if ( version == 0 && Weight == 15 )
				Weight = -1;
		}
	}

	[Furniture]
	[Flipable( 0x280F, 0x2810 )]
	public class GildedWoodenChest : LockableContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 2000; } }
		
		[Constructable]
		public GildedWoodenChest() : base( 0x280F )
		{
		}

		public GildedWoodenChest( Serial serial ) : base( serial )
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

			if ( version == 0 && Weight == 15 )
				Weight = -1;
		}
	}

	[Furniture]
	[Flipable( 0x2811, 0x2812 )]
	public class WoodenFootLocker : LockableContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 2000; } }
		
		[Constructable]
		public WoodenFootLocker() : base( 0x2811 )
		{
		}

		public WoodenFootLocker( Serial serial ) : base( serial )
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

			if ( version == 0 && Weight == 15 )
				Weight = -1;
		}
	}

	[Furniture]
	[Flipable( 0x2813, 0x2814 )]
	public class FinishedWoodenChest : LockableContainer, ICanBeStained
	{
		public override int MaxWeight{ get{ return 2000; } }
		
		[Constructable]
		public FinishedWoodenChest() : base( 0x2813 )
		{
		}

		public FinishedWoodenChest( Serial serial ) : base( serial )
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

			if ( version == 0 && Weight == 15 )
				Weight = -1;
		}
	}
}
