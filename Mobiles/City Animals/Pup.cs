using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a Pup corpse" )]
	public class Pup : BaseCreature, ISmallPrey, ITinyPet
	{
        public static int[] RandomHues = { 2581, 2585, 2591, 2592, 2594, 2674, 2683, 2689, 2822, 2812, 2818, 
                                           2734, 2725, 2724, 2714, 2706, 2813, 2821, 2796, 2797, 2786, 2815, 
                                           2798, 2982, 2966, 2939, 2932, 2840, 2817, 2816, 2814, 2810 };

		public override bool ParryDisabled{ get{ return true; } }
        [Constructable]
		public Pup() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a pup";
			Body = 0xD9;
			Hue = Utility.RandomAnimalHue();

            if( Utility.RandomMinMax( 1, 10 ) == 10 )
                Hue = RandomHues[Utility.Random( RandomHues.Length )];

			BaseSoundID = 0x85;

			SetStr( 27, 37 );
			SetDex( 28, 43 );
			SetInt( 35 );

			SetHits( 25 );
			SetMana( 0 );

			SetDamage( 4 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 10, 15 );
			SetResistance( ResistanceType.Piercing, 5, 10 );
			SetResistance( ResistanceType.Slashing, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 19.2, 31.0 );
			SetSkill( SkillName.UnarmedFighting, 19.2, 31.0 );

			Fame = 50;
			Karma = 100;

			VirtualArmor = 12;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 5.0;

		}

		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public Pup(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
