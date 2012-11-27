using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class PureWater : BaseIngredient, IDrinkIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        {
            get
            {
                return new KeyValuePair<CustomEffect, int>[]
                {
                    new KeyValuePair<CustomEffect, int>(CustomEffect.Cure, 40),
                    new KeyValuePair<CustomEffect, int>(CustomEffect.Thirst, 20),
                    new KeyValuePair<CustomEffect, int>(CustomEffect.Dexterity, -40)
                };
            }
        }
        public override int SkillRequired { get { return 400; } }

        bool IDrinkIngredient.CanUse(Mobile mobile)
        {
            return true;
        }

        int IDrinkIngredient.PotionBooster { get { return 0; } } // needs to be processed via feat

        [Constructable]
        public PureWater(int amount)
            : base(3848, amount)
        {
            Name = "Pure Water";
            Hue = 0;
        }

        [Constructable]
        public PureWater()
            : this(1)
        {
        }

        public PureWater(Serial serial)
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
	public class DriedDogbanePlants : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRegeneration, -20),
			}; 
		} }

		public override int SkillRequired { get { return 400; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedDogbanePlants( int amount ) : base( 3973, amount )
		{
			Name = "Dried Dogbane Plants";
			Hue = 2698;
		}

		[Constructable]
		public DriedDogbanePlants() : this( 1 )
		{
		}

		public DriedDogbanePlants( Serial serial ) : base( serial )
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

	public class DriedComfreyFlowers : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRegeneration, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Mana, -20),
			}; 
		} }

		public override int SkillRequired { get { return 400; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedComfreyFlowers( int amount ) : base( 6812, amount )
		{
			Name = "Dried Comfrey Flowers";
			Hue = 1163;
		}

		[Constructable]
		public DriedComfreyFlowers() : this( 1 )
		{
		}

		public DriedComfreyFlowers( Serial serial ) : base( serial )
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

	public class FreshAloeStems : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRestoration, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRegeneration, -20),
			}; 
		} }

		public override int SkillRequired { get { return 400; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public FreshAloeStems( int amount ) : base( 3968, amount )
		{
			Name = "Fresh Aloe Stems";
			Hue = 2595;
		}

		[Constructable]
		public FreshAloeStems() : this( 1 )
		{
		}

		public FreshAloeStems( Serial serial ) : base( serial )
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

	public class DriedGingkoBerries : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPoints, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRestoration, -20),
			}; 
		} }

		public override int SkillRequired { get { return 400; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedGingkoBerries( int amount ) : base( 2512, amount )
		{
			Name = "Dried Gingko Berries";
			Hue = 2587;
		}

		[Constructable]
		public DriedGingkoBerries() : this( 1 )
		{
		}

		public DriedGingkoBerries( Serial serial ) : base( serial )
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

	public class DriedMulleinSeeds : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Cure, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRestoration, -20),
			}; 
		} }

		public override int SkillRequired { get { return 400; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedMulleinSeeds( int amount ) : base( 3873, amount )
		{
			Name = "Dried Mullein Seeds";
			Hue = 1410;
		}

		[Constructable]
		public DriedMulleinSeeds() : this( 1 )
		{
		}

		public DriedMulleinSeeds( Serial serial ) : base( serial )
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

	public class PinonResin : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRestoration, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Intelligence, -20),
			}; 
		} }

		public override int SkillRequired { get { return 400; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public PinonResin( int amount ) : base( 3877, amount )
		{
			Name = "Pinon Resin";
			Hue = 2761;
		}

		[Constructable]
		public PinonResin() : this( 1 )
		{
		}

		public PinonResin( Serial serial ) : base( serial )
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

	public class DriedAlpineSorrelPetals : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRegeneration, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, -20),
			}; 
		} }

		public override int SkillRequired { get { return 400; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedAlpineSorrelPetals( int amount ) : base( 3960, amount )
		{
			Name = "Dried Alpine Sorrel Petals";
			Hue = 2841;
		}

		[Constructable]
		public DriedAlpineSorrelPetals() : this( 1 )
		{
		}

		public DriedAlpineSorrelPetals( Serial serial ) : base( serial )
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

    public class DriedPassionflowerLeaves : BaseIngredient, IDrinkIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        {
            get
            {
                return new KeyValuePair<CustomEffect, int>[] { 
                new KeyValuePair<CustomEffect, int>(CustomEffect.HitPoints, -100),
                new KeyValuePair<CustomEffect, int>(CustomEffect.ImprovedVision, 10)
			};
            }
        }

        public override int SkillRequired { get { return 1000; } }

        bool IDrinkIngredient.CanUse(Mobile mobile)
        {
            return true;
        }

        int IDrinkIngredient.PotionBooster { get { return 0; } }

        [Constructable]
        public DriedPassionflowerLeaves(int amount)
            : base(Utility.RandomList(9036, 9037), amount)
        {
            Name = "Dried Passionflower Leaves";
            Hue = 1163;
        }

        [Constructable]
        public DriedPassionflowerLeaves()
            : this(1)
        {
        }

        public DriedPassionflowerLeaves(Serial serial)
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
}
