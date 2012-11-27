using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a rat corpse" )]
    public class Rat : BaseCreature, ISmallPrey, IRodent, ITinyPet
	{
        public static int[] RandomHues = { 2581, 2585, 2591, 2592, 2594, 2674, 2683, 2689, 2822, 2812, 2818, 
                                           2983, 2725, 2724, 2714, 2706, 2813, 2821, 2796, 2797, 2786, 2815, 
                                           2798, 2982, 2966, 2939, 2932, 2840, 2817, 2816, 2814, 2810 };

        public override int MiniatureID { get { return 0x2123; } }
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Rat() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a rat";
			Body = 238;
			BaseSoundID = 0xCC;
            Hue = Utility.RandomAnimalHue();

            if( Utility.RandomMinMax( 1, 10 ) == 10 )
                Hue = RandomHues[Utility.Random( RandomHues.Length )];

			SetStr( 9 );
			SetDex( 35 );
			SetInt( 5 );

			SetHits( 1 );
			SetMana( 0 );

			SetDamage( 2 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 5, 10 );
			SetResistance( ResistanceType.Poison, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 4.0 );
			SetSkill( SkillName.UnarmedFighting, 4.0 );

			Fame = 50;
			Karma = -50;

			VirtualArmor = 6;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 5.0;
		}

		public override int Meat{ get{ return 1; } }
		public override int Bones{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish | FoodType.Eggs | FoodType.GrainsAndHay; } }

		public Rat(Serial serial) : base(serial)
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
