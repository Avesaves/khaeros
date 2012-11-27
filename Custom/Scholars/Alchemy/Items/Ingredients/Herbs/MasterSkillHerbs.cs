using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class PusantiaRoot : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] {
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return false;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( from is PlayerMobile && from.Backpack != null && this.IsChildOf( from.Backpack ) && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Pusantia) > 1 )
			{
				ProcessedPusantia pus = new ProcessedPusantia( this.Amount );
				from.Backpack.DropItem( pus );
				from.SendMessage( "You have successfuly refined the pusantia." );
				this.Delete();
			}
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } } // needs to be processed via feat

		[Constructable]
		public PusantiaRoot( int amount ) : base( 3182, amount )
		{
			Name = "Pusantia Root";
			Hue = 1401;
		}

		[Constructable]
		public PusantiaRoot() : this( 1 )
		{
		}

		public PusantiaRoot( Serial serial ) : base( serial )
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
	
	public class ProcessedPusantia : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] {
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			if( mobile is PlayerMobile && ((PlayerMobile)mobile).Feats.GetFeatLevel(FeatList.Pusantia) > 2 )
				return true;
			
			return false;
		}

		int IDrinkIngredient.PotionBooster { get { return 20; } }

		[Constructable]
		public ProcessedPusantia( int amount ) : base( 3972, amount )
		{
			Name = "Processed Pusantia";
			Hue = 1401;
		}

		[Constructable]
		public ProcessedPusantia() : this( 1 )
		{
		}

		public ProcessedPusantia( Serial serial ) : base( serial )
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

	public class DriedGingerRoot : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, 10),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Dexterity, 10),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Intelligence, 10)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedGingerRoot( int amount ) : base( 3973, amount )
		{
			Name = "Dried Ginger Root";
			Hue = 1062;
		}

		[Constructable]
		public DriedGingerRoot() : this( 1 )
		{
		}

		public DriedGingerRoot( Serial serial ) : base( serial )
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

	public class DriedRedValerianFlowers : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Paralysis, 20)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 0; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public DriedRedValerianFlowers( int amount ) : base( 3976, amount )
		{
			Name = "Dried Red Valerian Flowers";
			Hue = 2844;
		}

		[Constructable]
		public DriedRedValerianFlowers() : this( 1 )
		{
		}

		public DriedRedValerianFlowers( Serial serial ) : base( serial )
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

	public class CopalGoldResin : BaseIngredient, IBombIngredient, IOilIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Fire, 60)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IOilIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IOilIngredient.Corrosivity { get { return 10; } }
		int IOilIngredient.Duration { get { return 0; } }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 0; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public CopalGoldResin( int amount ) : base( 3873, amount )
		{
			Name = "Copal Gold Resin";
			Hue = 2213;
		}

		[Constructable]
		public CopalGoldResin() : this( 1 )
		{
		}

		public CopalGoldResin( Serial serial ) : base( serial )
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

	public class DriedAgrimonyPetals : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPoints, 10),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Stamina, 10),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Mana, 10)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedAgrimonyPetals( int amount ) : base( 3960, amount )
		{
			Name = "Dried Agrimony Petals";
			Hue = 2698;
		}

		[Constructable]
		public DriedAgrimonyPetals() : this( 1 )
		{
		}

		public DriedAgrimonyPetals( Serial serial ) : base( serial )
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

	public class DriedCliffrosePlants : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRestoration, 60),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Hunger, -40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Thirst, -40)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedCliffrosePlants( int amount ) : base( 3976, amount )
		{
			Name = "Dried Cliffrose Plants";
			Hue = 2213;
		}

		[Constructable]
		public DriedCliffrosePlants() : this( 1 )
		{
		}

		public DriedCliffrosePlants( Serial serial ) : base( serial )
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

	public class DriedSphagnumMoss : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRestoration, 60),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Hunger, -40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Thirst, -40)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DriedSphagnumMoss( int amount ) : base( 3553, amount )
		{
			Name = "Dried Sphagnum Moss";
			Hue = 2589;
		}

		[Constructable]
		public DriedSphagnumMoss() : this( 1 )
		{
		}

		public DriedSphagnumMoss( Serial serial ) : base( serial )
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

	public class DriedLousewortPetals : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Paralysis, 40)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 0; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public DriedLousewortPetals( int amount ) : base( 3960, amount )
		{
			Name = "Dried Lousewort Petals";
			Hue = 2729;
		}

		[Constructable]
		public DriedLousewortPetals() : this( 1 )
		{
		}

		public DriedLousewortPetals( Serial serial ) : base( serial )
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

	public class DriedBelladonnaSeeds : BaseIngredient, IOilIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRegeneration, -20)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IOilIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IOilIngredient.Corrosivity { get { return 2; } }
		int IOilIngredient.Duration { get { return 0; } }

		[Constructable]
		public DriedBelladonnaSeeds( int amount ) : base( 3962, amount )
		{
			Name = "Dried Belladonna Seeds";
			Hue = 2983;
		}

		[Constructable]
		public DriedBelladonnaSeeds() : this( 1 )
		{
		}

		public DriedBelladonnaSeeds( Serial serial ) : base( serial )
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

    // Disease Cures

    public class FairyShroom : BaseIngredient, IDrinkIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        {
            get
            {
                return new KeyValuePair<CustomEffect, int>[] { 
                new KeyValuePair<CustomEffect, int>(CustomEffect.InfluenzaCure, 50),
                new KeyValuePair<CustomEffect, int>(CustomEffect.Confusion, -5)
			};
            }
        }

        public override int SkillRequired { get { return 500; } }

        bool IDrinkIngredient.CanUse(Mobile mobile)
        {
            return true;
        }

        int IDrinkIngredient.PotionBooster { get { return 0; } }

        [Constructable]
        public FairyShroom(int amount)
            : base(2252, amount)
        {
            Name = "Fairy Shroom";
            Hue = 20;
        }

        [Constructable]
        public FairyShroom()
            : this(1)
        {
        }

        public FairyShroom(Serial serial)
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

    public class DriedChaulmoograLeaves : BaseIngredient, IDrinkIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        {
            get
            {
                return new KeyValuePair<CustomEffect, int>[] { 
                new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRegeneration, -100),
                new KeyValuePair<CustomEffect, int>(CustomEffect.Ointment, 10)
			};
            }
        }

        public override int SkillRequired { get { return 900; } }

        bool IDrinkIngredient.CanUse(Mobile mobile)
        {
            return true;
        }

        int IDrinkIngredient.PotionBooster { get { return 0; } }

        [Constructable]
        public DriedChaulmoograLeaves(int amount)
            : base(5922, amount)
        {
            Name = "Dried Chaulmoogra Leaves";
            Hue = 2212;
        }

        [Constructable]
        public DriedChaulmoograLeaves()
            : this(1)
        {
        }

        public DriedChaulmoograLeaves(Serial serial)
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

    public class RefinedMercury : BaseIngredient, IDrinkIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        {
            get
            {
                return new KeyValuePair<CustomEffect, int>[] { 
                new KeyValuePair<CustomEffect, int>(CustomEffect.Madness, 10),
                new KeyValuePair<CustomEffect, int>(CustomEffect.Intelligence, -100),
                new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRegeneration, -100)
			};
            }
        }

        public override int SkillRequired { get { return 900; } }

        bool IDrinkIngredient.CanUse(Mobile mobile)
        {
            return true;
        }

        int IDrinkIngredient.PotionBooster { get { return 0; } }

        [Constructable]
        public RefinedMercury(int amount)
            : base(3620, amount)
        {
            Name = "Refined Mercury";
            Hue = 2983;
        }

        [Constructable]
        public RefinedMercury()
            : this(1)
        {
        }

        public RefinedMercury(Serial serial)
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

    public class CamphorWax : BaseIngredient, IDrinkIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        {
            get
            {
                return new KeyValuePair<CustomEffect, int>[] { 
                new KeyValuePair<CustomEffect, int>(CustomEffect.Confusion, 10),
                new KeyValuePair<CustomEffect, int>(CustomEffect.Thirst, -20),
                new KeyValuePair<CustomEffect, int>(CustomEffect.Paralysis, 10)
			};
            }
        }

        public override int SkillRequired { get { return 900; } }

        bool IDrinkIngredient.CanUse(Mobile mobile)
        {
            return true;
        }

        int IDrinkIngredient.PotionBooster { get { return 0; } }

        [Constructable]
        public CamphorWax(int amount)
            : base(2323, amount)
        {
            Name = "Camphor Wax";
            Hue = 2971;
        }

        [Constructable]
        public CamphorWax()
            : this(1)
        {
        }

        public CamphorWax(Serial serial)
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
