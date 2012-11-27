using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Misc;
using Server.Engines.BulkOrders;
using Server.Regions;
using Server.Factions;

namespace Server.Mobiles
{
	public abstract class CustomVendor : BaseVendor
	{
		public override int GetPriceScalar()
		{
			return 100;
		}

		public override void UpdateBuyInfo()
		{
			base.UpdateBuyInfo();
		}

		public CustomVendor( string title ) : base( title )
		{
		}
		
		public CustomVendor( Serial serial ) : base( serial )
		{
		}

		public override void InitSBInfo()
		{
		}

		public override void LoadSBInfo()
		{
			base.LoadSBInfo();
		}

		public override void Restock()
		{
			base.Restock();
		}

		public override void VendorBuy( Mobile from )
		{
			base.VendorBuy( from );
		}

		public override void SendPacksTo( Mobile from )
		{
			base.SendPacksTo( from );
		}

		public override void VendorSell( Mobile from )
		{
			base.VendorSell( from );
		}

		public override bool OnBuyItems( Mobile buyer, ArrayList list )
		{			
			return base.OnBuyItems( buyer, list );
		}


		public override bool OnSellItems( Mobile seller, ArrayList list )
		{
			return base.OnSellItems( seller, list );
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}
}
