using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class WalrusBlubber : BaseIngredient, IDrinkIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        {
            get
            {
                return new KeyValuePair<CustomEffect, int>[]
                {
                    new KeyValuePair<CustomEffect, int>(CustomEffect.Cure, 20)
                };
            }
        }

        bool IDrinkIngredient.CanUse(Mobile mobile)
        {
            return true;
        }

        int IDrinkIngredient.PotionBooster { get { return 0; } } // needs to be processed via feat

        [Constructable]
        public WalrusBlubber(int amount)
            : base(7818, amount)
        {
            Name = "Walrus Blubber";
            Hue = 0;
        }

        [Constructable]
        public WalrusBlubber()
            : this(1)
        {
        }

        public WalrusBlubber(Serial serial)
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
        }
    }
	public class DriedWillowBark : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRestoration, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedWillowBark( int amount ) : base( 3553, amount )
		{
			Name = "Dried Willow Bark";
			Hue = 960;
		}

		[Constructable]
		public DriedWillowBark() : this( 1 )
		{
		}

		public DriedWillowBark( Serial serial ) : base( serial )
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

	public class DriedWolfLichen : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRegeneration, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedWolfLichen( int amount ) : base( 3184, amount )
		{
			Name = "Dried Wolf Lichen";
			Hue = 2971;
		}

		[Constructable]
		public DriedWolfLichen() : this( 1 )
		{
		}

		public DriedWolfLichen( Serial serial ) : base( serial )
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

	public class DriedMarshMallowFlowers : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPoints, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedMarshMallowFlowers( int amount ) : base( 3184, amount )
		{
			Name = "Dried Marsh Mallow Flowers";
			Hue = 2856;
		}

		[Constructable]
		public DriedMarshMallowFlowers() : this( 1 )
		{
		}

		public DriedMarshMallowFlowers( Serial serial ) : base( serial )
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

	public class MyrrhResin : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public MyrrhResin( int amount ) : base( 3877, amount )
		{
			Name = "Myrrh Resin";
			Hue = 2600;
		}

		[Constructable]
		public MyrrhResin() : this( 1 )
		{
		}

		public MyrrhResin( Serial serial ) : base( serial )
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

	public class DriedYarrowFlowers : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Cure, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedYarrowFlowers( int amount ) : base( 6812, amount )
		{
			Name = "Dried Yarrow Flowers";
			Hue = 2985;
		}

		[Constructable]
		public DriedYarrowFlowers() : this( 1 )
		{
		}

		public DriedYarrowFlowers( Serial serial ) : base( serial )
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

	public class DriedSkullcapButtons : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Intelligence, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedSkullcapButtons( int amount ) : base( 3699, amount )
		{
			Name = "Dried Skullcap Buttons";
			Hue = 2112;
		}

		[Constructable]
		public DriedSkullcapButtons() : this( 1 )
		{
		}

		public DriedSkullcapButtons( Serial serial ) : base( serial )
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

	public class DriedHyssopPetals : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Mana, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedHyssopPetals( int amount ) : base( 3960, amount )
		{
			Name = "Dried Hyssop Petals";
			Hue = 2735;
		}

		[Constructable]
		public DriedHyssopPetals() : this( 1 )
		{
		}

		public DriedHyssopPetals( Serial serial ) : base( serial )
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

	public class DriedPurslaneStems : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRestoration, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedPurslaneStems( int amount ) : base( 3978, amount )
		{
			Name = "Dried Purslane Stems";
			Hue = 2959;
		}

		[Constructable]
		public DriedPurslaneStems() : this( 1 )
		{
		}

		public DriedPurslaneStems( Serial serial ) : base( serial )
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

	public class DriedDamianaPetals : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRegeneration, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedDamianaPetals( int amount ) : base( 3960, amount )
		{
			Name = "Dried Damiana Petals";
			Hue = 773;
		}

		[Constructable]
		public DriedDamianaPetals() : this( 1 )
		{
		}

		public DriedDamianaPetals( Serial serial ) : base( serial )
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

	public class DriedCinchonaBuds : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRestoration, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedCinchonaBuds( int amount ) : base( 2512, amount )
		{
			Name = "Dried Cinchona Buds";
			Hue = 2602;
		}

		[Constructable]
		public DriedCinchonaBuds() : this( 1 )
		{
		}

		public DriedCinchonaBuds( Serial serial ) : base( serial )
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

	public class DriedEchinaceaBuds : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRegeneration, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedEchinaceaBuds( int amount ) : base( 2512, amount )
		{
			Name = "Dried Echinacea Buds";
			Hue = 178;
		}

		[Constructable]
		public DriedEchinaceaBuds() : this( 1 )
		{
		}

		public DriedEchinaceaBuds( Serial serial ) : base( serial )
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

	public class DriedJuniperBerries : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Stamina, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedJuniperBerries( int amount ) : base( 2513, amount )
		{
			Name = "Dried Juniper Berries";
			Hue = 38;
		}

		[Constructable]
		public DriedJuniperBerries() : this( 1 )
		{
		}

		public DriedJuniperBerries( Serial serial ) : base( serial )
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

	public class DriedDesertSageLeaves : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Dexterity, 20)
			}; 
		} }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedDesertSageLeaves( int amount ) : base( 3976, amount )
		{
			Name = "Dried Desert Sage Leaves";
			Hue = 1072;
		}

		[Constructable]
		public DriedDesertSageLeaves() : this( 1 )
		{
		}

		public DriedDesertSageLeaves( Serial serial ) : base( serial )
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
