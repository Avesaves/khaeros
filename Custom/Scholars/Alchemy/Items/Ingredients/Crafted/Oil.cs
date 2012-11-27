using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class WateryOil : BaseIngredient, IOilIngredient, IEasyCraft
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] {
			}; 
		} }

		bool IOilIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IOilIngredient.Corrosivity { get { return 0; } }
		int IOilIngredient.Duration { get { return 120; } }

		[Constructable]
		public WateryOil( int amount ) : base( 6192, amount )
		{
			Name = "Watery Oil";
		}

		[Constructable]
		public WateryOil() : this( 1 )
		{
		}

		public WateryOil( Serial serial ) : base( serial )
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

    public class Oil : BaseIngredient, IOilIngredient, IEasyCraft
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] {
			}; 
		} }

		bool IOilIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IOilIngredient.Corrosivity { get { return 0; } }
		int IOilIngredient.Duration { get { return 1200; } }

		[Constructable]
		public Oil( int amount ) : base( 6190, amount )
		{
			Name = "Oil";
		}

		[Constructable]
		public Oil() : this( 1 )
		{
		}

		public Oil( Serial serial ) : base( serial )
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

    public class AdhesiveOil : BaseIngredient, IOilIngredient, IEasyCraft
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] {
			}; 
		} }

		bool IOilIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IOilIngredient.Corrosivity { get { return 0; } }
		int IOilIngredient.Duration { get { return 12000; } }

		[Constructable]
		public AdhesiveOil( int amount ) : base( 6193, amount )
		{
			Name = "Adhesive Oil";
		}

		[Constructable]
		public AdhesiveOil() : this( 1 )
		{
		}

		public AdhesiveOil( Serial serial ) : base( serial )
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
