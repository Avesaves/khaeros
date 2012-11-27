using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class GiantHairball : BaseIngredient, IDrinkIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        {
            get
            {
                return new KeyValuePair<CustomEffect, int>[]
                {
                    new KeyValuePair<CustomEffect, int>(CustomEffect.Cure, 60),
                    new KeyValuePair<CustomEffect, int>(CustomEffect.Hunger, -40),
                    new KeyValuePair<CustomEffect, int>(CustomEffect.Thirst, -40)
                };
            }
        }
        public override int SkillRequired { get { return 600; } }

        bool IDrinkIngredient.CanUse(Mobile mobile)
        {
            return true;
        }

        int IDrinkIngredient.PotionBooster { get { return 0; } } // needs to be processed via feat

        [Constructable]
        public GiantHairball(int amount)
            : base(3969, amount)
        {
            Name = "A Giant Hairball";
            Hue = 1899;
        }

        [Constructable]
        public GiantHairball()
            : this(1)
        {
        }

        public GiantHairball(Serial serial)
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
	public class DriedGoldensealRoot : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Stamina, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRestoration, -20),
			}; 
		} }

		public override int SkillRequired { get { return 600; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedGoldensealRoot( int amount ) : base( 3974, amount )
		{
			Name = "Dried Goldenseal Root";
			Hue = 2936;
		}

		[Constructable]
		public DriedGoldensealRoot() : this( 1 )
		{
		}

		public DriedGoldensealRoot( Serial serial ) : base( serial )
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

	public class DriedCatsClawSeeds : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Dexterity, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRegeneration, -20),
			}; 
		} }

		public override int SkillRequired { get { return 600; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedCatsClawSeeds( int amount ) : base( 3873, amount )
		{
			Name = "Dried Cat's Claw Seeds";
			Hue = 2851;
		}

		[Constructable]
		public DriedCatsClawSeeds() : this( 1 )
		{
		}

		public DriedCatsClawSeeds( Serial serial ) : base( serial )
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

	public class FrankincenseResin : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRegeneration, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Stamina, -20),
			}; 
		} }

		public override int SkillRequired { get { return 600; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public FrankincenseResin( int amount ) : base( 3877, amount )
		{
			Name = "Frankincense Resin";
			Hue = 2775;
		}

		[Constructable]
		public FrankincenseResin() : this( 1 )
		{
		}

		public FrankincenseResin( Serial serial ) : base( serial )
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

	public class DriedBlueLilyFlowers : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRestoration, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Dexterity, -20),
			}; 
		} }

		public override int SkillRequired { get { return 600; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedBlueLilyFlowers( int amount ) : base( 9908, amount )
		{
			Name = "Dried Blue Lily Flowers";
			Hue = 2961;
		}

		[Constructable]
		public DriedBlueLilyFlowers() : this( 1 )
		{
		}

		public DriedBlueLilyFlowers( Serial serial ) : base( serial )
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

	public class DriedWormwoodPlants : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Mana, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRestoration, -20),
			}; 
		} }

		public override int SkillRequired { get { return 600; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedWormwoodPlants( int amount ) : base( 6812, amount )
		{
			Name = "Dried Wormwood Plants";
			Hue = 2960;
		}

		[Constructable]
		public DriedWormwoodPlants() : this( 1 )
		{
		}

		public DriedWormwoodPlants( Serial serial ) : base( serial )
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

	public class DriedChiaBuds : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Intelligence, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPoints, -20),
			}; 
		} }

		public override int SkillRequired { get { return 600; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedChiaBuds( int amount ) : base( 3577, amount )
		{
			Name = "Dried Chia Buds";
			Hue = 2754;
		}

		[Constructable]
		public DriedChiaBuds() : this( 1 )
		{
		}

		public DriedChiaBuds( Serial serial ) : base( serial )
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

	public class DriedDaturaPetals : BaseIngredient, IOilIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPoints, -20),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Stamina, -20),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Mana, -20)
			}; 
		} }

		public override int SkillRequired { get { return 600; } }

		bool IOilIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IOilIngredient.Corrosivity { get { return 0; } }
		int IOilIngredient.Duration { get { return 0; } }

		[Constructable]
		public DriedDaturaPetals( int amount ) : base( 3960, amount )
		{
			Name = "Dried Datura Petals";
			Hue = 2985;
		}

		[Constructable]
		public DriedDaturaPetals() : this( 1 )
		{
		}

		public DriedDaturaPetals( Serial serial ) : base( serial )
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

	public class DriedFoxgloveFlowers : BaseIngredient, IOilIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, -20),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Dexterity, -20),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Intelligence, -20)
			}; 
		} }

		public override int SkillRequired { get { return 600; } }

		bool IOilIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IOilIngredient.Corrosivity { get { return 0; } }
		int IOilIngredient.Duration { get { return 0; } }

		[Constructable]
		public DriedFoxgloveFlowers( int amount ) : base( 3976, amount )
		{
			Name = "Dried Foxglove Flowers";
			Hue = 626;
		}

		[Constructable]
		public DriedFoxgloveFlowers() : this( 1 )
		{
		}

		public DriedFoxgloveFlowers( Serial serial ) : base( serial )
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
