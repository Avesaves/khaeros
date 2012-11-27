using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a cat corpse" )]
	[TypeAlias( "Server.Mobiles.Housecat" )]
	public class Cat : BaseCreature, ISmallPredator, IFeline, ISmallPrey, ITinyPet
	{
        public static int[] RandomHues = { 2581, 2585, 2591, 2592, 2594, 2674, 2683, 2689, 2779, 2744, 2736, 
                                           2734, 2725, 2724, 2714, 2706, 2813, 2809, 2796, 2789, 2786, 2785, 
                                           2798, 2982, 2966, 2939, 2932, 2840, 2817, 2816, 2814, 2810 };

        public override int MiniatureID { get { return 0x211b; } }
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Cat() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a cat";
			Body = 0xC9;
			Hue = Utility.RandomAnimalHue();

            if( Utility.RandomMinMax( 1, 10 ) == 10 )
                Hue = RandomHues[Utility.Random( RandomHues.Length )];

			BaseSoundID = 0x69;

			SetStr( 9 );
			SetDex( 35 );
			SetInt( 5 );

			SetHits( 10 );
			SetMana( 0 );

			SetDamage( 3 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 5, 10 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 4.0 );
			SetSkill( SkillName.UnarmedFighting, 5.0 );

			Fame = 50;
			Karma = 150;

			VirtualArmor = 8;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 5.0;

		}

		public override int Meat{ get{ return 1; } }
		public override int Bones{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Feline; } }
		public override bool HasFur { get{ return true; } }

		public Cat(Serial serial) : base(serial)
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
