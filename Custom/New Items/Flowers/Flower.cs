using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.Alchemy;
using Server.ContextMenus;

namespace Server.Items
{
	public abstract class Flower : Item
	{
		public override double DefaultWeight{ get { return 0.1; } }
		
		public virtual int FlowerID { get { return 0; } }
		public virtual int BouquetID { get { return 0; } }

		public Flower( int itemID ) : this( itemID, 1 )
		{
		}

		public Flower( int itemID, int amount ) : base( itemID )
		{
			Stackable = false;
			Amount = amount;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( this.RootParent == from )
			{
				if ( ItemID == FlowerID )
				{
					ItemID = BouquetID;
					Layer = Layer.OneHanded;
				}
				else if ( from.Backpack != null )
				{
					if ( this.Parent == from )
						from.Backpack.DropItem( this );
					ItemID = FlowerID;
					Layer = Layer.Invalid;
					InvalidateProperties();
				}
			}
		}
		
		public Flower( Serial serial ) : base( serial )
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
}