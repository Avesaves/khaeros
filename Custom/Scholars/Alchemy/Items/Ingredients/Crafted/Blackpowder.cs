using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class WeakBlackPowder : BaseIngredient, IBombIngredient, IEasyCraft
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Explosion, 10)
			}; 
		} }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 2; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public WeakBlackPowder( int amount ) : base( 3983, amount )
		{
			Name = "Weak Black Powder";
			Hue = 923;
		}

		[Constructable]
		public WeakBlackPowder() : this( 1 )
		{
		}

		public WeakBlackPowder( Serial serial ) : base( serial )
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

    public class BlackPowder : BaseIngredient, IBombIngredient, IEasyCraft
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Explosion, 20)
			}; 
		} }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 4; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public BlackPowder( int amount ) : base( 3983, amount )
		{
			Name = "Black Powder";
			Hue = 922;
		}

		[Constructable]
		public BlackPowder() : this( 1 )
		{
		}

		public BlackPowder( Serial serial ) : base( serial )
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

    public class PowerfulBlackPowder : BaseIngredient, IBombIngredient, IEasyCraft
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Explosion, 40)
			}; 
		} }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 6; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public PowerfulBlackPowder( int amount ) : base( 3983, amount )
		{
			Name = "Powerful Black Powder";
			Hue = 921;
		}

		[Constructable]
		public PowerfulBlackPowder() : this( 1 )
		{
		}

		public PowerfulBlackPowder( Serial serial ) : base( serial )
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
