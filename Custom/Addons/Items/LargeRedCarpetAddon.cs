/////////////////////////////////////////////////
//                                             //
// Automatically generated by the              //
// AddonGenerator script by Arya               //
//                                             //
/////////////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AG_LargeRedCarpetAddon : BaseAddon, IDyable
	{
        public virtual bool Dye( Mobile from, DyeTub sender )
        {
            return true;
        }

		public override BaseAddonDeed Deed
		{
			get
			{
				return new AG_LargeRedCarpetAddonDeed();
			}
		}

		[ Constructable ]
		public AG_LargeRedCarpetAddon( int hue )
		{
			AddonComponent ac;
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, -2, -2, 0 );
			ac = new AddonComponent( 2760 );
			ac.Hue = hue;
			AddComponent( ac, -2, -1, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, -2, 0, 0 );
			ac = new AddonComponent( 2760 );
			ac.Hue = hue;
			AddComponent( ac, -2, 1, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, -2, 2, 0 );
			ac = new AddonComponent( 2760 );
			ac.Hue = hue;
			AddComponent( ac, -1, -2, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, -1, 1, 0 );
			ac = new AddonComponent( 2760 );
			ac.Hue = hue;
			AddComponent( ac, -1, 2, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, 0, -2, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 2760 );
			ac.Hue = hue;
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, 0, 2, 0 );
			ac = new AddonComponent( 2760 );
			ac.Hue = hue;
			AddComponent( ac, 1, -2, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 2760 );
			ac.Hue = hue;
			AddComponent( ac, 1, 2, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, 2, -2, 0 );
			ac = new AddonComponent( 2760 );
			ac.Hue = hue;
			AddComponent( ac, 2, -1, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, 2, 0, 0 );
			ac = new AddonComponent( 2760 );
			ac.Hue = hue;
			AddComponent( ac, 2, 1, 0 );
			ac = new AddonComponent( 2759 );
			ac.Hue = hue;
			AddComponent( ac, 2, 2, 0 );
			ac = new AddonComponent( 2761 );
			ac.Hue = hue;
			AddComponent( ac, 3, 3, 0 );
			ac = new AddonComponent( 2762 );
			ac.Hue = hue;
			AddComponent( ac, -3, -3, 0 );
			ac = new AddonComponent( 2763 );
			ac.Hue = hue;
			AddComponent( ac, -3, 3, 0 );
			ac = new AddonComponent( 2764 );
			ac.Hue = hue;
			AddComponent( ac, 3, -3, 0 );
			ac = new AddonComponent( 2765 );
			ac.Hue = hue;
			AddComponent( ac, -3, -2, 0 );
			ac = new AddonComponent( 2765 );
			ac.Hue = hue;
			AddComponent( ac, -3, -1, 0 );
			ac = new AddonComponent( 2765 );
			ac.Hue = hue;
			AddComponent( ac, -3, 0, 0 );
			ac = new AddonComponent( 2765 );
			ac.Hue = hue;
			AddComponent( ac, -3, 1, 0 );
			ac = new AddonComponent( 2765 );
			ac.Hue = hue;
			AddComponent( ac, -3, 2, 0 );
			ac = new AddonComponent( 2766 );
			ac.Hue = hue;
			AddComponent( ac, -2, -3, 0 );
			ac = new AddonComponent( 2766 );
			ac.Hue = hue;
			AddComponent( ac, -1, -3, 0 );
			ac = new AddonComponent( 2766 );
			ac.Hue = hue;
			AddComponent( ac, 0, -3, 0 );
			ac = new AddonComponent( 2766 );
			ac.Hue = hue;
			AddComponent( ac, 1, -3, 0 );
			ac = new AddonComponent( 2766 );
			ac.Hue = hue;
			AddComponent( ac, 2, -3, 0 );
			ac = new AddonComponent( 2767 );
			ac.Hue = hue;
			AddComponent( ac, 3, -2, 0 );
			ac = new AddonComponent( 2767 );
			ac.Hue = hue;
			AddComponent( ac, 3, -1, 0 );
			ac = new AddonComponent( 2767 );
			ac.Hue = hue;
			AddComponent( ac, 3, 0, 0 );
			ac = new AddonComponent( 2767 );
			ac.Hue = hue;
			AddComponent( ac, 3, 1, 0 );
			ac = new AddonComponent( 2767 );
			ac.Hue = hue;
			AddComponent( ac, 3, 2, 0 );
			ac = new AddonComponent( 2768 );
			ac.Hue = hue;
			AddComponent( ac, -2, 3, 0 );
			ac = new AddonComponent( 2768 );
			ac.Hue = hue;
			AddComponent( ac, -1, 3, 0 );
			ac = new AddonComponent( 2768 );
			ac.Hue = hue;
			AddComponent( ac, 0, 3, 0 );
			ac = new AddonComponent( 2768 );
			ac.Hue = hue;
			AddComponent( ac, 1, 3, 0 );
			ac = new AddonComponent( 2768 );
			ac.Hue = hue;
			AddComponent( ac, 2, 3, 0 );

		}

		public AG_LargeRedCarpetAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class AG_LargeRedCarpetAddonDeed : BaseAddonDeed, IDyable
	{
		public override BaseAddon Addon
		{
			get
			{
				return new AG_LargeRedCarpetAddon( this.Hue );
			}
		}
		
		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
		}

		[Constructable]
		public AG_LargeRedCarpetAddonDeed()
		{
			Name = "Large Red Carpet Deed";
		}

		public AG_LargeRedCarpetAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
